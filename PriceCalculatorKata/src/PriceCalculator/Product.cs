using PriceCalculator.Enumerations;
using PriceCalculator.Structures;

namespace PriceCalculator;

public delegate double DiscountAmount();

public class Product : IProduct
{
    private IDiscount _specialDiscount;
    private List<IExpenses> _expenses;

    public Product(string name, int upc, double price, IDiscount specialDiscount, List<IExpenses> expenses)
    {
        Name = name ?? " ";
        UPC = upc;
        SetPrice(price);
        _specialDiscount = specialDiscount;
        _expenses = expenses;
    }

    public string Name { get; set; } = string.Empty;
    public int UPC { get; set; }
    public double Price { get; private set; }

    private void SetPrice(double price)
    {
        var p = new FormattedDouble(price);
        Price = p.Number;
    }

    public double CalculateTaxValue()
    {
        var tax = Tax.GetTax();
        var taxPercentage = new FormattedDouble(tax.TaxValue / 100.0);
        return new FormattedDouble(Price * taxPercentage.Number).Number;
    }

    public double CalculateUniversalDiscountValue(double price)
    {
        var universalDiscount = RelativeDiscount.GetDiscountInstance();
        var discount = new FormattedDouble(universalDiscount.DiscountValue / 100.0).Number;
        return new FormattedDouble(price * discount).Number;
    }

    public double CalculateUpcDiscountValue(double price)
    {
        var upcDiscount = new FormattedDouble(_specialDiscount.DiscountValue / 100.0).Number;
        return new FormattedDouble(price * upcDiscount).Number;
    }

    private double CalculateAdditiveDiscounts()
    {
        return new FormattedDouble(CalculateUniversalDiscountValue(Price) + CalculateUpcDiscountValue(Price)).Number;
    }

    private double CalculateMultiplicativeDiscount()
    {
        var universalDiscount = CalculateUniversalDiscountValue(Price);
        var remaining = Price - universalDiscount;
        return new FormattedDouble(universalDiscount + CalculateUpcDiscountValue(remaining)).Number;
    }


    public double CalculateFinalPrice()
    {
        var combiningDiscount = RelativeDiscount.GetDiscountInstance().CombiningDiscount;
        switch (combiningDiscount)
        {
            case CombinedDiscount.Additive:
                return GetFinalPrice(CalculateAdditiveDiscounts);
            case CombinedDiscount.Multiplicative:
                return GetFinalPrice(CalculateMultiplicativeDiscount);
            default:
                return GetFinalPrice(CalculateAdditiveDiscounts);
        }
    }

    private double GetFinalPrice(DiscountAmount discountAmount)
    {
        return new FormattedDouble(Price + CalculateTaxValue() - discountAmount() + CalculateExpenses()).Number;
    }

    public double CalculateDiscountsValue()
    {
        var combiningDiscount = RelativeDiscount.GetDiscountInstance().CombiningDiscount;
        switch (combiningDiscount)
        {
            case CombinedDiscount.Additive:
                return CalculateAdditiveDiscounts();
            case CombinedDiscount.Multiplicative:
                return CalculateMultiplicativeDiscount();
            default:
                return GetFinalPrice(CalculateTaxValue);
        }
    }


    public static bool ValidEntry(string name, string upc, string price)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        int UPC;
        if (!int.TryParse(upc, out UPC)) return false;
        double p;
        if (!double.TryParse(price, out p)) return false;
        return true;
    }

    public void SetSpecialDiscount(string value)
    {
        _specialDiscount.SetDiscount(value);
    }

    public void AddExpense(IExpenses expense)
    {
        _expenses.Add(expense);
    }

    private double CalculateExpenses()
    {
        var cost = new FormattedDouble(0d).Number;
        foreach (var expense in _expenses) cost += GetExpenseAmount(expense);
        return new FormattedDouble(cost).Number;
    }

    private double GetExpenseAmount(IExpenses expense)
    {
        var cost = new FormattedDouble(0d).Number;
        if (expense.Type == PriceType.AbsoluteValue) cost = new FormattedDouble(expense.Amount).Number;
        else
            cost = new FormattedDouble(Price * expense.Amount).Number;
        return new FormattedDouble(cost).Number;
    }

    public string GetExpenseInfo()
    {
        var info = string.Empty;
        foreach (var expense in _expenses) info += $"{expense.Description} = ${GetExpenseAmount(expense)}\n";
        return info;
    }
}