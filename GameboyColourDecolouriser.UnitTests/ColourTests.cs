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
}
