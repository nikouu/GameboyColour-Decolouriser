using GameboyColourDecolouriser.Models;

namespace GameboyColourDecolouriser
{
    public class Decolouriser
    {
        private SpectreTasks? _spectreTasks;

        public GbImage Decolourise(GbcImage gbcImage, SpectreTasks? spectreTasks = null)
        {
            _spectreTasks = spectreTasks;
            var decolouredImage = new DeolouredImage(gbcImage);

            var decolouredTiles = Process(decolouredImage);

            // todo: redo this when properly moved to new gb model
            var gbImage = new GbImage(gbcImage.Width, gbcImage.Height, decolouredTiles);

            return gbImage;
        }

        private DecolouredTile[,] Process(DeolouredImage decolouredImage)
        {
            DecolourBasedOnFourColourTiles(decolouredImage);
            var mostUsedGbColoursPerRealColourDictionary = GetWeightedGbColoursByTrueColours(decolouredImage);

            // relying on Deferred Execution, useful for chained together multiple queries that result in different manipulations of the dataset.
            var unfinishedTiles = decolouredImage.Tiles.ToIEnumerable().Where(x => !x.IsFullyDecoloured);

            DecolourBasedOnTransparentTiles(unfinishedTiles);
            DecolourBasedOnExistingTileColours(mostUsedGbColoursPerRealColourDictionary, unfinishedTiles);
            DecolourBasedOnNearestSimilarColours(decolouredImage, unfinishedTiles);

            return decolouredImage.Tiles;
        }

        private static Dictionary<Colour, Colour> GetWeightedGbColoursByTrueColours(DeolouredImage decolouredImage)
        {
            // decolouredImage is the work so far. which at the moment is just all the 4 colour tiles
            // the result will be each GBC colour and its most popular GB colour it has been mapped to

            // this gives every key (GBC) to converted value (GB)
            var step1 = decolouredImage.TileColourDictionary.SelectMany(x => x.Value);

            // then order the all these key value pairs by brightness of the GBC colour (which i forgot why brightness was so key to this project lol)
            var step2 = step1.OrderByDescending(x => x.Key.GetBrightness());

            // then for every GBC colour, they get grouped together, with all the different GB colour values
            var step3 = step2.GroupBy(x => x.Key);

            // gets the most common GB colour value out per GBC colour
            var step4 = step3.ToDictionary(k => k.Key, v => v.GroupBy(x => x.Value).OrderByDescending(x => x.Count()).First().First().Value);

            return step4;
        }

        private void DecolourBasedOnNearestSimilarColours(DeolouredImage decolouredImage, IEnumerable<DecolouredTile> unfinishedTiles)
        {
            foreach (var unfinishedTile in unfinishedTiles)
            {
                if (decolouredImage.ContainsTile(unfinishedTile.OriginalTileHash))
                {
                    var colourDictionaryKey = decolouredImage.TileDictionary[unfinishedTile.OriginalTileHash];
                    ProcessFromExistingTileDictionary(unfinishedTile, decolouredImage.TileColourDictionary[colourDictionaryKey]);
                }
                else if (decolouredImage.ContainsColour(unfinishedTile.ColourKeyString))
                {
                    ProcessFromExistingTileDictionary(unfinishedTile, decolouredImage.TileColourDictionary[unfinishedTile.ColourKeyString]);
                }
                else
                {
                    ProcessBasedOnBestNearestEstimate(decolouredImage, unfinishedTiles, unfinishedTile);
                }

                _spectreTasks?.decolourStageFour.Increment(((double)1 / unfinishedTiles.Count()) * 100);
            }
        }

        // Goes through each pixel in a tile to make the best guess on what that pixel could be
        private void ProcessBasedOnBestNearestEstimate(DeolouredImage decolouredImage, IEnumerable<DecolouredTile> unfinishedTiles, DecolouredTile unfinishedTile)
        {
            // remember, colour here is what is on the /unfinished/ tile we are creating with GB colours, so it will have missing colours at first
            foreach (var ((i, j), colour) in unfinishedTile.ToIEnumerable())
            {
                var currentOriginalColour = unfinishedTile.OriginalTileColourMap[i, j];

                if (colour.IsDefault && (unfinishedTile.IsFullyDecoloured || unfinishedTile.IsColourTranslated(currentOriginalColour)))
                {
                    // the colour has been found in a previous loop, and now its time to apply that colour to the remaining pixels
                    unfinishedTile[i, j] = unfinishedTile.GetGBColour(unfinishedTile.OriginalTileColourMap[i, j]);
                }
                else if (colour.IsDefault && !unfinishedTile.IsFullyDecoloured)
                {
                    var remainingColourOptions = Colour.GbColourList.Except(unfinishedTile.Colours).ToDictionary(x => x.GetBrightness(), z => z);

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
            UpdateImageDictionaryCaches(decolouredImage, unfinishedTile);
        }

        private void DecolourBasedOnExistingTileColours(Dictionary<Colour, Colour> mostUsedGbColoursPerRealColourDictionary, IEnumerable<DecolouredTile> unfinishedTiles)
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

        private void DecolourBasedOnTransparentTiles(IEnumerable<DecolouredTile> unfinishedTiles)
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

        private void DecolourBasedOnFourColourTiles(DeolouredImage decolouredImage)
        {
            var tiles = decolouredImage.Tiles.ToIEnumerable().OrderByDescending(x => x.GBCColourCount).ToList();

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
                    if (decolouredImage.ContainsTile(tile.OriginalTileHash))
                    {
                        var colourDictionaryKey = decolouredImage.TileDictionary[tile.OriginalTileHash];
                        ProcessFromExistingTileDictionary(tile, decolouredImage.TileColourDictionary[colourDictionaryKey]);
                    }
                    else if (decolouredImage.ContainsColour(tile.ColourKeyString))
                    {
                        ProcessFromExistingTileDictionary(tile, decolouredImage.TileColourDictionary[tile.ColourKeyString]);
                    }
                    else if (tile.GBCColourCount == 4)
                    {
                        ProcessFourColours(tile);
                        UpdateImageDictionaryCaches(decolouredImage, tile);
                    }
                    else
                    {
                        if (tileGroups.Any(x => x.Key == tileGroup.Key + 1))
                        {
                            ProcessFromSimilarMoreColouredTiles(tile, tileGroups.Where(x => x.Key == tileGroup.Key + 1).First());
                        }

                        if (tile.Colours.Count == 0)
                        {
                            continue;
                        }

                        UpdateImageDictionaryCaches(decolouredImage, tile);
                    }
                }
                _spectreTasks?.decolourStageOne.Increment(((double)1 / tileGroups.Count) * 100);
            }
        }

        // does this function actually do anything with all the variables bewfore the tryadds?
        private void UpdateImageDictionaryCaches(DeolouredImage decolouredImage, DecolouredTile tile)
        {
            // for this translated dictionary, we have a unique key, and the mappings for GBC to GB colours associated with it
            decolouredImage.TileColourDictionary.TryAdd(tile.ColourKeyString, tile.GetTranslatedDictionary);

            // then for this tile, we can associate a key with it so if there are identical tiles, we know which translation to use
            decolouredImage.TileDictionary.TryAdd(tile.OriginalTileHash, tile.ColourKeyString);
        }

        // we have seen the identical tile before, so we know how to process it
        private void ProcessFromExistingTileDictionary(DecolouredTile decolouredTile, Dictionary<Colour, Colour> dictionary)
        {
            foreach (var ((i, j), colour) in decolouredTile.ToIEnumerable())
            {
                var originalColour = decolouredTile.OriginalTileColourMap[i, j];
                decolouredTile[i, j] = dictionary[originalColour];
            }
        }

        private void ProcessFromSimilarMoreColouredTiles(DecolouredTile decolouredTile, IEnumerable<DecolouredTile> tiles)
        {
            // go through the decolouredImage object to find something that has the same colours.      

            foreach (var tile in tiles)
            {
                if (tile.GBCColourCount == tile.Colours.Count && tile.GBCColours.ContainsAll(decolouredTile.GBCColours))
                {
                    // found an n+1 colour tile that has all the n colours of this
                    foreach (var ((i, j), colour) in decolouredTile.ToIEnumerable())
                    {
                        var originalColour = decolouredTile.OriginalTileColourMap[i, j];
                        decolouredTile[i, j] = tile.GetGBColour(originalColour);
                    }

                    return;
                }
            }
        }

        private void ProcessFourColours(DecolouredTile decolouredTile)
        {
            var lightestToDarkestColours = decolouredTile.GBCColours.OrderByDescending(x => x.GetBrightness()).ToList();

            var localColourMap = lightestToDarkestColours.Zip(Colour.GbColourList, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);

            // decolour each pixel of the decoloured tile as we have mapped the GBC to the GB colours based on brightness
            foreach (var ((i, j), colour) in decolouredTile.ToIEnumerable())
            {
                decolouredTile[i, j] = localColourMap[decolouredTile.OriginalTileColourMap[i, j]];
            }
        }
    }
}
