using Microsoft.Extensions.DependencyInjection;

namespace AutomationPractice.Core.DI;

public static class  ServiceRegistry
{
    public static IServiceProvider Register()
    {
        var collection = new ServiceCollection();
        collection.RegisterContainers();
        return collection.BuildServiceProvider();
    }
    
}