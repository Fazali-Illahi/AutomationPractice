using AutomationPractice.Core.Selenium;
using OpenQA.Selenium;

namespace AutomationPractice.Core.PageObjects;

public class HomePage : UrlNavigatedPage<HomePage>
{
    public IWebElement ShopLink => this.SearchElement(By.XPath(".//*[@id='main-nav']//a[contains(text(),'Shop')]"));
    public IWebElement MyAccountLink => this.SearchElement(By.XPath(".//*[@id='main-nav']//a[contains(text(),'My Account')]"));
    public IWebElement TestCaseLink => this.SearchElement(By.XPath(".//*[@id='main-nav']//a[contains(text(),'Test Case')]"));

    protected override string UrlSegment => string.Empty;

    public HomePage(IWebDriver driver) : base(driver)
    {
    }
        
    public ShopPage OpenShop()
    {
        ShopLink.EnsureClick();
        return new ShopPage(WrappedDriver);
    }
    public MyAccountPage OpenMyAccountPage()
    {
        MyAccountLink.EnsureClick() ;
        return new MyAccountPage(WrappedDriver);
    }

    public TestCasesPage OpenTestCasePage()
    {
        TestCaseLink.EnsureClick();
        return new TestCasesPage(WrappedDriver);
    }
}