using GameboyColourDecolouriser.Models;
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
