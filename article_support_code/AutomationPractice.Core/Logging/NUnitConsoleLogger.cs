using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace AutomationPractice.Core.Logging
{
    public class NUnitConsoleLogger : ILogger<string>
    {
        private readonly string _name;
        private readonly NUnitConsoleLoggerOptions _settings;

        public NUnitConsoleLogger(string name, Func<NUnitConsoleLoggerOptions> getCurrentConfig)
        {
            _name = name;
            _settings = getCurrentConfig();
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return default;
        }

        public bool IsEnabled(LogLevel logLevel) =>
            _settings.AllowedLevels.Contains(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            if (_settings.LogToTestConsole)                
            {
                var timeStamp = $"[{DateTime.UtcNow.ToString(_settings.DateTimeFormat)}]";
                var reproStep = formatter(state, exception);
                var log = $"{timeStamp} [ {logLevel,-12}] {_name} - {reproStep}";
                UiTestSession.Current.ExecutionContext.CurrentTest.Properties.Add("ReproSteps", reproStep);
                Console.WriteLine(log);
            }            
        }
    }
}