using PriceCalculator.Structures;

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
            report += $"Cost = {new FormattedDouble(product.Price).FinalNumber} {currencyCode}\n";
            if (taxAmount != noAmount)
                report += $"Tax = {new FormattedDouble(product.CalculateTaxValue()).FinalNumber} {currencyCode} \n";
            discountAmount = product.CalculateDiscountsValue();
            if (discountAmount != noAmount)
                report += $"Discounts = {new FormattedDouble(discountAmount).FinalNumber} {currencyCode} \n";
            report += product.GetExpenseInfo();
            report += $"Total = {new FormattedDouble(product.CalculateFinalPrice()).FinalNumber} {currencyCode} \n";
            if (discountAmount == noAmount) return report;
            report += $"{new FormattedDouble(discountAmount).FinalNumber} {currencyCode} amount which was deduced";
        }

        return report;
    }
}