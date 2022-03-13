using System.Text.RegularExpressions;

namespace PriceCalculator.Structures;

public struct Currency
{
    public string? _Currency { get; private set; }

    public Currency(string currency) : this()
    {
        SetCurrency(currency);
    }
    public void SetCurrency(string currency)
    {
        if (currency.Length != 3 && Regex.IsMatch(currency,@"[A-Z a-z]+$"))
            _Currency = currency.ToUpper();
        else
            _Currency = "USD";
    }
}