using GameboyColourDecolouriser.ImageConverters;
using GameboyColourDecolouriser.Models;
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
        public void ToGbcImage_LoadOneColourTile_TileIsLoaded()
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
        public void ToGbcImage_LoadOneColourTile_CorrectColour()
        {
            // Arrange
            var gbcImage = ImageConverterImageSharp.ToGbcImage(@"./Images/Input/1-colour-1-tile.png");

            // Act
            var colours = gbcImage.Tiles[0, 0].Colours;
            var expectedColour = Colour.FromRgb(200, 200, 0);

            // Assert
            Assert.Contains(expectedColour, colours);
            Assert.Equal(1, colours.Count);
        }

        [Fact]
        public void ToGbcImage_LoadTwoColourTile_CorrectColours()
        {
            
        }

        [Fact]
        public void ToGbcImage_LoadThreeColourTile_CorrectColours()
        {

        }

        [Fact]
        public void ToGbcImage_LoadFourColourTile_CorrectColours()
        {

        }

        [Fact]
        public void ToGbcImage_LoadFourColourTile_CorrectColourPositions()
        {
            // Arrange
            var gbcImage = ImageConverterImageSharp.ToGbcImage(@"./Images/Input/4-colour-1-tile.png");

            // Act
            var colourMap = gbcImage.Tiles[0, 0].ColourMap;

            // Assert
            /*
             * The alignment is 
             *  Down then right. So goes all the way down, then one to the right, then all the way down 
             *  
             *  White: Colour.FromRgb(222, 255, 222)
             *  Yellow: Colour.FromRgb(200, 200, 0)
             *  Brown: Colour.FromRgb(160, 136, 64)
             *  Black: Colour.FromRgb(57, 57, 57)
             */
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[0, 0]);
            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[0, 1]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[0, 2]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[0, 3]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[0, 4]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[0, 5]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[0, 6]);
            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[0, 7]);

            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[1, 0]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[1, 1]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[1, 2]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[1, 3]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[1, 4]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[1, 5]);
            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[1, 6]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[1, 7]);

            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[2, 0]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[2, 1]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[2, 2]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[2, 3]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[2, 4]);
            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[2, 5]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[2, 6]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[2, 7]);

            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[3, 0]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[3, 1]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[3, 2]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[3, 3]);
            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[3, 4]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[3, 5]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[3, 6]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[3, 7]);

            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[4, 0]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[4, 1]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[4, 2]);
            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[4, 3]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[4, 4]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[4, 5]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[4, 6]);
            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[4, 7]);

            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[5, 0]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[5, 1]);
            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[5, 2]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[5, 3]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[5, 4]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[5, 5]);
            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[5, 6]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[5, 7]);

            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[6, 0]);
            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[6, 1]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[6, 2]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[6, 3]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[6, 4]);
            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[6, 5]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[6, 6]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[6, 7]);

            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[7, 0]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[7, 1]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[7, 2]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[7, 3]);
            Assert.Equal(Colour.FromRgb(57, 57, 57), colourMap[7, 4]);
            Assert.Equal(Colour.FromRgb(222, 255, 222), colourMap[7, 5]);
            Assert.Equal(Colour.FromRgb(160, 136, 64), colourMap[7, 6]);
            Assert.Equal(Colour.FromRgb(200, 200, 0), colourMap[7, 7]);
        }


        [Fact]
        public void ToGbcImage_LoadFiveColourTile_ThrowException()
        {
            // Arrange
            var act = () => ImageConverterImageSharp.ToGbcImage(@"./Images/Input/5-colour-1-tile.png");

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void ToGbcImage_LoadBadWidthTile_ThrowException()
        {
            // Arrange
            var act = () => ImageConverterImageSharp.ToGbcImage(@"./Images/Input/bad-width.png");

            // Act

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ToGbcImage_LoadBadHeightTile_ThrowException()
        {
            // Arrange
            var act = () => ImageConverterImageSharp.ToGbcImage(@"./Images/Input/bad-height.png");

            // Act

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ToGbcImage_LoadBadWidthAndHeightTile_ThrowException()
        {

        }

        [Fact]
        public void ToGbcImage_LoadTwoTiles_TilesAreLoaded()
        {

        }
    }
}
