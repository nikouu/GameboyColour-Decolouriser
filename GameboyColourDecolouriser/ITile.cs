using GameboyColourDecolouriser.Models;

namespace GameboyColourDecolouriser
{
    public interface ITile
    {
        int X { get; }
        int Y { get; }

        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/indexers/indexers-in-interfaces
        Colour this[int x, int y] { get; }

        HashSet<Colour> Colours { get; }
        Colour[,] ColourMap { get; }

        int ColourHash { get; }
    }
}
