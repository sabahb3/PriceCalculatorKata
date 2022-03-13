using System.Text.RegularExpressions;

namespace PriceCalculator.Structures;

public struct Currency
{
    public string? _currency { get; private set; }

    public Currency(string currency) : this()
    {
        SetCurrency(currency);
    }
    public void SetCurrency(string currency)
    {
        if (currency.Length != 3 && Regex.IsMatch(currency,@"[A-Z a-z]+$"))
            _currency = currency.ToUpper();
        else
            _currency = "USD";
    }
}