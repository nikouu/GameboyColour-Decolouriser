using System.CommandLine;

namespace GameboyColourDecolouriser.cli.Decolourise
{
    internal static class DecolouriseCommandParser
    {
        public static readonly Argument<FileInfo> InputFileArgument = new Argument<FileInfo>()
        {
            Name = "input",
            Description = "The path of the input Gameboy Colour image."
        };

        public static readonly Argument<FileInfo> OutputFileArgument = new Argument<FileInfo>()
        {
            Name = "output",
            Description = "The path of the output Gameboy image."
        };

        public static readonly Option<bool> ExportColourData = new Option<bool>(name: "--export-colour-data")
        {
            Description = "Export colour palette information."
        };

        // part of the singleton pattern. static here because then there will always be just one instance of the instance property
        // when called for the first time it is instntiated and that's the only one
        // this is the private backing property that stores the instance
        private static readonly Command Command = ConstructCommand();

        // the only way we can get to the private backing instance
        public static Command GetCommand()
        {
            return Command;
        }

        private static Command ConstructCommand()
        {
            var command = new Command("Decolourise", "Decolourises a Gameboy Color image into a Gameboy image.");
            command.AddAlias("Decolourize");

            command.AddArgument(InputFileArgument);
            command.AddArgument(OutputFileArgument);
            command.AddOption(ExportColourData);

            command.Handler = new ParseResultCommandHandler(DecolouriseCommand.Run);

            return command;
        }
    }
}
