namespace PriceCalculator;

public class Report
{
    private readonly List<IProduct> _products;

    public Report(List<IProduct> products)
    {
        _products = products;
    }

    public string Reporting()
    {
        var noDiscount = 0d;
        var report = string.Empty;
        var discountAmount = 0d;
        foreach (var product in _products)
        {
            report += $"Price ${product.CalculateFinalPrice()}\n";
            discountAmount = product.CalculateDiscountsValue();
            if (discountAmount == noDiscount) return report;
            report += $"${discountAmount} amount which was deduced";
        }

        return report;
    }
}