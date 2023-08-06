namespace AutomationPractice.Core.PageObjects
{
    public static class Converters
    {
       public static Converter<string, decimal> AmountConverter => d => decimal.Parse(d.Trim()[1..]);
    }
}
