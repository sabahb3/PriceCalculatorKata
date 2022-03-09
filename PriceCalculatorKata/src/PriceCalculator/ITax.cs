namespace PriceCalculator;

public interface ITax
{
    int TaxValue { get; }
    bool IsValidTax(string tax);
    void SetTax(string newValue);
}