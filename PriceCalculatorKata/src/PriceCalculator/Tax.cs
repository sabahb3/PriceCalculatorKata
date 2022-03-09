namespace PriceCalculator;

public class Tax : ITax
{
    private static readonly Tax s_taxInstance = new();

    private Tax()
    {
    }

    public static Tax GetTax()
    {
        return s_taxInstance;
    }

    public int TaxValue { get; private set; } = 20;

    public bool IsValidTax(string tax)
    {
        int result;
        if (string.IsNullOrWhiteSpace(tax)) return false;
        if (!int.TryParse(tax, out result)) return false;
        if (result <= 0) return false;
        return true;
    }

    public void SetTax(string newValue)
    {
        if (!IsValidTax(newValue)) return;
        TaxValue = int.Parse(newValue);
    }
}