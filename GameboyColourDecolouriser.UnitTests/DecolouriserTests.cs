using GameboyColourDecolouriser.ImageConverters;
using GameboyColourDecolouriser.Models;

namespace GameboyColourDecolouriser.UnitTests
{
    public class DecolouriserTests
    {
        [Fact]
        public void Decolourise_OneColourTile_CorrectNumberOfTiles()
        {
            // Arrange
            var gbcImage = ImageConverterImageSharp.ToGbcImage(@"./Images/Input/1-colour-1-tile.png");
            var decolouriser = new Decolouriser();

            // Act
            var gbImage = decolouriser.Decolourise(gbcImage);

            // Assert
            Assert.Equal(1, gbImage.Tiles.Length);
        }

        [Fact]
        public void Decolourise_OneColourTile_CorrectNumberOfColours()
        {
            // Arrange
            var gbcImage = ImageConverterImageSharp.ToGbcImage(@"./Images/Input/1-colour-1-tile.png");
            var decolouriser = new Decolouriser();

            // Act
            var gbImage = decolouriser.Decolourise(gbcImage);

            // Assert
            Assert.Single(gbImage.Tiles[0, 0].TranslatedColours);
        }

        [Fact]

        public void Decolourise_OneColourTile_CorrectColourPositions()
        {
            // Arrange
            var gbcImage = ImageConverterImageSharp.ToGbcImage(@"./Images/Input/1-colour-1-tile.png");
            var decolouriser = new Decolouriser();

            // Act
            var gbImage = decolouriser.Decolourise(gbcImage);
            var colourMap = gbImage.Tiles[0, 0].ColourMap;

            // Assert
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 7]);
        }

        [Fact]
        public void Decolourise_TwoColourTile_CorrectNumberOfColours()
        {
            // Arrange
            var gbcImage = ImageConverterImageSharp.ToGbcImage(@"./Images/Input/2-colour-1-tile.png");
            var decolouriser = new Decolouriser();

            // Act
            var gbImage = decolouriser.Decolourise(gbcImage);

            // Assert
            Assert.Equal(2, gbImage.Tiles[0, 0].Colours.Count);
        }

        [Fact]
        public void Decolourise_TwoColourTile_CorrectColourPositions()
        {
            // Arrange
            var gbcImage = ImageConverterImageSharp.ToGbcImage(@"./Images/Input/2-colour-1-tile.png");
            var decolouriser = new Decolouriser();

            // Act
            var gbImage = decolouriser.Decolourise(gbcImage);
            var colourMap = gbImage.Tiles[0, 0].ColourMap;

            // Assert
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[0, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 1]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[0, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 3]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[0, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 5]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[0, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 0]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[1, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 2]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[1, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 4]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[1, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 6]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[1, 7]);

            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[2, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 1]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[2, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 3]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[2, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 5]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[2, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 0]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[3, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 2]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[3, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 4]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[3, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 6]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[3, 7]);

            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[4, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 1]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[4, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 3]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[4, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 5]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[4, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 0]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[5, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 2]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[5, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 4]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[5, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 6]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[5, 7]);

            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[6, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 1]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[6, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 3]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[6, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 5]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[6, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 0]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[7, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 2]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[7, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 4]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[7, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 6]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[7, 7]);
        }

        [Fact]
        public void Decolourise_ThreeColourTile_CorrectNumberOfColours()
        {
            // Arrange
            var gbcImage = ImageConverterImageSharp.ToGbcImage(@"./Images/Input/3-colour-1-tile.png");
            var decolouriser = new Decolouriser();

            // Act
            var gbImage = decolouriser.Decolourise(gbcImage);

            // Assert
            Assert.Equal(3, gbImage.Tiles[0, 0].Colours.Count);
        }

        [Fact]
        public void Decolourise_ThreeColourTile_CorrectColourPositions()
        {
            // Arrange
            var gbcImage = ImageConverterImageSharp.ToGbcImage(@"./Images/Input/3-colour-1-tile.png");
            var decolouriser = new Decolouriser();

            // Act
            var gbImage = decolouriser.Decolourise(gbcImage);
            var colourMap = gbImage.Tiles[0, 0].ColourMap;

            // Assert
            // note that this test the "black" true colour is 1 brightness value closer to GBDark instead of GBBlack
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[0, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 1]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[0, 2]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[0, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 4]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[0, 5]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[0, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 7]);
           
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 0]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[1, 1]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[1, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 3]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[1, 4]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[1, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 6]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[1, 7]);
            
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[2, 0]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[2, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 2]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[2, 3]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[2, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 5]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[2, 6]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[2, 7]);

            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[3, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 1]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[3, 2]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[3, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 4]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[3, 5]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[3, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 0]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[4, 1]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[4, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 3]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[4, 4]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[4, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 6]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[4, 7]);

            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[5, 0]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[5, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 2]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[5, 3]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[5, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 5]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[5, 6]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[5, 7]);

            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[6, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 1]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[6, 2]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[6, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 4]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[6, 5]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[6, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 0]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[7, 1]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[7, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 3]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[7, 4]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[7, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 6]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[7, 7]);
        }

        [Fact]
        public void Decolourise_FourColourTile_CorrectNumberOfColours()
        {
            // Arrange
            var gbcImage = ImageConverterImageSharp.ToGbcImage(@"./Images/Input/4-colour-1-tile.png");
            var decolouriser = new Decolouriser();

            // Act
            var gbImage = decolouriser.Decolourise(gbcImage);

            // Assert
            Assert.Equal(4, gbImage.Tiles[0, 0].Colours.Count);
        }

        [Fact]
        public void Decolourise_FourColourTile_CorrectColourPositions()
        {
            // Arrange
            var gbcImage = ImageConverterImageSharp.ToGbcImage(@"./Images/Input/4-colour-1-tile.png");
            var decolouriser = new Decolouriser();

            // Act
            var gbImage = decolouriser.Decolourise(gbcImage);
            var colourMap = gbImage.Tiles[0, 0].ColourMap;

            // Assert
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[0, 0]);
            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[0, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 2]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[0, 3]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[0, 4]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[0, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[0, 6]);
            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[0, 7]);

            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[1, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 1]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[1, 2]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[1, 3]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[1, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[1, 5]);
            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[1, 6]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[1, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 0]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[2, 1]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[2, 2]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[2, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[2, 4]);
            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[2, 5]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[2, 6]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[2, 7]);

            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[3, 0]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[3, 1]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[3, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 3]);
            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[3, 4]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[3, 5]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[3, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[3, 7]);

            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[4, 0]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[4, 1]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 2]);
            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[4, 3]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[4, 4]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[4, 5]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[4, 6]);
            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[4, 7]);

            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[5, 0]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 1]);
            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[5, 2]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[5, 3]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[5, 4]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[5, 5]);
            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[5, 6]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[5, 7]);

            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 0]);
            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[6, 1]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[6, 2]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[6, 3]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[6, 4]);
            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[6, 5]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[6, 6]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[6, 7]);

            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[7, 0]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[7, 1]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[7, 2]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 3]);
            Assert.Equal(Colour.FromRgb(7, 24, 33), colourMap[7, 4]);
            Assert.Equal(Colour.FromRgb(224, 248, 207), colourMap[7, 5]);
            Assert.Equal(Colour.FromRgb(48, 104, 80), colourMap[7, 6]);
            Assert.Equal(Colour.FromRgb(134, 192, 108), colourMap[7, 7]);
        }
    }
}
