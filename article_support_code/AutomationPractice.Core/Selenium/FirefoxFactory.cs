using AutomationPractice.Common;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace AutomationPractice.Core.Selenium;

public class FirefoxFactory : INamedBrowserFactory
{
    private readonly SessionSettings _options;
    private readonly string[] implicitlyDownloadedFileTypes =
        new[] {
            "text/csv"
            ,"text/plain"
        };

    public FirefoxFactory(SessionSettings options)
    {
        _options = options;
    }
    public IWebDriver Create()
    {
        var driverService = FirefoxDriverService.CreateDefaultService(_options.DriverPath);
        var options = new FirefoxOptions();
        if (_options.Headless)
        {
            options.AddArgument("-headless");
        }
        options.AddArgument("-private");
        options.SetPreference("browser.download.folderList", 2);
        options.SetPreference("browser.download.dir", _options.DownloadDirectory);
        options.SetPreference("network.cookie.cookieBehavior", 0);
        options.SetPreference("browser.helperApps.neverAsk.saveToDisk", string.Join(",", implicitlyDownloadedFileTypes));
        var driver = new FirefoxDriver(driverService, options, TimeSpan.FromSeconds(_options.DefaultTimeoutSeconds));
        driver.Manage().Window.Maximize();
        return driver;
    }
    public Browsers Name => Browsers.Firefox;
}