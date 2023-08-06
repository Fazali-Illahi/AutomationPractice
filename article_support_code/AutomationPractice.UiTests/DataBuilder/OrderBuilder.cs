using AutomationPractice.Core.Models;

namespace AutomationPractice.UiTests.DataBuilder
{
    // see Fluent Builder Pattern.
    internal class OrderBuilder
    {
        public OrderBuilder(bool allowZeroQuantity=false)
        {
            _allowZeroQuantity = allowZeroQuantity;
        }
        private readonly Order _order = new Order();
        private readonly bool _allowZeroQuantity;

        public OrderBuilder AddProduct(Product product, uint quantity)
        {
            if (quantity > 0 || _allowZeroQuantity)
            {
                if (_order.OrderedProducts.ContainsKey(product))
                {
                    _order.OrderedProducts[product] += quantity;
                }
                else
                {
                    _order.OrderedProducts.Add(product, quantity);
                }
            }
            return this;
        }
        public OrderBuilder ApplyDiscount(decimal discount, bool isFlatAmount)
        {
            _order.Discount = discount;
            _order.IsFlat = isFlatAmount;
            return this;
        }

        public Order Build()
        {
            return _order;
        }
    }
}
