using GameboyColourDecolouriser.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Spectre.Console;

namespace GameboyColourDecolouriser.ImageConverters
{
    public static class ImageConverterImageSharp
    {
        public static GbcImage ToGbcImage(string imagePath, ProgressTask? progressTask = null)
        {
            using var image = Image.Load<Rgba32>(imagePath);
            return ToGbcImage(image, progressTask);
        }

        public static GbcImage ToGbcImage(byte[] imageBytes, ProgressTask? progressTask = null)
        {
            using var image = Image.Load<Rgba32>(imageBytes);
            return ToGbcImage(image, progressTask);
        }

        private static GbcImage ToGbcImage(Image<Rgba32> image, ProgressTask? progressTask = null)
        {
            ValidateImage(image);
            var tiles = new Tile[image.Width / 8, image.Height / 8];

            for (int i = 0; i < image.Width; i += 8)
            {
                for (int j = 0; j < image.Height; j += 8)
                {
                    var clone = image.Clone(c => c.Crop(new Rectangle(i, j, 8, 8)));
                    var colourMap = GetColourMap(clone);
                    tiles[i / 8, j / 8] = new Tile(colourMap, i / 8, j / 8);
                }
                progressTask?.Increment((double)8 / image.Width * 100);
            }

            return new GbcImage(image.Width, image.Height, tiles);
        }

        public static byte[] ToImageBytes(DmgImage dmgImage, ProgressTask? progressTask = null)
        {
            using var image = new Image<Rgba32>(dmgImage.Width, dmgImage.Height);

            for (int i = 0; i < dmgImage.Width; i++)
            {
                for (int j = 0; j < dmgImage.Height; j++)
                {
                    var tileArrayX = i / 8;
                    var tileArrayY = j / 8;

                    var tileX = i % 8;
                    var tileY = j % 8;

                    var tile = dmgImage.Tiles[tileArrayX, tileArrayY];
                    var colour = tile[tileX, tileY];

                    image[i, j] = new Rgba32(colour.R, colour.G, colour.B, colour.A);
                }

                progressTask?.Increment((double)1 / dmgImage.Width * 100);
            }

            using var imageStream = new MemoryStream();
            image.SaveAsPng(imageStream);

            return imageStream.ToArray();
        }

        private static Colour[,] GetColourMap(Image<Rgba32> tile)
        {
            if (tile.Width > 8 || tile.Height > 8)
            {
                throw new ArgumentException(nameof(tile));
            }

            var colourMap = new Colour[8, 8];
            var colours = new HashSet<Colour>();

            for (int i = 0; i < tile.Width; i++)
            {
                for (int j = 0; j < tile.Height; j++)
                {
                    var drawingColour = tile[i, j];
                    var colour = Colour.FromArgb(drawingColour.A, drawingColour.R, drawingColour.G, drawingColour.B);

                    colourMap[i, j] = colour;
                    colours.Add(colour);
                }
            }

            if (colours.Count > 4)
            {
                throw new InvalidOperationException($"More than 4 colours found in a single tile.");
            }

            return colourMap;
        }

        private static void ValidateImage(Image image)
        {
            if (image.Height % 8 != 0)
            {
                throw new ArgumentException($"Image height of {image.Height}px is not divisible by 8.");
            }

            if (image.Width % 8 != 0)
            {
                throw new ArgumentException($"Image width of {image.Width}px is not divisible by 8.");
            }
        }
    }
}
