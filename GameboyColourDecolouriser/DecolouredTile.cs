using GameboyColourDecolouriser.Models;
using System.Text;

namespace GameboyColourDecolouriser
{
    public class DecolouredTile : ITile
    {
        private readonly Tile _gbcTile;
        private readonly Colour[,] _gbColourMap;
        private readonly ColourTranslator _colourTranslator = new();
        private Lazy<int> _gbcColourCount;
        private readonly Lazy<int> _originalTileHash;
        private Lazy<string> _colourKeyString;
        private bool _isFullyDecoloured = false;

        public int ColourHash => 0;

        // these are the gbc colours we have processed so far. so should this be under "colours" because that doesnt represent the colours of the decoloured tile...
        //public HashSet<Colour> Colours => _colourTranslator.ToGBCHashSet();

        //public HashSet<Colour> TranslatedColours => _colourTranslator.ToGBCHashSet();

        public Colour[,] ColourMap => _gbColourMap;
        public HashSet<Colour> GBCColours => _gbcTile.Colours;
        public int GBCColourCount => _gbcColourCount.Value;
        public Colour[,] OriginalTileColourMap => _gbcTile.ColourMap;
        public int X => _gbcTile.X;
        public int Y => _gbcTile.Y;
        public int OriginalTileHash => _originalTileHash.Value;
        public string ColourKeyString => _colourKeyString.Value;
        public bool IsFullyDecoloured
        {
            get
            {
                if (_isFullyDecoloured)
                {
                    return _isFullyDecoloured;
                }
                else if (GBCColourCount == Colours.Count)
                {
                    _isFullyDecoloured = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Colour GetGBColour(Colour gbColour) => _colourTranslator.GetGBColour(gbColour);
        public bool IsColourTranslated(Colour gbcColour) => _colourTranslator.IsTranslated(gbcColour);
        public Dictionary<Colour, Colour> GetTranslatedDictionary => _colourTranslator.ToDictionary();
        public IEnumerable<((int x, int y) coordinates, Colour item)> ToIEnumerable() => ColourMap.ToIEnumerableWithCoords();

        public DecolouredTile(ITile originalTile)
        {
            _gbcTile = (Tile)originalTile;
            _gbColourMap = new Colour[8, 8];

            // these are the mapped colours
            _gbcColourCount = new Lazy<int>(() => _gbcTile.Colours.Count);
            _colourKeyString = new Lazy<string>(() => GenerateColourKeyString());
            _originalTileHash = new Lazy<int>(() => _gbcTile.ColourHash);
        }

        public Colour this[int x, int y]
        {
            get => _gbColourMap[x, y];
            set
            {
                _gbColourMap[x, y] = value;
                if (!_colourTranslator.IsTranslated(_gbcTile[x, y]))
                {
                    _colourTranslator.UpdateTranslation(_gbcTile[x, y], value);
                }
            }
        }

        private int _alreadyTranslatedColourCount = -1;
        private HashSet<Colour> _gbHashSet;
        public HashSet<Colour> Colours
        {
            get
            {
                if (_alreadyTranslatedColourCount == GBCColourCount)
                {
                    return _gbHashSet;
                }
                else
                {
                    var gbHashSet = _colourTranslator.ToGBHashSet();
                    _alreadyTranslatedColourCount = gbHashSet.Count;
                    _gbHashSet = gbHashSet;
                    return gbHashSet;
                }
            }
        }

        public string GenerateColourKeyString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var item in GBCColours.OrderBy(x => x.GetBrightness()))
            {
                stringBuilder.Append(item);
            }

            return stringBuilder.ToString();
        }
    }
}
