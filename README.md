# Gameboy Colouriser
✨Colourises a Gameboy Color image into a GameBoy image✨ via looking at each tile and mapping the colour values to one of the 4 on a Gameboy. You could almost say it "decolourses" images.

![image](etc/mokkiarea-eachpass.gif)

## Why tho?

In short: I wanted a crisp image when importing into [GB Studio](https://www.gbstudio.dev/).

I wanted to have a set of tiles/sprites in the style of Pokémon Generation 2 on the Gameboy Color and ended up drawing a whole bunch of tiles in my repo [Pokemon Gen 2 Style Tilemap](https://github.com/nikouu/Pokemon-gen-2-style-tilemap). These were so I could both draw my own environments and have a play with GB Studio which a few months prior to creating my own tiles came out with palettes for coloured games - rather than just the 4 default Gameboy greens. 

Except... When importing my backgrounds into GB Studio I found they were automatically transformed into 4 colours and I had to "paint" coloured 4 colour palettes onto the tiles myself in the program. Which would be fine except GB Studio does not properly import coloured images. For example:

| Created From       | Resulting Image                          |
| ------------------ | ---------------------------------------- |
| Original Colour    | ![image](etc/mokkiarea-truecolour.png)   |
| GB Studio          | ![image](etc/mokkiarea-gbstudio.png)     |
| Gameboy Colouriser | ![image](etc/mokkiarea-gbcolouriser.png) |

Ignoring that the actual 4 colour palettes are slightly different (i.e. different shades of green) it still stands that the GB Studio version does not keep the original image fidelity. There is some sort of approximation of colour across the *entire* image and not per tile. This is easy to see if we zoom right into some of the detail:

| Detail     | Original                                    | GB Studio                                 | Gameboy Colouriser                            |
| ---------- | ------------------------------------------- | ----------------------------------------- | --------------------------------------------- |
| Building   | ![image](etc/mokkibuilding-truecolour.png)  | ![image](etc/mokkibuilding-gbstudio.png)  | ![image](etc/mokkibuilding-gbcolouriser.png)  |
| Grass      | ![image](etc/mokkigrass-truecolour.png)     | ![image](etc/mokkigrass-gbstudio.png)     | ![image](etc/mokkigrass-gbcolouriser.png)     |
| Tall Grass | ![image](etc/mokkitallgrass-truecolour.png) | ![image](etc/mokkitallgrass-gbstudio.png) | ![image](etc/mokkitallgrass-gbcolouriser.png) |

See how in GB Studio:
- Building roof shading is lost
- Building window colours are gone
- Grass has been reduced to two colours
- Tall grass has been reduced to two colours

To keep fidelity of the shading is the goal of this project. 

## Technical Notes

### Four colours only
The Gameboy and Gameboy Color both use 8x8px squares as tiles or sprites. Due to the limitation of the hardware each 8x8 tile can only contain a maximum of four colours. In the case of the original Gameboy, it was from any of the four greens and in the case of the Gameboy Colour, it could be any of the colours available, but only a selection of 4 per tile too. There are other limitations when we consider the whole screen and sprites, but that's out of scope here. If you're interested further checkout this: [How Graphics worked on the Nintendo Game Boy by MVG](https://www.youtube.com/watch?v=zQE1K074v3s) for a somewhat technical but still easily consumed video.

This encompasses the technical problem: How do you know what four Gameboy greens to map to true colour Gameboy Colour colours? 

It sounds easy at first beacuse you just obviously map the brightest true colour to the lightest Gameboy shade and keep going that way. But if we just think of brightness, we could have problems like:

- There are different ways to measure brightness which can lead to odd result
- A darker blue on a specific tile might be the darkest for that tile, but it could be the second darkest for another tile. What Gameboy shade applies to this dark blue?

Considering each tile individually and using the constraints of having 4 colours per tile is how the Gameboy Colouriser works. It does not take in a whole image to approximate and flatten the colours, like in GB Studio.

### How does it work?
While the program has been through a few iterations, it works by doing several passes over each tile of the image and forming reasoning around what true colour maps to what Gameboy colour (*read: a lot of `Dictionary<K, V>` objects are involved*). These passes are the core workings of the Gameboy Colouriser and they are:

#### 1. Recolour Based on Four Colour Tiles
This is the easiest step. Four colour tiles leave no guesswork on how each true colour maps to a Gameboy colour. Just order both true colours and Gameboy colours by brightness and marry them together. 

We can then begin to reason with three colour tiles that are a subset of the four colour tiles. If the three true colours are all the same as any three inside a four colour tile, we can directly map those true colours in the three tile to the Gameboy shades of the four colour.

Then so for two and one colour tiles.

As long as there is a decent amount of four colour tiles, a good chunk of the image can be colourised this way. For example, the image above becomes this much coloured:
![image](etc/mokkiarea-firstpass.png)

The tiles that haven't been resolved contain colours that aren't in any of the four colour tiles. Taking some examples from the image, the gravelly road has a true colour shade of grey that isn't present anywhere else, even though the white part of the road is. Same with the windows, there is a bright yellow that only appears in the windows and in no four colour tiles.

#### 2. Recolour Based on transparent tiles
PNGs are great when working with pixel-based projects, and working with a Gameboy is no different. However while the programs we use might easily undertand transparent pixels, a Gameboy doesn't. Meaning we need to replace the transparent pixels. For this, Gameboy Colouriser will convert any transparent pixel into the darkest Gameboy shade. I've found that often my transparent pixels are the "void", or the bits outside of the playable area and since Pokémon does that (check it out next time you're inside a small building) this does the same.

It might be a bit hard to see depending on your dark/light setting, but there is now dark tiles around the outside of the map. 

![image](etc/mokkiarea-secondpass.png)

Note that the insides for the paths, windows, and flowers are still transparent - this is because those have yet to be processed.

#### 3. Recolour based on existing tile colours
Previously in step 1, we just focused on four colour tiles and their full subset children.

1. Take a global image dictionary of each true colour and corresponding Gameboy colour
1. Weight each true colour/Gameboy colour based on which true colour has the most Gameboy colours. **Note**: This is the first bit of guesswork and could be a source of problems. Depending on the image, a true colour could map to serveral Gameboy colours depending on the make up of that true colour tile. See the example earlier about a dark blue colour.
1. When a true colour is found in the dictionary, paint that pixel with the corresponding most-weighted Gameboy colour.

![image](etc/mokkiarea-thirdpass.png)

Here we see more of the image filled in. Things like:
- The gravel paths are filled in because the whiter shade already existed in the other paths and the original grey was found on the sign. 
- The buildings and windows (minus the bright yellow) are filled in

#### 4. Recolour based on nearest similar colours
This is the biggest guesswork step. We get to this here when there are unique colours not found in any four colour tiles. For this step the process is:

1. Take the weighted global dictionary of colours and order from brightest to darkest.
2. For each tile with an unmapped pixel colour
	1. Get the brightness of the unmapped pixel
	1. Find the closest brightness of another true colour in the global dictionary
	1. Map the unmapped colour to the Gameboy colour of the closest true colour brightness
	1. Add the new mapping to the global dictionary

While this does have some guesswork, it has proven good enough for my purposes so far.

And that's how we get the final image from Gameboy Color to Gameboy:

![image](etc/mokkiarea-eachpasswithoriginal.gif)

### Brightness
It turns out, brightness isn't as easy as I thought. This project uses `System.Drawing.Color` which has a [`.GetBrightness()`](https://docs.microsoft.com/en-us/dotnet/api/system.drawing.color.getbrightness?view=net-6.0) method, except this ordered some colours in a way I didn't expect. For instance the colour on the left is considered brighter than the colour on the right.

![iamge](etc/BrightnessComparison.jpeg)

To get around this I looked for a custom implementation and that's when I realised there are all sorts of ways to try to capture the concept of "brightness". In the end I found an ancient (2008) post called [Calculating the Perceived Brightness of a Color](https://www.nbdtech.com/Blog/archive/2008/04/27/calculating-the-perceived-brightness-of-a-color.aspx) by [Nir Dobovizki](https://twitter.com/NirDobovizki). And used that algorithm - which has suited my use cases so far.

## Previous attempts
- Originally each tile just had it's colours ordered brightest to darkest and using the C# LINQ `.Zip()` call to try to marry them up.
- There was also fiddly mapping of the black and white colours to try to reduce the problem space.
- Ultimately my main problem was tiles of single colours that weren't present elsewhere. 
- Below is the result of an early algorithm idea which due to guesswork by brightnesses lead to weird situations with roofing. 

![image](etc/mokkiarea-oldalgorithm.png)


## Other Notes
- Originally this was in the [Pokemon Gen 2 Style Tilemap](https://github.com/nikouu/Pokemon-gen-2-style-tilemap) and when it grew too big, it came to live here.
- The code is roughly equivalent to mad ravings.
- The working with four colour tiles, then each colour subset tile came to me in a dream.
