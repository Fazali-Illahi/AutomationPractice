using AutomationPractice.Common;
using AutomationPractice.Core.Selenium;
using OpenQA.Selenium;
using System.Linq.Expressions;

namespace AutomationPractice.Core.PageObjects
{
    public abstract class UiPageBase : PageObject
    {
        protected abstract string UrlSegment { get; }

        protected readonly SessionSettings _settings = UiTestSession.Current.Settings;

        public string Url => $"{_settings.ApplicationUrl}{UrlSegment}";

        public virtual bool IsOpen => WrappedDriver.Url.Contains(UrlSegment);

        public UiPageBase(IWebDriver driver) : base(driver)
        {           
        }

        public void Close()
        {
            WrappedDriver.Close();
        }

        
    }
}