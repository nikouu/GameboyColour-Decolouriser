namespace GameboyColourDecolouriser.Models
{
    // A trimmed version of the System.Drawing Color object
    public readonly struct Colour : IEquatable<Colour>
    {
        private readonly bool? _isDefault;
        private readonly long value;

        private const int ARGBAlphaShift = 24;
        private const int ARGBRedShift = 16;
        private const int ARGBGreenShift = 8;
        private const int ARGBBlueShift = 0;

        public byte R => unchecked((byte)(value >> ARGBRedShift));
        public byte G => unchecked((byte)(value >> ARGBGreenShift));
        public byte B => unchecked((byte)(value >> ARGBBlueShift));
        public byte A => unchecked((byte)(value >> ARGBAlphaShift));

        internal Colour(long value, bool isDefault)
        {
            _isDefault = isDefault;
            this.value = value;
        }

        public static Colour FromArgb(int red, int green, int blue)
        {
            return FromArgb(255, red, green, blue);
        }

        public static Colour FromArgb(int alpha, int red, int green, int blue)
        {
            CheckByte(alpha, nameof(alpha));
            CheckByte(red, nameof(red));
            CheckByte(green, nameof(green));
            CheckByte(blue, nameof(blue));

            return new Colour(
                (uint)alpha << ARGBAlphaShift |
                (uint)red << ARGBRedShift |
                (uint)green << ARGBGreenShift |
                (uint)blue << ARGBBlueShift,
                false
            );
        }

        // https://www.nbdtech.com/Blog/archive/2008/04/27/calculating-the-perceived-brightness-of-a-color.aspx
        public int GetPerceivedBrightness()
        {
            return (int)Math.Sqrt(
               R * R * .241 +
               G * G * .691 +
               B * B * .068);
        }

        public bool IsBlank
            => A == 0 && R == 0 && G == 0 && B == 0;

        public bool IsDefault => _isDefault ?? true;

        // https://stackoverflow.com/a/37821008
        public string ToHexString => $"#{R:X2}{G:X2}{B:X2}";

        // RGB(R, G, B)
        public string ToRgbString => $"RGB({R}, {G}, {B})";

        // #RRGGBBAA
        public string ToHexaString => $"#{R:X2}{G:X2}{B:X2}{A:X2}";

        public double ToProportion(byte b) => b / (double)Byte.MaxValue;

        // RGBA(R, G, B, A)
        public string ToRgbaString => $"RGBA({ToProportion(A):N2}, {R}, {G}, {B})";

        private static void CheckByte(int value, string name)
        {
            if (unchecked((uint)value) > byte.MaxValue)
            {
                throw new ArgumentException();
            }
        }

        public static bool operator ==(Colour left, Colour right) =>
            left.value == right.value;

        public static bool operator !=(Colour left, Colour right) => !(left == right);

        public bool Equals(Colour other) => this == other;

        public override string ToString() => ToRgbaString;


    }
}
