using AutomationPractice.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationPractice.Core.Selenium;

public class ChromeFactory : INamedBrowserFactory
{
    private readonly SessionSettings _options;
    public ChromeFactory(SessionSettings options)
    {
        _options = options;
    }
    public IWebDriver Create()
    {
        var driverService = ChromeDriverService.CreateDefaultService(_options.DriverPath);
        var options = new ChromeOptions();
        if (_options.Headless)
        {
            options.AddArgument("headless");
        }
        options.AddArgument("--no-sandbox");
        options.AddArgument("--start-maximized");
        options.AddUserProfilePreference("download.default_directory", _options.DownloadDirectory);
        options.AddUserProfilePreference("profile.cookie_controls_mode", 0);
        options.SetLoggingPreference(LogType.Browser, LogLevel.All);
        return new ChromeDriver(driverService, options, TimeSpan.FromSeconds(_options.DefaultTimeoutSeconds));
    }
    public Browsers Name => Browsers.Chrome;
}