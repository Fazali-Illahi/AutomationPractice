using AutomationPractice.Core.Models;
using AutomationPractice.Core.Selenium;
using OpenQA.Selenium;

namespace AutomationPractice.Core.PageObjects;

public class ProductElement : PageObject
{
    private IWebElement _addTocart => _parentContext.SearchElement(By.CssSelector($".add_to_cart_button"));
    private IWebElement _parentContext;
    public Product Data => GetData();
    private string Name => _parentContext.GetConvertedText(By.TagName("h3"), d => d.Trim())!;

    private decimal ActualPrice => _parentContext.GetAmount(By.XPath(".//del/span[contains(@class,'amount')]|.//span/span[contains(@class,'amount')]"));
    private decimal Price => _parentContext.GetAmount(By.XPath(".//ins/span[contains(@class,'amount')]"));

    public ProductElement(IWebDriver driver,string name):base(driver)
    {
        _parentContext = driver.SearchElement(By.XPath($"//li[.//h3[contains(text(),'{name}')]]"));
    }

    public void AddToBasket()
    {
        _addTocart.EnsureClick();
        WaitFor(WaitConditions.AjaxRequestToComplete<ProductElement>());
    }

    private Product GetData() => new Product
    {
        Name = Name,
        ActualPrice = ActualPrice,
        DiscountedPrice = Price
    };

    
}
