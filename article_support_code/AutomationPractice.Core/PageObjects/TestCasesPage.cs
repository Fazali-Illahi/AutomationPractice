using OpenQA.Selenium;
using OpenQA.Selenium.Support;

namespace AutomationPractice.Core.PageObjects;

public class TestCasesPage : UiPageBase<TestCasesPage>
{
    public TestCasesPage(IWebDriver driver) : base(driver)
    {
        
    }
    protected override string UrlSegment => "/test-cases/";
}