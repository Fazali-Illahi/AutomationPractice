namespace AutomationPractice.Core.Models
{
    public class Order
    {
        //Not efficient way to handle this. Keys should not be a complex(class instance){Implementing a Test Data Repository using Repository Pattern is the recommended approach} 
        public Dictionary<Product, uint> OrderedProducts { get; set; } = new Dictionary<Product,uint>();

        public decimal Discount { get; set; }

        public bool IsFlat { get; set; }

        public decimal TotalDiscount => CalculateDiscount();

        private decimal CalculateDiscount()
        {
            if (IsFlat)
            {
                return Discount;
            }
            else
            {
                return TotalPrice * Discount / 100m;
            }
        }

        public decimal TotalPrice => OrderedProducts.Sum(p => p.Value * p.Key.DiscountedPrice);
        public decimal OrderTotal => TotalPrice - TotalDiscount;
        
       
    }
}
