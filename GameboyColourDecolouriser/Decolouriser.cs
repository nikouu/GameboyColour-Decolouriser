using GameboyColourDecolouriser.Models;

namespace GameboyColourDecolouriser
{
    /// <summary>
    /// Contains logic to decolourise a <seealso cref="GbcImage"/> to a <seealso cref="GbImage"/>.
    /// </summary>
    public class Decolouriser
    {
        private SpectreTasks? _spectreTasks;

        /// <summary>
        /// Decolourises a given <seealso cref="GbcImage"/>.
        /// </summary>
        /// <param name="gbcImage">The <seealso cref="GbcImage"/> to decolour.</param>
        /// <param name="spectreTasks"></param>
        /// <returns>A decoloured <seealso cref="GbImage"/>.</returns>
        public GbImage Decolourise(GbcImage gbcImage, SpectreTasks? spectreTasks = null)
        {
            _spectreTasks = spectreTasks;

            var decolouredTiles = Process(gbcImage);

            // todo: redo this when properly moved to new gb model
            var gbImage = new GbImage(gbcImage.Width, gbcImage.Height, decolouredTiles);

            return gbImage;
        }

        /// <summary>
        /// Decolourses a <seealso cref="GbcImage"/>.
        /// </summary>
        /// <param name="decolouredImage"></param>
        /// <returns>A <seealso cref="T:DecolouredTile[,]"/>.</returns>
        private DecolouredTile[,] Process(GbcImage gbcImage)
        {
            var decolouredImage = new DecolouredImage(gbcImage);

            DecolourBasedOnFourColourTiles(decolouredImage);

            // With no guesswork involved in recolouring 4 tile (and exact subsets), we can do some guesswork in mapping all the rest of the colours.
            var mostUsedGbColoursPerRealColourDictionary = GetWeightedGbColoursByTrueColours(decolouredImage);

            // relying on Deferred Execution, useful for chained together multiple queries that result in different manipulations of the dataset.
            var unfinishedTiles = decolouredImage.Tiles.ToIEnumerable().Where(x => !x.IsFullyDecoloured);

            DecolourBasedOnTransparentTiles(unfinishedTiles);
            DecolourBasedOnExistingTileColours(mostUsedGbColoursPerRealColourDictionary, unfinishedTiles);
            DecolourBasedOnNearestSimilarColours(decolouredImage, unfinishedTiles);

            return decolouredImage.Tiles;
        }

        /// <summary>
        /// Four colour tiles are easy to map to the four GameBoy greens due to the one to one brightness relationship. 
        /// This also allows decolouring of less-than-four colour tiles if they have an exact subset of the four colour tile.
        /// </summary>
        /// <param name="decolouredImage"></param>
        private void DecolourBasedOnFourColourTiles(DecolouredImage decolouredImage)
        {
            var tiles = decolouredImage.Tiles.ToIEnumerable().OrderByDescending(x => x.GBCColourCount).ToList();

            if (!tiles.Any(x => x.GBCColours.Count == 4))
            {
                // no 4 colour tiles
                return;
            }

            // Creates a group of tiles by number of colours in colour count descending order. (I.e. 4 colour tile group, 3 colour, 2 then 1).
            var tileGroups = tiles.GroupBy(x => x.GBCColourCount).ToList();

            foreach (var tileGroup in tileGroups)
            {
                foreach (var tile in tileGroup)
                {
                    if (decolouredImage.ContainsTile(tile.OriginalTileHash))
                    {
                        // If we have already decoloured an exact copy of this tile, we know how to decolour this one.
                        var colourDictionaryKey = decolouredImage.TileDictionary[tile.OriginalTileHash];
                        var colourDictionaryValue = decolouredImage.TileColourDictionary[colourDictionaryKey];
                        ProcessFromExistingTileDictionary(tile, colourDictionaryValue);
                    }
                    else if (decolouredImage.ContainsColour(tile.ColourKeyString))
                    {
                        // If another tile uses the same four colours, we can use the same mapping for this tile.
                        var colourDictionaryValue = decolouredImage.TileColourDictionary[tile.ColourKeyString];
                        ProcessFromExistingTileDictionary(tile, colourDictionaryValue);
                    }
                    else if (tile.GBCColourCount == 4)
                    {
                        // We can easily map the four GBC colours to the four GB colours.
                        ProcessFourColours(tile);

                        // Since we have newly mapped GBC -> GB colours, we update the cache.
                        UpdateImageDictionaryCaches(decolouredImage, tile);
                    }
                    else
                    {
                        if (tileGroups.Any(x => x.Key == tileGroup.Key + 1))
                        {
                            // todo: can this where have another where which only does completed tiles? or do we also rely on incompleted tiles for this?
                            var compareToTiles = tileGroups.Where(x => x.Key == tileGroup.Key + 1).First().Where(x => x.IsFullyDecoloured);

                            // Attempt to process the current tile with any tile that +1 colour in it - this tile might be an exact subset of one of them.                            
                            ProcessFromSimilarMoreColouredTiles(tile, compareToTiles);
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

        /// <summary>
        /// With the <seealso cref="DecolouredImage"/>, find the most popular mappings of a GBC -> GB colour. Somewhat guestimate work.
        /// </summary>
        /// <param name="decolouredImage">The decoloured work so far.</param>
        /// <returns></returns>
        private static Dictionary<Colour, Colour> GetWeightedGbColoursByTrueColours(DecolouredImage decolouredImage)
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

        /// <summary>
        /// Find all transparent tiles and set them to <seealso cref="Colour.GBBlack"/>.
        /// </summary>
        /// <param name="unfinishedTiles"><seealso cref="IEnumerable{DecolouredTile}"/> of tiles to check for transparent tiles.</param>
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

        /// <summary>
        /// Attempt to recolour each given <seealso cref="DecolouredImage"/> based on a given <seealso cref="Dictionary{Colour, Colour}"/> for mapping GBC -> GB colours.
        /// </summary>
        /// <param name="mostUsedGbColoursPerRealColourDictionary"></param>
        /// <param name="unfinishedTiles"></param>
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

        /// <summary>
        /// Attempt to colour any remaining unfinished tiles by looking at all existing coloured tiles, and if not, make a best estimate.
        /// </summary>
        /// <param name="decolouredImage">The decoloured image so far.</param>
        /// <param name="unfinishedTiles">Remaining unfinished tiles.</param>
        private void DecolourBasedOnNearestSimilarColours(DecolouredImage decolouredImage, IEnumerable<DecolouredTile> unfinishedTiles)
        {
            foreach (var unfinishedTile in unfinishedTiles)
            {
                if (decolouredImage.ContainsTile(unfinishedTile.OriginalTileHash))
                {
                    // If we have already decoloured an exact copy of this tile, we know how to decolour this one.
                    var colourDictionaryKey = decolouredImage.TileDictionary[unfinishedTile.OriginalTileHash];
                    var colourDictionaryValue = decolouredImage.TileColourDictionary[colourDictionaryKey];
                    ProcessFromExistingTileDictionary(unfinishedTile, colourDictionaryValue);
                }
                else if (decolouredImage.ContainsColour(unfinishedTile.ColourKeyString))
                {
                    // If another tile uses the same four colours, we can use the same mapping for this tile.
                    var colourDictionaryValue = decolouredImage.TileColourDictionary[unfinishedTile.ColourKeyString];
                    ProcessFromExistingTileDictionary(unfinishedTile, colourDictionaryValue);
                }
                else
                {
                    ProcessBasedOnBestNearestEstimate(decolouredImage, unfinishedTiles, unfinishedTile);
                }

                _spectreTasks?.decolourStageFour.Increment(((double)1 / unfinishedTiles.Count()) * 100);
            }
        }

        /// <summary>
        /// Goes through each pixel in a tile to make the best guess on what that pixel could be.
        /// </summary>
        /// <param name="decolouredImage"></param>
        /// <param name="unfinishedTiles"></param>
        /// <param name="unfinishedTile"></param>
        private void ProcessBasedOnBestNearestEstimate(DecolouredImage decolouredImage, IEnumerable<DecolouredTile> unfinishedTiles, DecolouredTile unfinishedTile)
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

        /// <summary>
        /// Update the caches for: 
        /// 1. For every unique tile, map the colour key.
        /// 2. For every different tile colour key, map the translated GBC -> GB colours.      
        /// This allows us to get the colour key for any tile and allows us to quickly get the translated colours for a given GBC colour key. 
        /// Tile -> colour key -> colour map.
        /// </summary>
        /// <param name="decolouredImage"></param>
        /// <param name="tile"></param>
        private void UpdateImageDictionaryCaches(DecolouredImage decolouredImage, DecolouredTile tile)
        {
            // for this tile, we can associate a key with it so if there are identical tiles, we know which translation to use
            decolouredImage.TileDictionary.TryAdd(tile.OriginalTileHash, tile.ColourKeyString);

            // for this translated dictionary, we have a unique key, and the mappings for GBC to GB colours associated with it
            decolouredImage.TileColourDictionary.TryAdd(tile.ColourKeyString, tile.GetTranslatedDictionary);
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

        /// <summary>
        /// Attempt to decolour a tile by looking for any already processed tile that has +1 number of colours, in the hope of finding
        /// a tile that is an exact superset of this tile such that it is easy to map the colours.
        /// </summary>
        /// <param name="decolouredTile">The tile being attempted to decolour.</param>
        /// <param name="tiles">All the <seealso cref="IEnumerable{DecolouredTile}"/> that have +1 colour of the given <seealso cref="DecolouredTile"/>.</param>
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

        /// <summary>
        /// Directly map each GBC -> GB colour based on brightness.
        /// </summary>
        /// <param name="decolouredTile"></param>
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
