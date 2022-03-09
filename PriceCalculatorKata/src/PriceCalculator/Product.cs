namespace PriceCalculator;

public class Product : IProduct
{
    private ITax _tax;
    private IDiscount _relativeDiscount;

    public Product(string name, int upc, double price)
    {
        Name = name ?? " ";
        UPC = upc;
        Price = price;
        _tax = Tax.GetTax();
        _relativeDiscount = RelativeDiscount.GetDiscountInstance();
    }

    public string Name { get; set; } = string.Empty;
    public int UPC { get; set; }
    public double Price { get; set; }

    public string DisplayProductDescription()
    {
        var productDescription = string.Empty;
        productDescription =
            $"\nBook with name = \"{Name}\", UPC = {UPC}, price ={string.Format("{0:0.##}", Price)}.\n";
        productDescription += $"Product price reported as ${string.Format("{0:0.##}", Price)} before tax";
        productDescription += $" and ${string.Format("{0:0.##}", CalculatePriceAfterTax())} after {_tax.TaxValue}% tax";
        return productDescription;
    }

    public string DisplayProductDescription(Enumerations.ProductDescription type)
    {
        var productDescription = string.Empty;
        if (type == Enumerations.ProductDescription.RelativeDiscount)
            productDescription = DisplayProductDescriptionWithRelativeDiscount();
        else productDescription = DisplayProductDescription();
        return productDescription;
    }

    public string DisplayProductDescriptionWithRelativeDiscount()
    {
        var productDescription = string.Empty;
        productDescription =
            $"\nBook with name = \"{Name}\", UPC = {UPC}, price ={string.Format("{0:0.##}", Price)}.\n";
        productDescription += $"Tax= {_tax.TaxValue}%, discount={_relativeDiscount.DiscountValue}%";
        productDescription +=
            $"Tax amount = ${string.Format("{0:0.##}", CalculateTaxValue())}, Discount amount = ${string.Format("{0:0.##}", CalculateDiscountValue())}\n";
        productDescription +=
            $" Price before ${string.Format("{0:0.##}", Price)}, price after = ${string.Format("{0:0.##}", CalculatePriceAfterDiscount())}";
        return productDescription;
    }

    public double CalculatePriceAfterTax()
    {
        return Price + CalculateTaxValue();
    }

    public double CalculateTaxValue()
    {
        return Price * (_tax.TaxValue / 100.0);
    }

    public double CalculateDiscountValue()
    {
        return Price * (_relativeDiscount.DiscountValue / 100.0);
    }

    public double CalculatePriceAfterDiscount()
    {
        return CalculatePriceAfterTax() - CalculateDiscountValue();
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
}