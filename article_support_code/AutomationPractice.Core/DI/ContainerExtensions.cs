using System.Reflection;
using AutomationPractice.Common;
using AutomationPractice.Core.DI.Containers;
using AutomationPractice.Core.PageObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AutomationPractice.Core.DI;

public static class ContainerExtensions
{
    public static IServiceCollection RegisterContainers(this IServiceCollection collection)
    {
        var type = typeof(IServiceContainer);
        foreach (var container in Assembly.GetAssembly(type)!.GetTypes().Where(t=>t.IsClass && type.IsAssignableFrom(t)))
        {
            //Containers should be in AutomationPractice.Core.DI.Containers namespace
            Conventions.Enforce(container,
                c => c.Namespace!.StartsWith(type.Namespace!),
           $"{container.GetFriendlyTypeName()} is not in a valid namespace.");
            var constructors = container.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            //Containers should have only default constructors.
            Conventions.Enforce(constructors,
                c => c.Length == 1 && c[0].GetParameters().Length < 1,
                $"{container.GetFriendlyTypeName()} must only have a default constructor.");
            (Activator.CreateInstance(container) as IServiceContainer)!.Register(collection);
        }
        return collection;
    }
    
    public static IServiceCollection RegisterPages(this IServiceCollection collection)
    {
        var type = typeof(UiPageBase<>);
        foreach (var page in Assembly.GetAssembly(type)!.GetTypes().Where(t=>t.Namespace!.StartsWith("AutomationPractice.Core.PageObjects") && t.Name.EndsWith("Page")))
        {
            collection.AddTransient(page);
        }
        return collection;
    }

    public static IServiceCollection UseTestConfiguration<T>(this IServiceCollection container) where T : class,new()
    {
        var filePath = Environment.GetEnvironmentVariable("SessionSettings");
        filePath=string.IsNullOrWhiteSpace(filePath) ? "test-settings.json" : filePath;
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(filePath);
        }
        var config = new ConfigurationBuilder()
            .AddJsonFile(filePath, false, true)
            .Build();
        var settings = new T();
        new ConfigureFromConfigurationOptions<T>(config.GetSection("TestSessionSettings")).Configure(settings);
        return container
            .AddSingleton(config)
            .AddSingleton(settings);
    }
}