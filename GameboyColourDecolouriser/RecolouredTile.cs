using System.Drawing;
using System.Text;

namespace GameboyColourDecolouriser
{
    public class RecolouredTile : ITile
    {
        private readonly Tile _originalTile;
        private readonly Color[,] _gbColourMap;
        private readonly HashSet<Color> _gbColours;
        // key is original colour
        private readonly Dictionary<Color, Color> _colourDictionary;
        private Lazy<int> _originalColourCount;
        private readonly Lazy<int> _originalTileHash;
        private Lazy<string> _colourKeyString;

        public int ColourHash => 0;
        public HashSet<Color> Colours => _gbColours;
        public Color[,] ColourMap => _gbColourMap;
        public HashSet<Color> OriginalColours => _originalTile.Colours;
        public int OriginalColourCount => _originalColourCount.Value;
        public Color[,] OriginalTileColourMap => _originalTile.ColourMap;
        public Point Coordinate => _originalTile.Coordinate;
        public int OriginalTileHash => _originalTileHash.Value;
        public bool IsFullyRecoloured => OriginalColourCount == Colours.Count;
        public Color ColourDictionary(Color colour) => _colourDictionary[colour];
        public Dictionary<Color, Color> ColourDictionaryCopy => _colourDictionary;
        public IEnumerable<((int x, int y) coordinates, Color item)> ToIEnumerable() => ColourMap.ToIEnumerableWithCoords();

        public RecolouredTile(Tile originalTile)
        {
            _originalTile = originalTile;
            _gbColours = new HashSet<Color>();
            _gbColourMap = new Color[8, 8];
            _colourDictionary = new Dictionary<Color, Color>();

            _originalColourCount = new Lazy<int>(() => OriginalColours.Count);

            _colourKeyString = new Lazy<string>(() => GenerateColourKeyString());

            _originalTileHash = new Lazy<int>(() => _originalTile.ColourHash);
        }

        public Color this[int x, int y]
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

            foreach (var item in OriginalColours.OrderBy(x => x.GetPerceivedBrightness()))
            {
                stringBuilder.Append(item);
            }

            return stringBuilder.ToString();
        }
    }
}
