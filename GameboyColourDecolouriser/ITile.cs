using GameboyColourDecolouriser.Models;

namespace GameboyColourDecolouriser
{
    public interface ITile
    {
        /// <summary>
        /// X coordinate of this tile in the greater image.
        /// </summary>
        int X { get; }

        /// <summary>
        /// X coordinate of this tile in the greater image.
        /// </summary>
        int Y { get; }

        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/indexers/indexers-in-interfaces
        /// <summary>
        /// Gets or sets a <seealso cref="Colour"/> pixel associated with a specific x and y coordinate in the <seealso cref="DecolouredTile"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The <seealso cref="Colour"/> for the pixel.</returns>
        Colour this[int x, int y] { get; }

        /// <summary>
        /// All the colours for the <see cref="ITile"/>.
        /// </summary>
        HashSet<Colour> Colours { get; }

        /// <summary>
        /// A <seealso cref="T:Colour[,]"/> being each of the 64 pixels in the 8x8 pixel tile.
        /// </summary>
        Colour[,] ColourMap { get; }

        int ColourHash { get; }
    }
}
