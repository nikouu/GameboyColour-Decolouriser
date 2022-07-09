using GameboyColourDecolouriser.Models;
using Spectre.Console;

namespace GameboyColourDecolouriser
{
    public class Decolouriser
    {
        private SpectreTasks? _spectreTasks;

        public GbImage Decolourise(GbcImage gbcImage, SpectreTasks? spectreTasks = null)
        {
            _spectreTasks = spectreTasks;
            var recolouredImage = new RecolouredImage(gbcImage);            

            var recolouredTiles = Process(recolouredImage);

            // todo: redo this when properly moved to new gb model
            var gbImage = new GbImage(gbcImage.Width, gbcImage.Height, recolouredTiles);

            return gbImage;
        }

        private RecolouredTile[,] Process(RecolouredImage recolouredImage)
        {
            RecolourBasedOnFourColourTiles(recolouredImage);

            // imagine actually writing this horror of a linq statement
            var mostUsedGbColoursPerRealColourDictionary = recolouredImage.TileColourDictionary
                .SelectMany(x => x.Value)
                .OrderByDescending(x => x.Key.GetBrightness())
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, y => y.GroupBy(x => x.Value).OrderByDescending(x => x.Key).First().First().Value);

            var unfinishedTiles = recolouredImage.Tiles.ToIEnumerable().Where(x => !x.IsFullyRecoloured).ToList();
            RecolourBasedOnTransparentTiles(unfinishedTiles);

            unfinishedTiles = unfinishedTiles.Where(x => !x.IsFullyRecoloured).ToList();
            RecolourBasedOnExistingTileColours(mostUsedGbColoursPerRealColourDictionary, unfinishedTiles);

            unfinishedTiles = unfinishedTiles.Where(x => !x.IsFullyRecoloured).ToList();
            RecolourBasedOnNearestSimilarColours(recolouredImage, unfinishedTiles);

            return recolouredImage.Tiles;
        }

        private void RecolourBasedOnNearestSimilarColours(RecolouredImage recolouredImage, List<RecolouredTile> unfinishedTiles)
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
                    foreach (var ((i, j), colour) in unfinishedTile.ToIEnumerable())
                    {
                        var currentOriginalColour = unfinishedTile.OriginalTileColourMap[i, j];

                        if (colour.IsDefault && (unfinishedTile.IsFullyRecoloured || unfinishedTile.ColourDictionaryCopy.ContainsKey(currentOriginalColour)))
                        {
                            // the colour has been found in a previous loop, and now its time to apply that colour to the remaining pixels
                            unfinishedTile[i, j] = unfinishedTile.ColourDictionary(unfinishedTile.OriginalTileColourMap[i, j]);
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
                            _spectreTasks?.decolourStageFour.Increment(((double)1 / unfinishedTiles.Count) * 100);
                            continue;
                        }
                    }
                    UpdateImageDictionaryCaches(recolouredImage, unfinishedTile);
                }

                _spectreTasks?.decolourStageFour.Increment(((double)1 / unfinishedTiles.Count) * 100);
            }
        }

        private void RecolourBasedOnExistingTileColours(Dictionary<Colour, Colour> mostUsedGbColoursPerRealColourDictionary, List<RecolouredTile> unfinishedTiles)
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

                _spectreTasks?.decolourStageThree.Increment(((double)1 / unfinishedTiles.Count) * 100);
            }
        }

        private void RecolourBasedOnTransparentTiles(List<RecolouredTile> unfinishedTiles)
        {
            foreach (var unfinishedTile in unfinishedTiles)
            {
                if (unfinishedTile.OriginalColourCount != 1)
                {
                    _spectreTasks?.decolourStageTwo.Increment(((double)1 / unfinishedTiles.Count) * 100);
                    continue;
                }

                var singleColour = unfinishedTile.OriginalColours.First();

                if (singleColour.IsBlank)
                {
                    foreach (var ((i, j), colour) in unfinishedTile.ToIEnumerable())
                    {
                        unfinishedTile[i, j] = Colour.GBBlack;
                    }
                }

                _spectreTasks?.decolourStageTwo.Increment(((double)1 / unfinishedTiles.Count) * 100);
            }
        }

        private void RecolourBasedOnFourColourTiles(RecolouredImage recolouredImage)
        {
            var tiles = recolouredImage.Tiles.ToIEnumerable().OrderByDescending(x => x.OriginalColourCount).ToList();

            var tileGroups = tiles.GroupBy(x => x.OriginalColourCount).ToList();

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
                    else if (tile.OriginalColourCount == 4)
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
                AnsiConsole.WriteLine("ayylmao");
                _spectreTasks?.decolourStageOne.Increment(((double)1 / tileGroups.Count) * 100);
            }
        }

        // does this function actually do anything with all the variables bewfore the tryadds?
        private void UpdateImageDictionaryCaches(RecolouredImage recolouredImage, RecolouredTile tile)
        {
            var tileColourDictionaryKeys = tile.OriginalColours.OrderBy(x => x.GetBrightness());
            var tileColourDictionaryValues = new List<Colour>();

            foreach (var tileColour in tileColourDictionaryKeys)
            {
                tileColourDictionaryValues.Add(tile.ColourDictionary(tileColour));
            }

            recolouredImage.TileColourDictionary.TryAdd(tile.ColourKeyString, tile.ColourDictionaryCopy);
            recolouredImage.TileDictionary.TryAdd(tile.OriginalTileHash, tile.ColourKeyString);
        }

        private void ProcessFromExistingTileDictionary(RecolouredTile recolouredTile, Dictionary<Colour, Colour> dictionary)
        {
            foreach (var ((i, j), colour) in recolouredTile.ToIEnumerable())
            {
                var originalColour = recolouredTile.OriginalTileColourMap[i, j];
                recolouredTile[i, j] = dictionary[originalColour];
            }
        }

        private void ProcessFromSimilarMoreColouredTiles(RecolouredTile recolouredTile, IEnumerable<RecolouredTile> tiles)
        {
            // go through the recolouredImage object to find something that has the same colours.      

            foreach (var tile in tiles)
            {
                if (tile.OriginalColourCount == tile.Colours.Count && tile.OriginalColours.ContainsAll(recolouredTile.OriginalColours))
                {
                    // found an n+1 colour tile that has all the n colours of this
                    foreach (var ((i, j), colour) in recolouredTile.ToIEnumerable())
                    {
                        var originalColour = recolouredTile.OriginalTileColourMap[i, j];
                        recolouredTile[i, j] = tile.ColourDictionary(originalColour);
                    }

                    return;
                }
            }
        }

        private void ProcessFourColours(RecolouredTile recolouredTile)
        {
            var possibleGBColours = new List<Colour> { Colour.GBWhite, Colour.GBLight, Colour.GBDark, Colour.GBBlack };
            var lightestToDarkestColours = recolouredTile.OriginalColours.OrderByDescending(x => x.GetBrightness()).ToList();

            var localColourMap = lightestToDarkestColours.Zip(possibleGBColours, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);

            foreach (var ((i, j), colour) in recolouredTile.ToIEnumerable())
            {
                recolouredTile[i, j] = localColourMap[recolouredTile.OriginalTileColourMap[i, j]];
            }
        }
    }
}
