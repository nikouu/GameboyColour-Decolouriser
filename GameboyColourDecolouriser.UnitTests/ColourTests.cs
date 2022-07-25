using GameboyColourDecolouriser.Models;

namespace GameboyColourDecolouriser.UnitTests;

// Inspired by https://github.com/SixLabors/ImageSharp/tree/main/tests/ImageSharp.Tests/Color
// Simple tests for post-refactoring checks
public class ColourTests
{
    [Fact]
    public void FromRgb()
    {
        // Arrange
        var red = 100;
        var green = 100;
        var blue = 100;

        // Act
        var colour = Colour.FromRgb(red, green, blue);

        // Assert
        Assert.Equal(255, colour.A);
        Assert.Equal(red, colour.R);
        Assert.Equal(green, colour.G);
        Assert.Equal(blue, colour.B);
    }

    [Fact]
    public void FromArgb()
    {
        // Arrange
        var alpha = 255;
        var red = 100;
        var green = 100;
        var blue = 100;

        // Act
        var colour = Colour.FromArgb(alpha, red, green, blue);

        // Assert
        Assert.Equal(alpha, colour.A);
        Assert.Equal(red, colour.R);
        Assert.Equal(green, colour.G);
        Assert.Equal(blue, colour.B);
    }

    [Fact]
    public void GetBrightness()
    {
        // Arrange
        var alpha = 255;
        var red = 160;
        var green = 136;
        var blue = 64;

        // Act
        var colour = Colour.FromArgb(alpha, red, green, blue);
        var brightness = colour.GetBrightness();

        // Assert
        Assert.Equal(138, brightness);
    }

    [Fact]
    public void IsBlank()
    {
        // Arrange
        var alpha = 0;
        var red = 0;
        var green = 0;
        var blue = 0;

        // Act
        var colour = Colour.FromArgb(alpha, red, green, blue);

        // Assert
        Assert.True(colour.IsBlank);
    }

    [Fact]
    public void IsDefault()
    {
        // Arrange           
        var colour = new Colour();

        // Act

        // Assert
        Assert.True(colour.IsDefault);
    }

    [Fact]
    public void GBWhite()
    {
        // Arrange
        var alpha = 255;
        var red = 224;
        var green = 248;
        var blue = 207;

        // Act
        var gbWhite = Colour.GBWhite;

        // Assert
        Assert.Equal(alpha, gbWhite.A);
        Assert.Equal(red, gbWhite.R);
        Assert.Equal(green, gbWhite.G);
        Assert.Equal(blue, gbWhite.B);
    }

    [Fact]
    public void GBLight()
    {
        // Arrange
        var alpha = 255;
        var red = 134;
        var green = 192;
        var blue = 108;

        // Act
        var gbWhite = Colour.GBLight;

        // Assert
        Assert.Equal(alpha, gbWhite.A);
        Assert.Equal(red, gbWhite.R);
        Assert.Equal(green, gbWhite.G);
        Assert.Equal(blue, gbWhite.B);
    }

    [Fact]
    public void GBDark()
    {
        // Arrange
        var alpha = 255;
        var red = 48;
        var green = 104;
        var blue = 80;

        // Act
        var gbWhite = Colour.GBDark;

        // Assert
        Assert.Equal(alpha, gbWhite.A);
        Assert.Equal(red, gbWhite.R);
        Assert.Equal(green, gbWhite.G);
        Assert.Equal(blue, gbWhite.B);
    }

    [Fact]
    public void GBBlack()
    {
        // Arrange
        var alpha = 255;
        var red = 7;
        var green = 24;
        var blue = 33;

        // Act
        var gbWhite = Colour.GBBlack;

        // Assert
        Assert.Equal(alpha, gbWhite.A);
        Assert.Equal(red, gbWhite.R);
        Assert.Equal(green, gbWhite.G);
        Assert.Equal(blue, gbWhite.B);
    }
}
