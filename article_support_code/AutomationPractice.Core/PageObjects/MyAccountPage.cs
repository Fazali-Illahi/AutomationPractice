using AutomationPractice.Core.Models;
using AutomationPractice.Core.Selenium;
using OpenQA.Selenium;
using System.Linq.Expressions;

namespace AutomationPractice.Core.PageObjects;
public class MyAccountPage : UiPageBase<MyAccountPage>
{
    public MyAccountPage(IWebDriver driver) : base(driver)
    {
    }
    protected override string UrlSegment => "/my-account/";
}

public class BasketPage : UiPageBase<BasketPage>
{
    private readonly IWebDriver _driver;

    public bool IsEmpty => this.HasElement(By.CssSelector(".cart-empty"));

    public decimal Subtotal => _driver.GetAmount(By.CssSelector(".cart-subtotal .amount"));
    public decimal Tax => _driver.GetAmount(By.CssSelector(".tax-rate .amount"));
    public decimal Total => _driver.GetAmount(By.CssSelector(".tax-rate .amount"));

    public IWebElement UpdateBasketButton => _driver.FindElement(By.Name("update_cart"));
    public IWebElement ApplyCouponButton => _driver.FindElement(By.Name("apply_coupon"));

    public IWebElement ProceedToCheckoutButton => _driver.FindElement(By.CssSelector(".checkout-button"));

    public IWebElement CouponCode => _driver.FindElement(By.Id("coupon_code"));

    public BasketPage(IWebDriver driver) : base(driver)
    {
        _driver = driver;
    }
    protected override string UrlSegment => "/basket/";

    public BasketPage PlaceOrder(Order order)
    {
        List<OrderedProduct>? Products = new List<OrderedProduct>();
        foreach (var product in order.OrderedProducts)
        {
            var productElement = new OrderedProduct(this, product.Key);
            productElement.SetQuantity(product.Value);
            Products.Add(productElement);
        }
        UpdateBasketButton.EnsureClick();
        WaitFor(WaitConditions.AjaxRequestToComplete<BasketPage>());
        ProceedToCheckoutButton.EnsureClick();
        WaitFor(WaitConditions.AjaxRequestToComplete<BasketPage>());
        /*
         use order.BillingDetails to choose pass Biling Details
         use order.PaymentMethod to choose the relevant payment method
         use order.Notes to add notes to order
         use order.CreateAccount| use order.BillingDetails.CreateAccount to add create new account.
         Don't hardcode anything inside this method. 
         */
        return this;
    }

   
}

public class OrderedProduct
{
    //divide and conquer: Find Top Level element.
    private IWebElement _parentRow => parent.FindElement(By.XPath($"//tr[.//a[contains(text(),'{_product.Name}')]]"));
    private readonly UiPageBase parent;
    private readonly Product _product;

    public decimal Tax => _parentRow.GetAmount(By.CssSelector(".tax-rate .amount"));
    public decimal Total => _parentRow.GetAmount(By.CssSelector(".tax-rate .amount"));

    private IWebElement Quantity => _parentRow.FindElement(By.CssSelector(".qty"));

    private IWebElement RemoveButton => _parentRow.FindElement(By.CssSelector(".remove"));

    public OrderedProduct(UiPageBase parent, Product product)
    {
        this.parent = parent;
        _product = product;
    }

    public void SetQuantity(uint quantity,bool deleteIfZero=true)
    {
        if(quantity == 0 && deleteIfZero)
        {
            Remove();
        }
        Quantity.EnsureSendKeys(quantity);
    }

    private void Remove()
    {
        RemoveButton.EnsureClick();
        parent.WaitFor(WaitConditions.AjaxRequestToComplete<UiPageBase>());
    }
}
