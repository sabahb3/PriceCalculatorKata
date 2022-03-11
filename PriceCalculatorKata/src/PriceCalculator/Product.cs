namespace PriceCalculator;

public class Product : IProduct
{
    private IDiscount _specialDiscount;

    public Product(string name, int upc, double price, IDiscount specialDiscount)
    {
        Name = name ?? " ";
        UPC = upc;
        Price = price;
        _specialDiscount = specialDiscount;
    }

    public string Name { get; set; } = string.Empty;
    public int UPC { get; set; }
    public double Price { get; set; }


    public double CalculatePriceAfterTax()
    {
        return Price + CalculateTaxValue();
    }

    public double CalculateTaxValue()
    {
        var tax = Tax.GetTax();
        return Price * (tax.TaxValue / 100.0);
    }

    public double CalculateDiscountValue()
    {
        var universalDiscount = RelativeDiscount.GetDiscountInstance();
        return Price * (universalDiscount.DiscountValue / 100.0);
    }

    public double CalculateUpcDiscountValue()
    {
        var upcDiscpunt = _specialDiscount.DiscountValue;
        return Price * (upcDiscpunt/ 100.0);
    }

    public double CalculatePriceAfterDiscount()
    {
        return CalculatePriceAfterTax() - CalculateDiscountValue()-CalculateUpcDiscountValue();
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

    public bool HasSpecialDiscount()
    {
        if (_specialDiscount.DiscountValue > 0) return true;
        return false;
    }
}