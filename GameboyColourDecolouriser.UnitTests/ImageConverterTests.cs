using GameboyColourDecolouriser.ImageConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyColourDecolouriser.UnitTests
{
    public class ImageConverterTests
    {
        [Fact]
        public void ToGbcImage_LoadSimpleOneColourTile_TileIsLoaded()
        {
            // Arrange
            var gbcImage = ImageConverterImageSharp.ToGbcImage(@"./Images/Input/1-colour-1-tile.png");

            // Act


            // Assert
            Assert.Equal(8, gbcImage.Height);
            Assert.Equal(8, gbcImage.Width);
            Assert.Equal(1, gbcImage.Tiles.Length);
        }

        [Fact]
        public void ToGbcImage_LoadSimpleOneColourTile_CorrectColours()
        {
            // Arrange
            var gbcImage = ImageConverterImageSharp.ToGbcImage(@"./Images/Input/1-colour-1-tile.png");

            // Act


            // Assert
            
        }
    }
}
