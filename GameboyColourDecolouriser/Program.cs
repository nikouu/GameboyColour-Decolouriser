using GameboyColourDecolouriser.cli;
using System.CommandLine;
using System.CommandLine.Parsing;

var parseResult = CommandParser.Instance.Parse(args);

if (parseResult.HasOption(CommandParser.VersionOption))
{
    // print version
    return;
}

parseResult.Invoke();
