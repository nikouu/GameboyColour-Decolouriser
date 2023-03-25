using GameboyColourDecolouriser.Models;

namespace GameboyColourDecolouriser
{
    public class DeolouredImage
    {
        private readonly GbcImage _originalImage;

        private readonly DecolouredTile[,] _decolouredTiles;

        // Keys being the colours in the tile, ordered by brightness, and values being the gb colours, in the same order
        public Dictionary<string, Dictionary<Colour, Colour>> TileColourDictionary = new();

        // Keys being the "hash" of the tile. Allows quick lookup for Colour dictionary for previously seen identical tiles
        public Dictionary<int, string> TileDictionary = new();

        public DecolouredTile[,] Tiles => _decolouredTiles;

        public bool ContainsTile(int key) => TileDictionary.ContainsKey(key);

        public bool ContainsColour(string colourHash) => TileColourDictionary.ContainsKey(colourHash);

        public ITile[,] OriginalTiles => _originalImage.Tiles;

        public DeolouredImage(GbcImage originalImage)
        {
            _originalImage = originalImage;
            _decolouredTiles = new DecolouredTile[_originalImage.Width / 8, _originalImage.Height / 8];
            SetupTiles();
        }

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
