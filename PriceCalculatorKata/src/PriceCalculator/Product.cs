using PriceCalculator.Enumerations;
using PriceCalculator.Structures;

namespace PriceCalculator;

public delegate double DiscountAmount();

public class Product : IProduct
{
    private IDiscount _specialDiscount;
    private List<IExpenses> _expenses;

    public Product(string name, int upc, double price, IDiscount specialDiscount, List<IExpenses> expenses,
        Currency currency)
    {
        Name = name ?? " ";
        UPC = upc;
        SetPrice(price);
        _specialDiscount = specialDiscount;
        _expenses = expenses;
        CurrencyCode = currency;
    }

    public string Name { get; set; } = string.Empty;
    public int UPC { get; set; }
    public double Price { get; private set; }
    public Currency CurrencyCode { get; private set; }


    private void SetPrice(double price)
    {
        var p = new FormattedDouble(price);
        Price = p.CalculatedNumber;
    }

    public double CalculateTaxValue()
    {
        var tax = Tax.GetTax();
        var taxPercentage = new FormattedDouble(tax.TaxValue / 100.0);
        return new FormattedDouble(Price * taxPercentage.CalculatedNumber).CalculatedNumber;
    }

    public double CalculateUniversalDiscountValue(double price)
    {
        var universalDiscount = RelativeDiscount.GetDiscountInstance();
        var discount = new FormattedDouble(universalDiscount.DiscountValue / 100.0).CalculatedNumber;
        return new FormattedDouble(price * discount).CalculatedNumber;
    }

    public double CalculateUpcDiscountValue(double price)
    {
        var upcDiscount = new FormattedDouble(_specialDiscount.DiscountValue / 100.0).CalculatedNumber;
        return new FormattedDouble(price * upcDiscount).CalculatedNumber;
    }

    private double CalculateAdditiveDiscounts()
    {
        return new FormattedDouble(CalculateUniversalDiscountValue(Price) + CalculateUpcDiscountValue(Price))
            .CalculatedNumber;
    }

    private double CalculateMultiplicativeDiscount()
    {
        var universalDiscount = CalculateUniversalDiscountValue(Price);
        var remaining = Price - universalDiscount;
        return new FormattedDouble(universalDiscount + CalculateUpcDiscountValue(remaining)).CalculatedNumber;
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
        var discounts = new FormattedDouble(discountAmount()).CalculatedNumber;
        var valid = Cap.GetCapInstance().ValidDiscount(Price, discounts);
        if (valid)
            return new FormattedDouble(Price + CalculateTaxValue() - discountAmount() + CalculateExpenses())
                .CalculatedNumber;
        else
            return new FormattedDouble(Price + CalculateTaxValue() - Cap.GetCapInstance().CapAmount(Price) +
                                       CalculateExpenses()).CalculatedNumber;
    }

    public double CalculateDiscountsValue()
    {
        var combiningDiscount = RelativeDiscount.GetDiscountInstance().CombiningDiscount;
        var discounts = 0d;
        switch (combiningDiscount)
        {
            case CombinedDiscount.Additive:
                discounts = CalculateAdditiveDiscounts();
                break;
                ;
            case CombinedDiscount.Multiplicative:
                discounts = CalculateMultiplicativeDiscount();
                break;
            default:
                discounts = GetFinalPrice(CalculateTaxValue);
                break;
        }

        if (Cap.GetCapInstance().ValidDiscount(Price, discounts)) return discounts;
        else return Cap.GetCapInstance().CapAmount(Price);
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
        var cost = new FormattedDouble(0d).CalculatedNumber;
        foreach (var expense in _expenses) cost += GetExpenseAmount(expense);
        return new FormattedDouble(cost).CalculatedNumber;
    }

    private double GetExpenseAmount(IExpenses expense)
    {
        var cost = new FormattedDouble(0d).CalculatedNumber;
        if (expense.Type == PriceType.AbsoluteValue) cost = new FormattedDouble(expense.Amount).CalculatedNumber;
        else
            cost = new FormattedDouble(Price * expense.Amount).CalculatedNumber;
        return new FormattedDouble(cost).CalculatedNumber;
    }

    private double GetReportedExpenseAmount(IExpenses expense)
    {
        var cost = new FormattedDouble(0d).CalculatedNumber;
        if (expense.Type == PriceType.AbsoluteValue) cost = new FormattedDouble(expense.Amount).FinalNumber;
        else
            cost = new FormattedDouble(Price * expense.Amount).FinalNumber;
        return new FormattedDouble(cost).FinalNumber;
    }

    public string GetExpenseInfo()
    {
        var info = string.Empty;
        foreach (var expense in _expenses)
            info += $"{expense.Description} = {GetReportedExpenseAmount(expense)} {CurrencyCode.CurrencyCode}\n";
        return info;
    }

    public void SetCurrency(string currency)
    {
        CurrencyCode.SetCurrency(currency);
    }
}