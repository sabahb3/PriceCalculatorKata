using PriceCalculator.Enumerations;
using PriceCalculator.Structures;

namespace PriceCalculator;

public interface IProduct
{
    public string Name { get; set; }
    public int UPC { get; set; }
    public double Price { get; }
    public double CalculateTaxValue();
    public double CalculateUniversalDiscountValue(double price);
    public double CalculateDiscountsValue();
    public double CalculateFinalPrice();
    public void SetSpecialDiscount(string value);
    public double CalculateUpcDiscountValue(double price);
    public void AddExpense(IExpenses expense);
    public string GetExpenseInfo();
    public Currency CurrencyCode { get; }
}