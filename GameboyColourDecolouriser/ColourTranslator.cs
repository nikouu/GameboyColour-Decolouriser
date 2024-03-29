﻿using GameboyColourDecolouriser.Models;

namespace GameboyColourDecolouriser
{
    public class ColourTranslator : IEquatable<ColourTranslator>
    {
        private Colour _GBCWhite;
        private Colour _GBCLight;
        private Colour _GBCDark;
        private Colour _GBCBlack;
        private readonly Dictionary<Colour, Colour> _dictionary = new();

        public Colour GBCWhite => _GBCWhite;
        public Colour GBCLight => _GBCLight;
        public Colour GBCDark => _GBCDark;
        public Colour GBCBlack => _GBCBlack;

        public ColourTranslator() { }

        public ColourTranslator(Dictionary<Colour, Colour> dictionary)
        {
            foreach (var (gbcColour, gbColour) in dictionary)
            {
                UpdateTranslation(gbcColour, gbColour);
            }
        }

        public void UpdateTranslation(Colour gbcColour, Colour gbColour)
        {
            if (gbColour == Colour.GBWhite)
            {
                _GBCWhite = gbcColour;
                _dictionary.Add(_GBCWhite, Colour.GBWhite);
            }
            else if (gbColour == Colour.GBLight)
            {
                _GBCLight = gbcColour;
                _dictionary.Add(_GBCLight, Colour.GBLight);
            }
            else if (gbColour == Colour.GBDark)
            {
                _GBCDark = gbcColour;
                _dictionary.Add(_GBCDark, Colour.GBDark);
            }
            else if (gbColour == Colour.GBBlack)
            {
                _GBCBlack = gbcColour;
                _dictionary.Add(_GBCBlack, Colour.GBBlack);
            }
            else
            {
                throw new Exception("Unable to update translations. Either colour has already been written, or gbColour not found.");
            }
        }

        public bool IsTranslated(Colour gbcColour)
        {
            // remove duplication from GetGBColour
            if (gbcColour == _GBCWhite && !_GBCWhite.IsDefault)
            {
                return true;
            }
            else if (gbcColour == _GBCLight && !_GBCLight.IsDefault)
            {
                return true;
            }
            else if (gbcColour == _GBCDark && !_GBCDark.IsDefault)
            {
                return true;
            }
            else if (gbcColour == _GBCBlack && !_GBCBlack.IsDefault)
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

        public Dictionary<Colour, Colour> ToDictionary() => new(_dictionary);

        public bool Equals(ColourTranslator? other)
        {
            if (other == null)
            {
                return false;
            }

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
            return new HashSet<Colour>(_dictionary.Keys);
        }

        public HashSet<Colour> ToGBHashSet()
        {
            return new HashSet<Colour>(_dictionary.Values);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Equals(obj as ColourTranslator);
        }

        public override int GetHashCode()
        {
            return GBCWhite.GetHashCode() + GBCLight.GetHashCode() + GBCDark.GetHashCode() + GBCBlack.GetHashCode();
        }
    }
}
