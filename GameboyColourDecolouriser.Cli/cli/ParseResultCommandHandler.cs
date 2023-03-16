using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace GameboyColourDecolouriser.cli
{
    internal class ParseResultCommandHandler : ICommandHandler
    {
        private readonly Func<ParseResult, int> _action;

        internal ParseResultCommandHandler(Func<ParseResult, int> action)
        {
            _action = action;
        }

        public Task<int> InvokeAsync(InvocationContext context) => Task.FromResult(_action(context.ParseResult));
        public int Invoke(InvocationContext context) => _action(context.ParseResult);
    }
}
