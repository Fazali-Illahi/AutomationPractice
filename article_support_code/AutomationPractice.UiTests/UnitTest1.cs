using AutomationPractice.Core;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AutomationPractice.UiTests;

[TestFixture]
public class WebdriverInjectionTests
{
    private IWebDriver? _driver;
    [SetUp]
    public void Setup()
    {
        UiTestSession.Current.Start();
    }

    [Test]
    public void Test1()
    {
        _driver = UiTestSession.Current.Resolve<IWebDriver>();
        var url = UiTestSession.Current.Settings.ApplicationUrl;
        _driver.Navigate().GoToUrl(url);
        Assert.Pass();
    }

    [TearDown]
    public void CleanUp()
    {
        UiTestSession.Current.CleanUp();
        _driver?.Close();
        _driver?.Quit();
    }
}