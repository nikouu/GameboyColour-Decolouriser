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

        private static readonly Command Command = ConstructCommand();

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

            command.Handler = new ParseResultCommandHandler(DecolouriseCommand.Run);

            return command;
        }
    }
}
