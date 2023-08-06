using AutomationPractice.Core.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationPractice.Core.PageObjects;

public class ShopPage : UrlNavigatedPage<ShopPage>
{
    
    public override bool IsOpen => WaitFor(WaitConditions.ElementToBeVisible(By.CssSelector("ul.products"))).Displayed;
    protected override string UrlSegment => "/shop/";

    private IWebElement BasketLink => this.SearchElement(By.CssSelector("#wpmenucartli>a"));

    public ProductElement GetProductElement(string name) => new ProductElement(WrappedDriver, name);
    public ShopPage(IWebDriver driver) : base(driver)
    {
    }
    
    public BasketPage OpenBasket()
    {
        WrappedDriver.SearchElement(By.CssSelector(""));
        var  element=this.SearchElement(By.CssSelector(""));
        element.SearchElement(By.CssSelector(""));
        var wait = new ShopPageWait(this);
        wait.Until(d=>d.IsProductValid());
        BasketLink.EnsureClick();
        return new BasketPage(WrappedDriver);
    }

    public bool IsProductValid()
    {
        return true;
    }
}

public class ShopPageWait : DefaultWait<ShopPage>
{
    public ShopPageWait(ShopPage input) : base(input)
    {
    }
}
