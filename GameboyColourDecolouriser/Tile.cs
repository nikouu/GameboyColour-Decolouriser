﻿using System.Drawing;
using System.Text;

namespace GameboyColourDecolouriser
{
    public class Tile : ITile
    {
        private readonly Color[,] _colourMap;
        private readonly HashSet<Color> _colours;
        private Lazy<int> _hash;
        private Point _coordinate;

        public int ColourHash => _hash.Value;
        public HashSet<Color> Colours => _colours;
        public Point Coordinate => _coordinate;
        public Color[,] ColourMap => _colourMap;

        public Tile(Bitmap tile, int x, int y)
        {

            _colourMap = new Color[8, 8];
            _colours = new HashSet<Color>();
            _hash = new Lazy<int>(() => GenerateHash()); // hm, this should be ensured to only be called after a load
            LoadTile(tile, x, y);

        }

        public Color this[int x, int y]
        {
            get => _colourMap[x, y];
        }

        private void LoadTile(Bitmap tile, int x, int y)
        {
            if (tile.Width > 8 || tile.Height > 8)
            {
                throw new ArgumentException(nameof(tile));
            }

            _coordinate = new Point(x, y);

            for (int i = 0; i < tile.Width; i++)
            {
                for (int j = 0; j < tile.Height; j++)
                {
                    var colour = tile.GetPixel(i, j);

                    _colourMap[i, j] = colour;
                    _colours.Add(colour);
                }
            }

            if (_colours.Count > 4)
            {
                throw new InvalidOperationException($"More than 4 colours found in tile: ({x}, {y}).");
            }
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