using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace AutomationPractice.Core.Logging
{
    public static class TestSessionLoggerExtensions
    {
        public static ILoggingBuilder AddNUnitConsoleLogger(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, NUnitConsoleLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<NUnitConsoleLoggerOptions, NUnitConsoleLoggerProvider>(builder.Services);
            return builder;
        }

        public static ILoggingBuilder AddNUnitConsoleLogger(this ILoggingBuilder builder, Action<NUnitConsoleLoggerOptions> configure)
        {
            builder.AddNUnitConsoleLogger();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}