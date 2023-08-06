using AutomationPractice.Core.PageObjects;
using AutomationPractice.UiTests.DataBuilder;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AutomationPractice.UiTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class E2EShoppingTests : UiTestBase<ShopPage>
    {
        //This is not a good test and does not add value to testing.
        [UiTest]
        [TestCase("Android Quick Start Guide", 600, 450)]
        [TestCase("Functional Programming in JS", 250, 0)]
        public void ValidateProductPricesTest(string name, decimal actual, decimal discounted)
        {   
            var product = Wrap(()=>Page.Open().GetProductElement(name));
            Assert.IsNotNull(product);
            var prodModel = product.Data;
            Assert.That(prodModel.DiscountedPrice, Is.EqualTo(discounted));
            Assert.That(prodModel.ActualPrice, Is.EqualTo(actual));
        }
        //This is not a good test and does not add much value to testing if Place Order is not automated.
        [UiTest]
        public void AddProductToCartTest()
        {
            //Clear mini cart items before opening this page.
            Page.Open();
            var orderData = new Dictionary<string,uint> {
                ["Android Quick Start Guide"] =2,
                ["HTML5 Forms"]=1 
                };
            var builder = new OrderBuilder();
            foreach (var item in orderData)
            {
                var element= Page.GetProductElement(item.Key);
                element.AddToBasket();
                builder.AddProduct(element.Data, item.Value);
            }
            //Add Assert to verify mini cart price & Quantity
            //Add Assert to check products in Basket page
            //Validate Order Summary in Basket page
        }

        [UiTest]
        public void PlaceOrderTests()
        {
            Page.Open();
            var orderData = new Dictionary<string, uint>
            {
                ["Android Quick Start Guide"] = 2,
                ["HTML5 Forms"] = 1
            };
            var builder = new OrderBuilder();
            foreach (var item in orderData)
            {
                var product = Page.GetProductElement(item.Key);
                product.AddToBasket();
                builder.AddProduct(product.Data, item.Value);
            }
            Page.OpenBasket()
                .PlaceOrder(builder.Build());
            
            //Add Asserts here
        }

        public TOut Wrap<TOut>(Expression<Func<TOut>> method)
        {
            var name = method.ToString();
            Console.WriteLine($"Method execution started: {name}");
            return method.Compile().Invoke();
        }
    }
}
