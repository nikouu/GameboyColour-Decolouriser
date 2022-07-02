using GameboyColourDecolouriser.cli;
using System.CommandLine;
using System.CommandLine.Parsing;

var parseResult = CommandParser.Instance.Parse(args);

parseResult.Invoke();
