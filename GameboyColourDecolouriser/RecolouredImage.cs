using System.Drawing;

namespace GameboyColourDecolouriser
{
    public class RecolouredImage
    {
        private readonly GbImage _originalImage;

        private readonly RecolouredTile[,] _recolouredTiles;

        // Keys being the colours in the tile, ordered by brightness, and values being the gb colours, in the same order
        public Dictionary<string, Dictionary<Color, Color>> TileColourDictionary;

        // Keys being the "hash" of the tile. Allows quick lookup for Colour dictionary for previously seen identical tiles
        public Dictionary<int, string> TileDictionary;


        public RecolouredImage(GbImage originalImage)
        {
            _originalImage = originalImage;
            _recolouredTiles = new RecolouredTile[_originalImage.Width / 8, _originalImage.Height / 8];
            TileColourDictionary = new Dictionary<string, Dictionary<Color, Color>>();
            TileDictionary = new Dictionary<int, string>();

            SetupTiles();
        }

        public RecolouredTile[,] Tiles => _recolouredTiles;

        public bool ContainsTile(int key) => TileDictionary.ContainsKey(key);

        public bool ContainsColour(string colourHash) => TileColourDictionary.ContainsKey(colourHash);

        public ITile[,] OriginalTiles => _originalImage.Tiles;

        private void SetupTiles()
        {
            var originalTileIterator = _originalImage.Tiles.ToIEnumerable();
            foreach (var originalTile in originalTileIterator)
            {
                _recolouredTiles[originalTile.Coordinate.X, originalTile.Coordinate.Y] = new RecolouredTile(originalTile);
            }
        }
    }
}
