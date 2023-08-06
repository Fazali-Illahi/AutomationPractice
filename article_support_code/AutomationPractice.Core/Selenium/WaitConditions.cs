using System.Linq.Expressions;
using AutomationPractice.Common;
using OpenQA.Selenium;

namespace AutomationPractice.Core.Selenium;

public static class WaitConditions
{

    public static Expression<Func<ISearchContext, IWebElement>> ElementExists(By locator) => ctx => ctx.FindElement(locator);
    public static Expression<Func<ISearchContext, IWebElement>> ElementToBeClickable(IWebElement? element) => ctx => Clickable(element)!;
    public static Expression<Func<ISearchContext, IWebElement>> ElementToBeVisible(By locator) => ctx => Visible(ctx, locator)!;
    public static Expression<Func<ISearchContext, bool>> AjaxRequestToComplete<T>() where T : ISearchContext => d=>AjaxRequestToComplete(d);   

    private static bool AjaxRequestToComplete<T>(T context) where T: ISearchContext
    {
        try
        {
            var value= context.ExecuteScript("return jQuery.active===0;");
            return Convert.ToBoolean(value);
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static IWebElement? Clickable(IWebElement? webElement)
    {
        try
        {
            return (webElement?.Enabled).GetValueOrDefault()  ? webElement : null;
        }
        catch (StaleElementReferenceException)
        {
            return null;
        }
    }
    
    private static IWebElement? Visible(ISearchContext context,By locator)
    {
        var webElement = Visible(context.FindElement(locator));
        return Visible(webElement);
    }
    private static IWebElement? Visible(IWebElement? webElement)
    {
        try
        {
            return (webElement?.Displayed).GetValueOrDefault() ? webElement : null;
        }
        catch (StaleElementReferenceException)
        {
            return null;
        }
    }
    
}
