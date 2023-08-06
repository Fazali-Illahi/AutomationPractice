using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace AutomationPractice.Core.Logging
{
    [ProviderAlias("NUnitConsoleLogger")]
    public sealed class NUnitConsoleLoggerProvider : ILoggerProvider
    {
        private readonly IDisposable _onChangeToken;
        private NUnitConsoleLoggerOptions _current;
        private readonly ConcurrentDictionary<string, NUnitConsoleLogger> _loggers =
            new(StringComparer.OrdinalIgnoreCase);

        public NUnitConsoleLoggerProvider(IOptionsMonitor<NUnitConsoleLoggerOptions> options)
        {
            _current = options.CurrentValue;
            _onChangeToken = options.OnChange(updatedOptions => _current = updatedOptions)!;
        }

        public ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, name => new NUnitConsoleLogger(name, GetOptions));

        private NUnitConsoleLoggerOptions GetOptions() => _current;

        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken.Dispose();
        }
    }
}