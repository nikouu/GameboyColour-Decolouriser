using GameboyColourDecolouriser.Models;

namespace GameboyColourDecolouriser
{
    /// <summary>
    /// Represents a GBC image being decoloured to a GB image with associated data.
    /// </summary>
    public class DecolouredImage
    {
        private readonly GbcImage _originalImage;

        private readonly DecolouredTile[,] _decolouredTiles;

        /// <summary>
        /// Lookup for a known decoloured <seealso cref="Colour"/> key for a <seealso cref="Tile"/> via the <seealso cref="Tile"/> hash. 
        /// </summary>
        public Dictionary<int, string> TileDictionary = new();

        /// <summary>
        /// Lookup for a <see cref="Dictionary{Colour, Colour}"/> via a string representing the tile <seealso cref="Colour"/>.
        /// </summary>
        public Dictionary<string, Dictionary<Colour, Colour>> TileColourDictionary = new();

        /// <summary>
        /// The tiles being decoloured for this image.
        /// </summary>
        public DecolouredTile[,] Tiles => _decolouredTiles;

        /// <summary>
        /// Determines whether the <see cref="DecolouredImage"/> contains a specific <see cref="DecolouredTile"/> based on the <seealso cref="Tile"/> hash.
        /// </summary>
        /// <param name="key">The <seealso cref="Tile"/> hash.</param>
        /// <returns>True if the <seealso cref="Tile"/> hash exists in the <see cref="DecolouredImage"/>, false otherwise.</returns>
        public bool ContainsTile(int key) => TileDictionary.ContainsKey(key);

        /// <summary>
        /// Determines whether the <see cref="DecolouredImage"/> contains a specific <see cref="Colour"/> based on the <seealso cref="Colour"/> hash.
        /// </summary>
        /// <param name="colourHash">The <seealso cref="Colour"/> hash.</param>
        /// <returns>True if the <seealso cref="Colour"/> hash exists in the <see cref="DecolouredImage"/>, false otherwise.</returns>
        public bool ContainsColour(string colourHash) => TileColourDictionary.ContainsKey(colourHash);

        /// <summary>
        /// The original GBC tiles.
        /// </summary>
        public ITile[,] OriginalTiles => _originalImage.Tiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecolouredImage"/> class.
        /// </summary>
        /// <param name="originalImage">The original image to setup to decolourise.</param>
        public DecolouredImage(GbcImage originalImage)
        {
            _originalImage = originalImage;
            _decolouredTiles = new DecolouredTile[_originalImage.Width / 8, _originalImage.Height / 8];
            SetupTiles();
        }

        /// <summary>
        /// Sets up the 2D array of <see cref="DecolouredTile"/> objects based on the original tiles.
        /// </summary>
        private void SetupTiles()
        {
            var originalTileIterator = _originalImage.Tiles.ToIEnumerable();
            foreach (var originalTile in originalTileIterator)
            {
                _decolouredTiles[originalTile.X, originalTile.Y] = new DecolouredTile(originalTile);
            }
        }
    }
}
