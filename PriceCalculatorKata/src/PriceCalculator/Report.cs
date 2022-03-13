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
        const double noAmount = 0d;
        var report = string.Empty;
        var discountAmount = 0d;
        var taxAmount = Tax.GetTax().TaxValue;
        foreach (var product in _products)
        {
            report += $"Cost = ${product.Price}\n";
            if (taxAmount != noAmount) report += $"Tax = ${product.CalculateTaxValue()} \n";
            discountAmount = product.CalculateDiscountsValue();
            if (discountAmount != noAmount) report += $"Discounts = ${discountAmount} \n";
            report += product.GetExpenseInfo();
            report += $"Total = ${product.CalculateFinalPrice()} \n";
            if (discountAmount == noAmount) return report;
            report += $"${discountAmount} amount which was deduced";
        }

        return report;
    }
}