using System.CommandLine.Parsing;

namespace GameboyColourDecolouriser.cli
{
    public static class ParseResultExtensions
    {
        public static void ShowHelp(this ParseResult parseResult)
        {
            // take from the start of the list until we hit an option/--/unparsed token
            // since commands can have arguments, we must take those as well in order to get accurate help
            var tokenList = parseResult.Tokens.TakeWhile(token => token.Type == TokenType.Argument || token.Type == TokenType.Command || token.Type == TokenType.Directive).Select(t => t.Value).ToList();
            tokenList.Add("-h");
            CommandParser.Instance.Parse(tokenList).Invoke();
        }
    }
}
