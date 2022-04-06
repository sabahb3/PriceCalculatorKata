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
        var currencyCode = string.Empty;
        foreach (var product in _products)
        {
            currencyCode = product.CurrencyCode.CurrencyCode;
            report += $"Cost = {product.Price} {currencyCode}\n";
            if (taxAmount != noAmount) report += $"Tax = {product.CalculateTaxValue()} {currencyCode} \n";
            discountAmount = product.CalculateDiscountsValue();
            if (discountAmount != noAmount) report += $"Discounts = {discountAmount} {currencyCode} \n";
            report += product.GetExpenseInfo();
            report += $"Total = {product.CalculateFinalPrice()} {currencyCode} \n";
            if (discountAmount == noAmount) return report;
            report += $"{discountAmount} {currencyCode} amount which was deduced";
        }

        return report;
    }
}