using PriceCalculator.Enumerations;
using PriceCalculator.Structures;

namespace PriceCalculator;

public delegate double TaxAmount();

public delegate double DiscountAmount(double price);

public class Product : IProduct
{
    private IDiscount _specialDiscount;
    private List<IExpenses> _expenses;

    public Product(string name, int upc, double price, IDiscount specialDiscount,List<IExpenses> expenses)
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

    private double CalculateTaxValueAfterUniversalDiscount()
    {
        var price = Price - new FormattedDouble(CalculateUniversalDiscountValue(Price)).Number;
        var tax = Tax.GetTax();
        var taxPercentage = new FormattedDouble(tax.TaxValue / 100.0).Number;
        return new FormattedDouble(price * taxPercentage).Number;
    }

    private double CalculateTaxValueAfterUpcDiscount()
    {
        var price = Price - new FormattedDouble(CalculateUpcDiscountValue(Price)).Number;
        var tax = Tax.GetTax();
        var taxPercentage = new FormattedDouble(tax.TaxValue / 100.0).Number;
        return new FormattedDouble(price * taxPercentage).Number;
    }

    private double CalculateTaxValueAfterAllDiscounts()
    {
        var price = Price - new FormattedDouble(CalculateUniversalDiscountValue(Price)).Number;
        price -= new FormattedDouble(CalculateUpcDiscountValue(Price)).Number;
        var tax = Tax.GetTax();
        var taxPercentage = new FormattedDouble(tax.TaxValue / 100.0).Number;
        return new FormattedDouble(price * taxPercentage).Number;
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

    private double CalculateDiscountsValueOnActualPrice()
    {
        return new FormattedDouble(CalculateUniversalDiscountValue(Price) + CalculateUpcDiscountValue(Price)).Number;
    }

    private double CalculateDiscountsValueOnRemaining(DiscountAmount onActualPrice, DiscountAmount onRemainingPrice)
    {
        var beforeTaxDiscount = onActualPrice(Price);
        var remaining = Price - beforeTaxDiscount;
        return new FormattedDouble(beforeTaxDiscount + onRemainingPrice(remaining)).Number;
    }


    public double CalculateFinalPrice()
    {
        var universalDiscount = RelativeDiscount.GetDiscountInstance();

        if (_specialDiscount.DiscountPrecedence == Precedence.AfterTax &&
            universalDiscount.DiscountPrecedence == Precedence.AfterTax)
            return GetFinalPrice(CalculateTaxValue);

        else if (_specialDiscount.DiscountPrecedence == Precedence.AfterTax &&
                 universalDiscount.DiscountPrecedence == Precedence.BeforeTax)
            return GetFinalPrice(CalculateTaxValueAfterUniversalDiscount);

        else if (_specialDiscount.DiscountPrecedence == Precedence.BeforeTax &&
                 universalDiscount.DiscountPrecedence == Precedence.AfterTax)
            return GetFinalPrice(CalculateTaxValueAfterUpcDiscount);

        else
            return GetFinalPrice(CalculateTaxValueAfterAllDiscounts);
    }

    private double GetFinalPrice(TaxAmount taxAmount)
    {
        return new FormattedDouble(Price + taxAmount() - CalculateDiscountsValue()+ CalculateExpenses()).Number;
    }

    public double CalculateDiscountsValue()
    {
        var universalDiscount = RelativeDiscount.GetDiscountInstance();

        if (_specialDiscount.DiscountPrecedence == Precedence.AfterTax &&
            universalDiscount.DiscountPrecedence == Precedence.BeforeTax)
            return CalculateDiscountsValueOnRemaining(CalculateUniversalDiscountValue, CalculateUpcDiscountValue);

        else if (_specialDiscount.DiscountPrecedence == Precedence.BeforeTax &&
                 universalDiscount.DiscountPrecedence == Precedence.AfterTax)
            return CalculateDiscountsValueOnRemaining(CalculateUpcDiscountValue, CalculateUniversalDiscountValue);
        else
            return CalculateDiscountsValueOnActualPrice();
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

    public void SetDiscountPrecedence(Precedence precedence)
    {
        _specialDiscount.DiscountPrecedence = precedence;
    }

    public bool HasSpecialDiscount()
    {
        return _specialDiscount.DiscountValue > 0;
    }

    public void AddExpense(IExpenses expense)
    {
        _expenses.Add(expense);
    }

    private double CalculateExpenses()
    {
        double cost = new FormattedDouble(0d).Number;
        foreach (var expense in _expenses)
        {
            cost += GetExpenseAmount(expense);
        }
        return cost;
    }

    private double GetExpenseAmount(IExpenses expense)
    {
        double cost = new FormattedDouble(0d).Number;
        if (expense.Type == QuantityType.AbsoluteValue) cost = new FormattedDouble(expense.Amount).Number;
        else
        {
            cost = new FormattedDouble(Price * expense.Amount).Number;
        }
        return cost;

    }
    public string GetExpenseInfo()
    {
        string info = string.Empty;
        foreach (var expense in _expenses)
        {
            info += $"{expense.Description} = ${GetExpenseAmount(expense)}\n";
        }
        return info;
    }
}