using System.Linq.Expressions;
using System.Text;
using AutomationPractice.Common;
using AutomationPractice.Core.PageObjects;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationPractice.Core.Selenium;

public static class SearchContextExtensions
{
    private static ILogger CurrentLogger => UiTestSession.Current.Logger!;
    public static IWebElement SearchElement(this ISearchContext context, Expression<Func<ISearchContext, IWebElement>> condition, TimeSpan timeOut)
    {
        var messageBuilder = new StringBuilder();
        messageBuilder.Append(
            $"Searching element in {context.GetFriendlyTypeName()}, with timeout of {timeOut.Seconds} seconds.");
        var by = TryGetByFromCondition(condition.Body);
        if (by != null)
        {
            messageBuilder.Append($" by {by.Mechanism}({by.Criteria})");
        }
        CurrentLogger.LogInformation(messageBuilder.ToString());
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        var wait = new DefaultWait<ISearchContext>(context);
        wait.Timeout = timeOut;
        return wait.Until(condition.Compile());
    }

    public static bool HasElement(this ISearchContext parentContext,By selector)
    {
        try
        {
            return parentContext.FindElement(selector) != null;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public static IWebElement SearchElement(this ISearchContext context, Expression<Func<ISearchContext, IWebElement>> condition) => SearchElement(context, condition,
        TimeSpan.FromSeconds(UiTestSession.Current.Settings.DefaultTimeoutSeconds));

    public static IWebElement SearchElement(this ISearchContext context, By selector)
    {
        return SearchElement(context, WaitConditions.ElementExists(selector));
    }

    public static decimal GetAmount(this ISearchContext parent, By selector)
    {
        return parent.GetConvertedText(selector, Converters.AmountConverter);
    }

    public static T? GetTypedValue<T>(this IWebElement element)
    {
        try
        {
            var value = element.ExecuteScript("return arguments[0].value");
            return (T?)Convert.ChangeType(value,typeof(T));
        }
        catch 
        {
            return default;
        }
    }
    public static T? ExecuteScript<T>(this ISearchContext context, string script)
    {
        try
        {
            var value = ExecuteScript(context, script);
             return (T?)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            return default;
        }
    }
    

    public static object? ExecuteScript(this ISearchContext context, string script, bool log=false)
    {
        try
        {
            if (log)
            {
                CurrentLogger.LogInformation($"Executing script '{script}'");
            }
            var driver = context.GetDriver();
            if (context is IWebElement)
            {
                return (driver as IJavaScriptExecutor)?.ExecuteScript(script, context);
            }
            if (context is IWebDriver || context is IWrapsDriver)
            {
                return (driver as IJavaScriptExecutor)?.ExecuteScript(script);
            }
            return null;
        }
        catch 
        {
            return null;
        }
    }
    public static IWebDriver AttachedDriver(this ISearchContext context, string script)
    {
        var driver = (context as IWrapsDriver)?.WrappedDriver;
        if (driver == null)
        {
            throw new InvalidOperationException($"{context.GetFriendlyTypeName()} is not IWebDriver does not implement {nameof(IWrapsDriver)}");
        }
        return driver;
    }

    public static T? GetConvertedText<T>(this ISearchContext parent, By selector,Converter<string,T> converter)
    {
        IWebElement element;
        try
        {
            element = parent.SearchElement(selector);
            var value = element.Text;
            var type = typeof(T);
            CurrentLogger.LogInformation($"Converting text '{value}' of the element to {type.GetFriendlyTypeName()}");
           return converter(value);
        }
        catch (Exception)
        {
            return default;
        }
    }

    public static void EnsureClick(this IWebElement? element, TimeSpan timeOut)
    {
        CurrentLogger.LogInformation($"Clicking WebElement");
        if (element == null)
        {
            throw new ArgumentNullException(nameof(element));
        }
        var wait = new WebDriverWait(element.GetDriver(), timeOut);
        wait.Until(WaitConditions.ElementToBeClickable(element).Compile()).Click();
    }
    public static void EnsureClick(this IWebElement element) => EnsureClick(element,TimeSpan.FromSeconds(UiTestSession.Current.Settings.DefaultTimeoutSeconds));

    public static void EnsureSendKeys<T>(this IWebElement element, T keys) => EnsureSendKeys(element, keys, TimeSpan.FromSeconds(UiTestSession.Current.Settings.DefaultTimeoutSeconds));
    public static void EnsureSendKeys<T>(this IWebElement element,T keys,TimeSpan timeOut)
    {
        var wait = new DefaultWait<IWebElement>(element);
        wait.Timeout = timeOut;
        var value=keys!.ToString();
        CurrentLogger.LogInformation($"Sending keystrokes to webelement '{value}'");
        element.Clear();
        element.SendKeys(value);
        CurrentLogger.LogInformation($"Sending keystrokes to webelement '{value}'");
        wait.Until(e=>e.GetTypedValue<string>()==value);
    }

    private static IWebDriver? GetDriver(this ISearchContext? context)
    {
        switch (context)
        {
            case IWebDriver driver:
            return driver;
            case IWrapsDriver wrapsDriver:
                return wrapsDriver.WrappedDriver;
            default:
                throw new InvalidOperationException($"{context?.GetFriendlyTypeName() ?? "NULL"} is not IWebDriver does not implement {nameof(IWrapsDriver)}");
        }
    }
    
    private static By? TryGetByFromCondition(Expression expression)
    {
        if (expression is not MethodCallExpression method)
            return null;
        if (!method.Arguments.Any() || method.Arguments.FirstOrDefault(a=>a.Type==typeof(By)) is not MemberExpression member)
            return null;
        try
        {
            return Expression.Lambda<Func<By>>(Expression.Convert(member, typeof(By))).Compile().Invoke();
        }
        catch
        {
            return null;
        }
    }
   
}