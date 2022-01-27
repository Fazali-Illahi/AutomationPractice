using AutomationPractice.Common;
using OpenQA.Selenium;

namespace AutomationPractice.Core;
public interface INamedBrowserFactory : IFactory<IWebDriver>
{
    Browsers Name { get; }
}

public interface IFactory<out T>
{
    T Create();
}