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
        var dicountAmount = 0d;
        foreach (var product in _products)
        {
            report += $"Price ${string.Format("{0:0.##}", product.CalculatePriceAfterDiscount())}\n";
            if (product.CalculateDiscountValue() == noDiscount) return report;
            dicountAmount = product.CalculateDiscountValue() + product.CalculateUpcDiscountValue();
            report += $"${string.Format("{0:0.##}", dicountAmount)} amount which was deduced";
        }

        return report;
    }
}