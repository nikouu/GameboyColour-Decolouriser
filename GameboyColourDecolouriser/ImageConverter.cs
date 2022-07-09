using GameboyColourDecolouriser.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyColourDecolouriser
{
    public static class ImageConverter
    {
        public static GbcImage ToGbcImage(string imagePath)
        {
            var bytes = File.ReadAllBytes(imagePath);
            using var data = SKData.CreateCopy(bytes);
            using var image = SKBitmap.Decode(data);

            ValidateImage(image);
            var tiles = new Tile[image.Width / 8, image.Height / 8];
  

            for (int i = 0; i < image.Width; i += 8)
            {
                for (int j = 0; j < image.Height; j += 8)
                {
                    var croppingRect = new SKRectI(i, j, i + 8, j + 8);

                    var bitmapTile = new SKBitmap(8, 8);

                    image.ExtractSubset(bitmapTile, croppingRect);

                    var colourMap = GetColourMap(bitmapTile);

                    tiles[i / 8, j / 8] = new Tile(colourMap, i / 8, j / 8);
                }

                //progressTask?.Increment(((double)8 / image.Width) * 100);
            }


            return new GbcImage(image.Width, image.Height, tiles);
        }

        public static byte[] ToImageBytes(DmgImage dmgImage)
        {
            var recolouredImage = new SKBitmap(dmgImage.Width, dmgImage.Height);

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

                    recolouredImage.SetPixel(i, j, new SKColor(colour.R, colour.G, colour.B, colour.A));
                }

                //_spectreTasks?.generatingFinalImage.Increment(((double)1 / dmgImage.Width) * 100);
            }

            return recolouredImage.Encode(SKEncodedImageFormat.Png, 100).Span.ToArray();
        }

        private static Colour[,] GetColourMap(SKBitmap tile)
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
                    var drawingColour = tile.GetPixel(i, j);
                    var colour = Colour.FromArgb(drawingColour.Alpha, drawingColour.Red, drawingColour.Green, drawingColour.Blue);

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


        private static void ValidateImage(SKBitmap image)
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
