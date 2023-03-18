using GameboyColourDecolouriser.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyColourDecolouriser
{
    public class ColourTranslator : IEquatable<ColourTranslator>
    {
        private Colour _GBCWhite;
        private Colour _GBCLight;
        private Colour _GBCDark;
        private Colour _GBCBlack;

        public Colour GBCWhite => _GBCWhite;
        public Colour GBCLight => _GBCLight;
        public Colour GBCDark => _GBCDark;
        public Colour GBCBlack => _GBCBlack;

        public ColourTranslator()
        {

        }

        public void UpdateTranslation(Colour gbcColour, Colour gbColour)
        {
            if (gbColour == Colour.GBWhite && _GBCWhite.IsDefault)
            {
                _GBCWhite = gbcColour;
            }
            else if (gbColour == Colour.GBLight && _GBCLight.IsDefault)
            {
                _GBCLight = gbcColour;
            }
            else if (gbColour == Colour.GBDark && _GBCDark.IsDefault)
            {
                _GBCDark = gbcColour;
            }
            else if (gbColour == Colour.GBBlack && _GBCBlack.IsDefault)
            {
                _GBCBlack = gbcColour;
            }
            else
            {
                throw new Exception("Unable to update translations. Either colour has already been written, or gbColour not found.");
            }
        }

        public bool IsTranslated(Colour gbcColour)
        {
            // remove duplication from GetGBColour
            if (gbcColour == _GBCWhite)
            {
                return true;
            }
            else if (gbcColour == _GBCLight)
            {
                return true;
            }
            else if (gbcColour == _GBCDark)
            {
                return true;
            }
            else if (gbcColour == _GBCBlack)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Colour GetGBColour(Colour gbcColour)
        {
            if (gbcColour == _GBCWhite)
            {
                return Colour.GBWhite;
            }
            else if (gbcColour == _GBCLight)
            {
                return Colour.GBLight;
            }
            else if (gbcColour == _GBCDark)
            {
                return Colour.GBDark;
            }
            else if (gbcColour == _GBCBlack)
            {
                return Colour.GBBlack;
            }
            else
            {
                throw new Exception("GBC colour not mapped to GB colours.");
            }
        }

        public Colour GetGBCColour(Colour gbColour)
        {
            if (gbColour == Colour.GBWhite)
            {
                return _GBCWhite;
            }
            else if (gbColour == Colour.GBLight)
            {
                return _GBCLight;
            }
            else if (gbColour == Colour.GBDark)
            {
                return _GBCDark;
            }
            else if (gbColour == Colour.GBBlack)
            {
                return _GBCBlack;
            }
            else
            {
                throw new Exception("GB colour not mapped to GBC colours.");
            }
        }

        public Dictionary<Colour, Colour> ToDictionary()
        {
            var colourDictionary = new Dictionary<Colour, Colour>();
            if (!_GBCWhite.IsDefault)
            {
                colourDictionary.Add(_GBCWhite, Colour.GBWhite);
            }

            if (!_GBCLight.IsDefault)
            {
                colourDictionary.Add(_GBCLight, Colour.GBLight);
            }

            if (!_GBCDark.IsDefault)
            {
                colourDictionary.Add(_GBCDark, Colour.GBDark);
            }

            if (!_GBCBlack.IsDefault)
            {
                colourDictionary.Add(_GBCBlack, Colour.GBBlack);
            }

            return colourDictionary;
        }

        public bool Equals(ColourTranslator? other)
        {
            if (GBCWhite != other.GBCWhite)
            {
                return false;
            }

            if (GBCLight != other.GBCLight)
            {
                return false;
            }

            if (GBCDark != other.GBCDark)
            {
                return false;
            }

            if (GBCBlack != other.GBCBlack)
            {
                return false;
            }

            return true;
        }

        public HashSet<Colour> ToGBCHashSet()
        {
            return new HashSet<Colour>(ToDictionary().Keys);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ColourTranslator);
        }

        public override int GetHashCode()
        {
            return GBCWhite.GetHashCode() + GBCLight.GetHashCode() + GBCDark.GetHashCode() + GBCBlack.GetHashCode();
        }
    }
}
