using GameboyColourDecolouriser.Models;
using System.Text;

namespace GameboyColourDecolouriser
{
    public class Tile : ITile
    {
        private readonly Colour[,] _colourMap;
        private readonly HashSet<Colour> _colours;
        private Lazy<int> _hash;
        private readonly int _x;
        private readonly int _y;

        public int ColourHash => _hash.Value;
        public HashSet<Colour> Colours => _colours;
        public int X => _x;
        public int Y => _y;
        public Colour[,] ColourMap => _colourMap;

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
