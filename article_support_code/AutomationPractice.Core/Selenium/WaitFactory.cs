using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Linq.Expressions;

namespace AutomationPractice.Core.PageObjects;

//public class WaitFactory<T> : IFactory<DefaultWait<T>>
//{
//    private readonly T _waitContext;
//    private readonly TimeSpan _timeOut;

//    public WaitFactory(T waitContext, TimeSpan timeOut)
//    {
//        _waitContext = waitContext;
//        _timeOut = timeOut;
//    }
//    public WaitFactory(T waitContext) : this(waitContext, TimeSpan.FromSeconds(UiTestSession.Current.Settings.DefaultTimeoutSeconds))
//    {
//    }
//    public DefaultWait<T> Create()
//    {
//        if (_waitContext is IWebDriver driver)
//        {
//            return new WebDriverWait(driver, _timeOut);
//        }
//        return new DefaultWait<T>(_waitContext)
//        {
//            Timeout = _timeOut
//        };
//    }   

//    internal object Until(Expression<Func<ISearchContext, bool>> expression)
//    {        
//        return Create().Until(e => expression.Compile().Invoke(e as ISearchContext));
//    }
//}
