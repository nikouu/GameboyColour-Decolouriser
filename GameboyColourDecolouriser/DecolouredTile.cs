using GameboyColourDecolouriser.Models;
using System.Text;

namespace GameboyColourDecolouriser
{
    /// <summary>
    /// Represents a GBC tile being decoloured to a GB tile with associated data.
    /// </summary>
    public class DecolouredTile : ITile
    {
        private readonly Tile _gbcTile;
        private readonly Colour[,] _gbColourMap;
        private readonly ColourTranslator _colourTranslator = new();
        private Lazy<int> _gbcColourCount;
        private readonly Lazy<int> _originalTileHash;
        private Lazy<string> _colourKeyString;
        private bool _isFullyDecoloured = false;
        private int _alreadyTranslatedColourCount = -1;
        private HashSet<Colour> _gbHashSet;

        /// <summary>
        /// A <seealso cref="T:Colour[,]"/> being each of the 64 pixels in the 8x8 pixel tile from the decoloured image.
        /// </summary>
        public Colour[,] ColourMap => _gbColourMap;

        /// <summary>
        /// Count of all the GBC colours for this <seealso cref="DecolouredTile"/>.
        /// </summary>
        public int GBCColourCount => _gbcColourCount.Value;

        /// <summary>
        /// All the GBC colours for this <seealso cref="DecolouredTile"/>.
        /// </summary>
        public HashSet<Colour> GBCColours => _gbcTile.Colours;
        
        /// <summary>
        /// A <seealso cref="T:Colour[,]"/> being each of the 64 pixels in the 8x8 pixel tile from the original colour image.
        /// </summary>
        public Colour[,] OriginalTileColourMap => _gbcTile.ColourMap;

        /// <inheritdoc/>
        public int X => _gbcTile.X;

        /// <inheritdoc/>
        public int Y => _gbcTile.Y;

        /// <summary>
        /// Hash of the GBC tile. Used as an identifier for the tile.
        /// </summary>
        public int OriginalTileHash => _originalTileHash.Value;

        /// <summary>
        /// A string representation of the GBC colours in the original GBC tile. Used to identify all the GBC colours used in this tile.
        /// </summary>
        public string ColourKeyString => _colourKeyString.Value;

        /// <summary>
        /// Whether all the pixels have been decoloured.
        /// </summary>
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
        public int ColourHash => 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecolouredTile"/> class.
        /// </summary>
        /// <param name="originalTile">The original tile to setup to decolourise.</param>
        public DecolouredTile(ITile originalTile)
        {
            _gbcTile = (Tile)originalTile;
            _gbColourMap = new Colour[8, 8];

            // these are the mapped colours
            _gbcColourCount = new Lazy<int>(() => _gbcTile.Colours.Count);
            _colourKeyString = new Lazy<string>(() => GenerateColourKeyString());
            _originalTileHash = new Lazy<int>(() => _gbcTile.ColourHash);
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// Gets the GB <seealso cref="Colour"/> mapped to the GBC <seealso cref="Colour"/>.
        /// </summary>
        /// <param name="gbcColour"></param>
        /// <returns>The GB <seealso cref="Colour"/>.</returns>
        public Colour GetGBColour(Colour gbcColour) => _colourTranslator.GetGBColour(gbcColour);

        /// <summary>
        /// Gets whether there is a mapping of a GBC <seealso cref="Colour"/> to a GB <seealso cref="Colour"/>.
        /// </summary>
        /// <param name="gbcColour">The GBC <seealso cref="Colour"/> to check.</param>
        /// <returns>True if the <seealso cref="Colour"/> has been mapped, false otherwise.</returns>
        public bool IsColourTranslated(Colour gbcColour) => _colourTranslator.IsTranslated(gbcColour);

        /// <summary>
        /// Gets a <seealso cref="Dictionary{Colour, Colour}"/> of each GBC <seealso cref="Colour"/> mapped to a GB <seealso cref="Colour"/>.
        /// </summary>
        public Dictionary<Colour, Colour> GetTranslatedDictionary => _colourTranslator.ToDictionary();

        /// <summary>
        /// Gets an iterable version of the <see cref="ColourMap"/> with coordinates and <seealso cref="Colour"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<((int x, int y) coordinates, Colour item)> ToIEnumerable() => ColourMap.ToIEnumerableWithCoords();

        /// <summary>
        /// All the currently found GB colours for this <seealso cref="DecolouredTile"/>.
        /// </summary>
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

        /// <summary>
        /// Generates key as a string to represent the GBC colours.
        /// </summary>
        /// <returns>A string representing the GBC colours.</returns>
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
