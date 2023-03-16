using GameboyColourDecolouriser.Models;
using System.Text;

namespace GameboyColourDecolouriser
{
    public class RecolouredTile : ITile
    {
        private readonly Tile _originalTile;
        private readonly Colour[,] _gbColourMap;
        private readonly HashSet<Colour> _gbColours = new();
        // key is original colour and it maps to a GB colour
        private readonly Dictionary<Colour, Colour> _colourDictionary = new();
        private Lazy<int> _originalColourCount;
        private readonly Lazy<int> _originalTileHash;
        private Lazy<string> _colourKeyString;

        public int ColourHash => 0;
        public HashSet<Colour> Colours => _gbColours;
        public Colour[,] ColourMap => _gbColourMap;
        public HashSet<Colour> OriginalColours => _originalTile.Colours;
        public int OriginalColourCount => _originalColourCount.Value;
        public Colour[,] OriginalTileColourMap => _originalTile.ColourMap;
        public int X => _originalTile.X;
        public int Y => _originalTile.Y;
        public int OriginalTileHash => _originalTileHash.Value;
        public bool IsFullyRecoloured => OriginalColourCount == Colours.Count;
        public Colour ColourDictionary(Colour colour) => _colourDictionary[colour];
        public Dictionary<Colour, Colour> ColourDictionaryCopy => _colourDictionary;
        public IEnumerable<((int x, int y) coordinates, Colour item)> ToIEnumerable() => ColourMap.ToIEnumerableWithCoords();

        public RecolouredTile(ITile originalTile)
        {
            _originalTile = (Tile)originalTile;
            _gbColourMap = new Colour[8, 8];

            _originalColourCount = new Lazy<int>(() => OriginalColours.Count);
            _colourKeyString = new Lazy<string>(() => GenerateColourKeyString());
            _originalTileHash = new Lazy<int>(() => _originalTile.ColourHash);
        }

        public Colour this[int x, int y]
        {
            get => _gbColourMap[x, y];
            set
            {
                _gbColourMap[x, y] = value;
                if (!_colourDictionary.ContainsKey(_originalTile[x, y]))
                {
                    _colourDictionary.Add(_originalTile[x, y], value);
                }
                _gbColours.Add(value);
                if (_gbColours.Count > OriginalColourCount)
                {
                    throw new Exception();
                }
            }
        }

        public string ColourKeyString
        {
            get
            {
                if (_colourKeyString.IsValueCreated || IsFullyRecoloured)
                {
                    return _colourKeyString.Value;
                }
                else
                {
                    return "";
                }
            }
        }

        public string GenerateColourKeyString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var item in OriginalColours.OrderBy(x => x.GetBrightness()))
            {
                stringBuilder.Append(item);
            }

            return stringBuilder.ToString();
        }
    }
}
