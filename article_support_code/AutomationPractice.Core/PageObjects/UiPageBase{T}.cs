using AutomationPractice.Common;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Linq.Expressions;

namespace AutomationPractice.Core.PageObjects;

public abstract class UiPageBase<T> : UiPageBase where T : UiPageBase<T>
{
    public UiPageBase(IWebDriver driver):base(driver)
    {
        Conventions.Enforce((T)this, p => GetType().Equals(typeof(T)) , "Gereric Type arguments of base class and page object class name should be same.");
        Conventions.Enforce((T)this, p => {
            var pageConstructors = p.GetType().GetConstructors();
            return pageConstructors.Length==1 && pageConstructors[0].GetParameters().Length==1;        
        },"Pages Should have only one constructor with IWebDrver as the only parameter.");
    }
   
    public virtual string Name => typeof(T).GetFriendlyTypeName();

    
}

public abstract class UrlNavigatedPage<T> : UiPageBase<T> where T : UrlNavigatedPage<T>
{
    protected UrlNavigatedPage(IWebDriver driver) : base(driver)
    {
    }

    public virtual T Open()
    {
        UiTestSession.Current.Logger!.LogInformation($"Opening {this.GetFriendlyTypeName()} using url '{Url}'");
        WrappedDriver.Navigate().GoToUrl(Url);
        return (T)this;
    }

    
}

//public interface IWaitPage
//{
//    TOut WaitFor<TOut>(Expression<Func<ISearchContext, TOut>> condition);
//}