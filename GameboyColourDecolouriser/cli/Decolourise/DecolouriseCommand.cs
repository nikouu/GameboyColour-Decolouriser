using GameboyColourDecolouriser.Models;
using Spectre.Console;
using System.CommandLine.Parsing;

namespace GameboyColourDecolouriser.cli.Decolourise
{
    internal class DecolouriseCommand
    {
        public static int Run(ParseResult parseResult)
        {
            var inputFile = parseResult.GetValueForArgument(DecolouriseCommandParser.InputFileArgument);
            var outputFile = parseResult.GetValueForArgument(DecolouriseCommandParser.OutputFileArgument);

            if (inputFile is null)
            {
                throw new ArgumentException("No input file given.");
            }

            if (outputFile is null)
            {
                throw new ArgumentException("No ouput file given.");
            }

            if (outputFile?.DirectoryName is null)
            {
                throw new ArgumentException("No ouput file directory given.");
            }

            if (!File.Exists(inputFile.FullName))
            {
                throw new FileNotFoundException($"Input file {inputFile.Name} does not exist.");
            }

            Console.WriteLine($"Input file {inputFile.Name}, output file {outputFile.Name}");

            AnsiConsole.Progress()
            .Start(ctx =>
            {

                // Define tasks
                var loadingImage = ctx.AddTask("[green]Loading Image[/]");
                var decolourStageOne = ctx.AddTask("[green]Decolouring Stage One[/]");
                var decolourStageTwo = ctx.AddTask("[green]Decolouring Stage Two[/]");
                var decolourStageThree = ctx.AddTask("[green]Decolouring Stage Three[/]");
                var decolourStageFour = ctx.AddTask("[green]Decolouring Stage Four[/]");
                var generatingFinalImage = ctx.AddTask("[green]Generating Final Image[/]");

                var spectreTasks = new SpectreTasks(decolourStageOne, decolourStageTwo, decolourStageThree, decolourStageFour, generatingFinalImage);

                var gbcImage = ImageConverter.ToGbcImage(inputFile.FullName, loadingImage);

                var decolouriser = new Decolouriser();

                var gbImage = decolouriser.Decolourise(gbcImage, spectreTasks);

                if (!Directory.Exists(outputFile.DirectoryName))
                {
                    Directory.CreateDirectory(outputFile.DirectoryName);
                }

                var recolouredImageBytes = ImageConverter.ToImageBytes(gbImage, generatingFinalImage);
                File.WriteAllBytes(outputFile.FullName, recolouredImageBytes);                
            });

            return 0;
        }
    }
}
