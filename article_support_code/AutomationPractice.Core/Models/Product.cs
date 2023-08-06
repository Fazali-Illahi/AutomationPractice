namespace AutomationPractice.Core.Models
{
    public record class Product
    {
        public string? Name { get; init; }
        public decimal ActualPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
    }
}
