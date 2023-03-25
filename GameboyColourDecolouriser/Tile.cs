using GameboyColourDecolouriser.Models;
using System.Text;

namespace GameboyColourDecolouriser
{
    /// <summary>
    /// Represents a tile.
    /// </summary>
    public class Tile : ITile
    {
        private readonly Colour[,] _colourMap;
        private readonly HashSet<Colour> _colours;
        private Lazy<int> _hash;
        private readonly int _x;
        private readonly int _y;

        /// <inheritdoc/>
        public int ColourHash => _hash.Value;

        /// <inheritdoc/>
        public HashSet<Colour> Colours => _colours;

        /// <inheritdoc/>
        public int X => _x;

        /// <inheritdoc/>
        public int Y => _y;

        /// <inheritdoc/>
        public Colour[,] ColourMap => _colourMap;

        /// <summary>
        ///  Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="colourMap">The <see cref="Colour[,]"/> representing the pixel colours for the <see cref="Tile"/>.</param>
        /// <param name="x">The x coordinate of the tile on the greater image.</param>
        /// <param name="y">The y coordinate of the tile on the greater image.</param>
        public Tile(Colour[,] colourMap, int x, int y)
        {
            _colourMap = colourMap;
            _x = x;
            _y = y;
            _colours = new HashSet<Colour>();
            _hash = new Lazy<int>(GenerateHash); // hm, this should be ensured to only be called after a load

            foreach (var ((i, j), colour) in _colourMap.ToIEnumerableWithCoords())
            {
                _colours.Add(colour);
            }
        }

        /// <inheritdoc/>
        public Colour this[int x, int y]
        {
            get => _colourMap[x, y];
        }

        private int GenerateHash()
        {
            // i couldnt work out how to make a hash that worked :/
            // couldnt get it working with adding in i and j values and bit shifting
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < _colourMap.GetLength(0); i++)
            {
                for (int j = 0; j < _colourMap.GetLength(1); j++)
                {
                    stringBuilder.Append(_colourMap[i, j]);
                }
            }

            return stringBuilder.ToString().GetHashCode();
        }
    }
}
