using GameboyColourDecolouriser.cli.Decolourise;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace GameboyColourDecolouriser.cli
{
    internal static class CommandParser
    {
        public static readonly RootCommand RootCommand = new();

        // Subcommands
        public static readonly Command[] Subcommands = new Command[]
        {
            // add command parsers here
            DecolouriseCommandParser.GetCommand()
        };

        // root options
        //public static readonly Option<bool> VersionOption = new Option<bool>("--version");

        // root arguments      

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
            .UseExceptionHandler(ExceptionHandler)
            .UseParserErrorHandler()
            .UseHelp()
            //.UseHelpBuilder()
            //.UseSuggestDirective()
            //.EnableLegacyDoubleDashBehavior()
            .Build();

        private static void ExceptionHandler(Exception exception, InvocationContext context)
        {
            Console.Error.WriteLine(exception.Message);
            context.ExitCode = 1;
        }

        private static CommandLineBuilder UseParserErrorHandler(this CommandLineBuilder builder)
        {
            builder.AddMiddleware(async (context, next) =>
            {
                if (context.ParseResult.Errors.Any())
                {
                    context.ExitCode = 127; //parse error

                    foreach (var error in context.ParseResult.Errors)
                    {
                        Console.Error.WriteLine(error.Message);
                    }
                    context.ParseResult.ShowHelp();
                }
                else
                {
                    await next(context).ConfigureAwait(false);
                }
            }, MiddlewareOrder.ErrorReporting);
            return builder;
        }
    }
}
