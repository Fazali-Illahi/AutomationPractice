using AutomationPractice.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutomationPractice.Core.DI;

public static class  ServiceRegistry
{
    public static IServiceProvider Register()
    {
        var collection = new ServiceCollection();
        collection
            .AddLogging(c => {
                c.ClearProviders();
                c.AddNUnitConsoleLogger(o=> o.LogToTestConsole=true);                
            })
            .RegisterContainers()
            .RegisterPages();
        return collection.BuildServiceProvider();
    }
    
}