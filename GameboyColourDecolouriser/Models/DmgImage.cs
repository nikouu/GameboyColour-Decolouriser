namespace GameboyColourDecolouriser.Models
{
    public abstract class DmgImage
    {
        private readonly int _width;
        private readonly int _height;
        private readonly ITile[,] _tiles;

        public ITile[,] Tiles => _tiles;
        public int Width => _width;
        public int Height => _height;

        public DmgImage(int width, int height, ITile[,] tiles)
        {
            _width = width;
            _height = height;
            _tiles = tiles;
        }
    }
}
