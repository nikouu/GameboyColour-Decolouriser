using Spectre.Console;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Color = System.Drawing.Color;

namespace GameboyColourDecolouriser
{
    public class GbcImage
    {
        private int _width;
        private int _height;

        private Tile[,] _tiles;

        public Tile[,] Tiles => _tiles;
        public int Width => _width;
        public int Height => _height;

        public void LoadImage(string filePath, ProgressTask? progressTask = null)
        {
            var image = new Bitmap(filePath);
            LoadImage(image, progressTask);
        }

        public void LoadImage(Bitmap image, ProgressTask? progressTask = null)
        {
            Validate(image);

            _width = image.Width;
            _height = image.Height;
            _tiles = new Tile[_width / 8, _height / 8];

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

                    _tiles[i / 8, j / 8] = new Tile(croppedBitmap, i / 8, j / 8);
                }

                progressTask?.Increment(((double)8 / image.Width) * 100);
            }

            image.UnlockBits(rawImage);
        }

        public Color GetPixel(int x, int y)
        {
            var tileArrayX = x / 8;
            var tileArrayY = y / 8;

            var tileX = x % 8;
            var tileY = y % 8;

            var tile = _tiles[tileArrayX, tileArrayY];

            var colour = tile[tileX, tileY];

            return colour;
        }

        private byte[] GetCroppedImage(byte[] imageBytes, int stride, int x, int y, int width, int height)
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

        private void Validate(Bitmap image)
        {
            if (image.Height % 8 != 0)
            {
                throw new ArgumentException($"Image height of {image.Height} is not divisible by 8.");
            }

            if (image.Width % 8 != 0)
            {
                throw new ArgumentException($"Image width of {image.Width} is not divisible by 8.");
            }
        }
    }
}
