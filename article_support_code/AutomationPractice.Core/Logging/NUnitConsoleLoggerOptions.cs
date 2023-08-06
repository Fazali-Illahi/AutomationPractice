using Microsoft.Extensions.Logging;

namespace AutomationPractice.Core.Logging
{
    public class NUnitConsoleLoggerOptions
    {
        public bool LogToTestConsole { get; set; }
        public string DateTimeFormat { get; set; } = "u";

        public List<LogLevel> AllowedLevels { get; private set; } = new()
        {
            LogLevel.Information,
            LogLevel.Error,
            LogLevel.Warning,
            LogLevel.Critical
        };
    }
}