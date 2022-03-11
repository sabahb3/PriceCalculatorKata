namespace PriceCalculator;

public interface IProduct
{
    public string Name { get; set; }
    public int UPC { get; set; }
    public double Price { get; set; }
    public double CalculateTaxValue();
    public double CalculateDiscountValue();
    public double CalculatePriceAfterDiscount();
    public void SetSpecialDiscount(string value);
}