using GameboyColourDecolouriser.Models;
using Spectre.Console;
using System.Collections.Generic;

namespace GameboyColourDecolouriser
{
    public class Decolouriser
    {
        private SpectreTasks? _spectreTasks;

        // could return two things:
        // the gb image
        // and a palette map on info on how to colour the gbimage
        // like it has a list of palettes, with IDs based on the minimal set of palettes you could have per image
        // then it has an array of each tile and what palette is used ot colour it
        public GbImage Decolourise(GbcImage gbcImage, SpectreTasks? spectreTasks = null)
        {
            _spectreTasks = spectreTasks;
            var recolouredImage = new RecolouredImage(gbcImage);            

            var recolouredTiles = Process(recolouredImage);

            // todo: redo this when properly moved to new gb model
            var gbImage = new GbImage(gbcImage.Width, gbcImage.Height, recolouredTiles);

            return gbImage;
        }

        private DecolouredTile[,] Process(RecolouredImage recolouredImage)
        {
            RecolourBasedOnFourColourTiles(recolouredImage);
            var mostUsedGbColoursPerRealColourDictionary = GetWeightedGbColoursByTrueColours(recolouredImage);

            // relying on Deferred Execution, useful for chained together multiple queries that result in different manipulations of the dataset.
            var unfinishedTiles = recolouredImage.Tiles.ToIEnumerable().Where(x => !x.IsFullyRecoloured);

            RecolourBasedOnTransparentTiles(unfinishedTiles);
            RecolourBasedOnExistingTileColours(mostUsedGbColoursPerRealColourDictionary, unfinishedTiles);
            RecolourBasedOnNearestSimilarColours(recolouredImage, unfinishedTiles);

            return recolouredImage.Tiles;
        }

        private static Dictionary<Colour, Colour> GetWeightedGbColoursByTrueColours(RecolouredImage recolouredImage)
        {
            // imagine actually writing this horror of a linq statement
            return recolouredImage.TileColourDictionary
                .SelectMany(x => x.Value)
                .OrderByDescending(x => x.Key.GetBrightness())
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, y => y.GroupBy(x => x.Value).OrderByDescending(x => x.Key).First().First().Value);
        }

        private void RecolourBasedOnNearestSimilarColours(RecolouredImage recolouredImage, IEnumerable<DecolouredTile> unfinishedTiles)
        {
            foreach (var unfinishedTile in unfinishedTiles)
            {
                if (recolouredImage.ContainsTile(unfinishedTile.OriginalTileHash))
                {
                    var colourDictionaryKey = recolouredImage.TileDictionary[unfinishedTile.OriginalTileHash];
                    ProcessFromExistingTileDictionary(unfinishedTile, recolouredImage.TileColourDictionary[colourDictionaryKey]);
                }
                else if (recolouredImage.ContainsColour(unfinishedTile.ColourKeyString))
                {
                    ProcessFromExistingTileDictionary(unfinishedTile, recolouredImage.TileColourDictionary[unfinishedTile.ColourKeyString]);
                }
                else
                {
                    ProcessBasedOnBestNearestEstimate(recolouredImage, unfinishedTiles, unfinishedTile);
                }

                _spectreTasks?.decolourStageFour.Increment(((double)1 / unfinishedTiles.Count()) * 100);
            }
        }

        private void ProcessBasedOnBestNearestEstimate(RecolouredImage recolouredImage, IEnumerable<DecolouredTile> unfinishedTiles, DecolouredTile unfinishedTile)
        {
            foreach (var ((i, j), colour) in unfinishedTile.ToIEnumerable())
            {
                var currentOriginalColour = unfinishedTile.OriginalTileColourMap[i, j];

                if (colour.IsDefault && (unfinishedTile.IsFullyRecoloured || unfinishedTile.IsColourTranslated(currentOriginalColour)))
                {
                    // the colour has been found in a previous loop, and now its time to apply that colour to the remaining pixels
                    unfinishedTile[i, j] = unfinishedTile.GetGBColour(unfinishedTile.OriginalTileColourMap[i, j]);
                }
                else if (colour.IsDefault && !unfinishedTile.IsFullyRecoloured)
                {
                    var remainingColourOptions = new List<Colour> { Colour.GBWhite, Colour.GBLight, Colour.GBDark, Colour.GBBlack }.Except(unfinishedTile.Colours).ToDictionary(x => x.GetBrightness(), z => z);

                    var currentOriginalColourBrightness = currentOriginalColour.GetBrightness();

                    var closest = remainingColourOptions.OrderBy(x => Math.Abs(currentOriginalColourBrightness - x.Key)).First();

                    unfinishedTile[i, j] = closest.Value;
                    // still more colours to find
                }
                else
                {
                    // pixel is good to go already
                    _spectreTasks?.decolourStageFour.Increment(((double)1 / unfinishedTiles.Count()) * 100);
                    continue;
                }
            }
            UpdateImageDictionaryCaches(recolouredImage, unfinishedTile);
        }

        private void RecolourBasedOnExistingTileColours(Dictionary<Colour, Colour> mostUsedGbColoursPerRealColourDictionary, IEnumerable<DecolouredTile> unfinishedTiles)
        {
            foreach (var unfinishedTile in unfinishedTiles)
            {
                foreach (var ((i, j), colour) in unfinishedTile.ToIEnumerable())
                {
                    var currentOriginalColour = unfinishedTile.OriginalTileColourMap[i, j];

                    if (mostUsedGbColoursPerRealColourDictionary.ContainsKey(currentOriginalColour))
                    {
                        unfinishedTile[i, j] = mostUsedGbColoursPerRealColourDictionary[currentOriginalColour];
                    }
                }

                _spectreTasks?.decolourStageThree.Increment(((double)1 / unfinishedTiles.Count()) * 100);
            }
        }

        private void RecolourBasedOnTransparentTiles(IEnumerable<DecolouredTile> unfinishedTiles)
        {
            foreach (var unfinishedTile in unfinishedTiles)
            {
                if (unfinishedTile.GBCColourCount != 1)
                {
                    _spectreTasks?.decolourStageTwo.Increment(((double)1 / unfinishedTiles.Count()) * 100);
                    continue;
                }

                var singleColour = unfinishedTile.GBCColours.First();

                if (singleColour.IsBlank)
                {
                    foreach (var ((i, j), colour) in unfinishedTile.ToIEnumerable())
                    {
                        unfinishedTile[i, j] = Colour.GBBlack;
                    }
                }

                _spectreTasks?.decolourStageTwo.Increment(((double)1 / unfinishedTiles.Count()) * 100);
            }
        }

        private void RecolourBasedOnFourColourTiles(RecolouredImage recolouredImage)
        {
            var tiles = recolouredImage.Tiles.ToIEnumerable().OrderByDescending(x => x.GBCColourCount).ToList();

            if (!tiles.Any(x => x.GBCColours.Count == 4))
            {
                // no 4 colour tiles
                return;
            }

            var tileGroups = tiles.GroupBy(x => x.GBCColourCount).ToList();

            foreach (var tileGroup in tileGroups)
            {
                foreach (var tile in tileGroup)
                {
                    if (recolouredImage.ContainsTile(tile.OriginalTileHash))
                    {
                        var colourDictionaryKey = recolouredImage.TileDictionary[tile.OriginalTileHash];
                        ProcessFromExistingTileDictionary(tile, recolouredImage.TileColourDictionary[colourDictionaryKey]);
                    }
                    else if (recolouredImage.ContainsColour(tile.ColourKeyString))
                    {
                        ProcessFromExistingTileDictionary(tile, recolouredImage.TileColourDictionary[tile.ColourKeyString]);
                    }
                    else if (tile.GBCColourCount == 4)
                    {
                        ProcessFourColours(tile);
                        UpdateImageDictionaryCaches(recolouredImage, tile);
                    }
                    else
                    {
                        ProcessFromSimilarMoreColouredTiles(tile, tileGroups.Where(x => x.Key == tileGroup.Key + 1).First());

                        if (tile.Colours.Count == 0)
                        {
                            continue;
                        }

                        UpdateImageDictionaryCaches(recolouredImage, tile);
                    }
                }
                _spectreTasks?.decolourStageOne.Increment(((double)1 / tileGroups.Count) * 100);
            }
        }

        // does this function actually do anything with all the variables bewfore the tryadds?
        private void UpdateImageDictionaryCaches(RecolouredImage recolouredImage, DecolouredTile tile)
        {
            recolouredImage.TileColourDictionary.TryAdd(tile.ColourKeyString, tile.GetTranslatedDictionary);
            recolouredImage.TileDictionary.TryAdd(tile.OriginalTileHash, tile.ColourKeyString);
        }

        private void ProcessFromExistingTileDictionary(DecolouredTile recolouredTile, Dictionary<Colour, Colour> dictionary)
        {
            foreach (var ((i, j), colour) in recolouredTile.ToIEnumerable())
            {
                var originalColour = recolouredTile.OriginalTileColourMap[i, j];
                recolouredTile[i, j] = dictionary[originalColour];
            }
        }

        private void ProcessFromSimilarMoreColouredTiles(DecolouredTile recolouredTile, IEnumerable<DecolouredTile> tiles)
        {
            // go through the recolouredImage object to find something that has the same colours.      

            foreach (var tile in tiles)
            {
                if (tile.GBCColourCount == tile.Colours.Count && tile.GBCColours.ContainsAll(recolouredTile.GBCColours))
                {
                    // found an n+1 colour tile that has all the n colours of this
                    foreach (var ((i, j), colour) in recolouredTile.ToIEnumerable())
                    {
                        var originalColour = recolouredTile.OriginalTileColourMap[i, j];
                        recolouredTile[i, j] = tile.GetGBColour(originalColour);
                    }

                    return;
                }
            }
        }

        private void ProcessFourColours(DecolouredTile recolouredTile)
        {
            var possibleGBColours = new List<Colour> { Colour.GBWhite, Colour.GBLight, Colour.GBDark, Colour.GBBlack };
            var lightestToDarkestColours = recolouredTile.GBCColours.OrderByDescending(x => x.GetBrightness()).ToList();

            var localColourMap = lightestToDarkestColours.Zip(possibleGBColours, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);

            foreach (var ((i, j), colour) in recolouredTile.ToIEnumerable())
            {
                recolouredTile[i, j] = localColourMap[recolouredTile.OriginalTileColourMap[i, j]];
            }
        }
    }
}
