using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace AutomationPractice.Core.PageObjects
{
    public abstract class PageObject: IWrapsDriver, ISearchContext, ITakesScreenshot, IDisposable
    {
        private readonly IWebDriver _driver;
        private ISearchContext _innerContext;
        private bool disposedValue;

        public PageObject(IWebDriver driver)
        {
            _driver = driver;
            _innerContext = driver;
        }
        public void SetContext(By contextSelector)
        {
            _innerContext = _driver.FindElement(contextSelector);
        }

        public IWebDriver WrappedDriver => _driver;

        public IWebElement FindElement(By by)
        {
            return _innerContext.FindElement(by);
        }


        public ReadOnlyCollection<IWebElement> FindElements(By by) => _innerContext.FindElements(by);

        public Screenshot GetScreenshot()
        {
           return (_innerContext as ITakesScreenshot)?.GetScreenshot()!;
        }


        public bool WaitFor(Expression<Func<ISearchContext, bool>> condition)
        {
            return new WebDriverWait(_driver,TimeSpan.FromSeconds(UiTestSession.Current.Settings.DefaultTimeoutSeconds)).Until(d=>condition.Compile().Invoke(d));
        }

        public T WaitFor<T>(Expression<Func<ISearchContext, T>> condition) where T : ISearchContext
        {
            return new WebDriverWait(_driver, TimeSpan.FromSeconds(UiTestSession.Current.Settings.DefaultTimeoutSeconds)).Until(d => condition.Compile().Invoke(d));
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _driver.Quit();
                    _driver.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
