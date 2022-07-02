using GameboyColourDecolouriser.cli.Decolourise;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

namespace GameboyColourDecolouriser.cli
{
    internal static class CommandParser
    {
        public static readonly RootCommand RootCommand = new RootCommand();

        public static readonly Command[] Subcommands = new Command[]
        {
            // add command parsers here
            DecolouriseCommandParser.GetCommand()
        };

        // root options
        public static readonly Option<bool> VersionOption = new Option<bool>("--version");

        private static Command ConfigureCommandLine(Command rootCommand)
        {
            foreach (var subcommand in Subcommands)
            {
                rootCommand.AddCommand(subcommand);
            }

            // add options if needed


            // add arguments if needed

            return rootCommand;
        }

        public static Parser Instance { get; } = new CommandLineBuilder(ConfigureCommandLine(RootCommand))
            //.UseExceptionHandler()
            .UseHelp()
            //.UseHelpBuilder()
            //.UseSuggestDirective()
            //.EnableLegacyDoubleDashBehavior()
            .Build();
    }
}
