using AutomationPractice.Common;
using AutomationPractice.Core.DI;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;

namespace AutomationPractice.Core.Selenium;

public class WebDriverFactory
{
    private readonly  SessionSettings _driverOptions ;
    private readonly IServiceProvider _serviceProvider;

    public WebDriverFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _driverOptions = _serviceProvider.GetRequiredService<SessionSettings>();
    }

    public IWebDriver Create()
    {
        var factory= _serviceProvider.GetServices<INamedBrowserFactory>()
            .FirstOrDefault(f=>f.Name == _driverOptions.Browser);
        if (factory == null)
        {
            throw new ServiceNotRegisteredException($"No factory registered for {_driverOptions.Browser} browser.");
        }
        return factory.Create();
    }
}
