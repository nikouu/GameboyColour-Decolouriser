

namespace GameboyColourDecolouriser.ImageConverters
{
    /*
    public static class ImageConverterWindows
    {
        public static GbcImage ToGbcImage(string imagePath)
        {
            var image = new Bitmap(imagePath);
            ValidateImage(image);
            var tiles = new Tile[image.Width / 8, image.Height / 8];

            // https://stackoverflow.com/a/9691388
            var rawImage = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            var imageByteCount = rawImage.Stride * rawImage.Height;
            var imageBytes = new byte[imageByteCount];

            var croppedBitmap = new Bitmap(8, 8);

            Marshal.Copy(rawImage.Scan0, imageBytes, 0, imageByteCount);

            for (int i = 0; i < image.Width; i += 8)
            {
                for (int j = 0; j < image.Height; j += 8)
                {
                    var croppedBytes = GetCroppedImage(imageBytes, rawImage.Stride, i, j, 8, 8);
                    var croppedData = croppedBitmap.LockBits(new Rectangle(0, 0, 8, 8), ImageLockMode.WriteOnly, image.PixelFormat);

                    Marshal.Copy(croppedBytes, 0, croppedData.Scan0, croppedBytes.Length);

                    croppedBitmap.UnlockBits(croppedData);

                    var colourMap = GetColourMap(croppedBitmap);

                    tiles[i / 8, j / 8] = new Tile(colourMap, i / 8, j / 8);
                }

                //progressTask?.Increment(((double)8 / image.Width) * 100);
            }

            image.UnlockBits(rawImage);

            return new GbcImage(image.Width, image.Height, tiles);
        }

        public static byte[] ToImageBytes(DmgImage dmgImage)
        {
            var recolouredImage = new Bitmap(dmgImage.Width, dmgImage.Height);

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

                    recolouredImage.SetPixel(i, j, Color.FromArgb(colour.A, colour.R, colour.G, colour.B));
                }

                //_spectreTasks?.generatingFinalImage.Increment(((double)1 / dmgImage.Width) * 100);
            }

            using var memoryStream = new MemoryStream();
            recolouredImage.Save(memoryStream, ImageFormat.Png);
            return memoryStream.ToArray();
        }

        private static byte[] GetCroppedImage(byte[] imageBytes, int stride, int x, int y, int width, int height)
        {
            var bpp = 4;
            byte[] croppedBytes = new byte[width * height * bpp];


            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width * bpp; j += bpp)
                {
                    int origIndex = (y * stride) + (i * stride) + (x * bpp) + (j);
                    int croppedIndex = (i * width * bpp) + (j);

                    //copy data: once for each channel
                    for (int k = 0; k < bpp; k++)
                    {
                        croppedBytes[croppedIndex + k] = imageBytes[origIndex + k];
                    }
                }
            }

            return croppedBytes;
        }

        private static void ValidateImage(Bitmap image)
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

        private static Colour[,] GetColourMap(Bitmap tile)
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
    }
    */
}
