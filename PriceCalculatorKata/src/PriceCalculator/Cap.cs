using PriceCalculator.Enumerations;
using PriceCalculator.Structures;

namespace PriceCalculator;

public class Cap : ICap
{
    private static readonly Cap s_cap = new();

    public double Amount { get; private set; }
    public PriceType Type { get; private set; }

    public void SetAmount(double amount)
    {
        if (s_cap.Type == PriceType.AbsoluteValue)
        {
            s_cap.Amount = new FormattedDouble(amount).Number;
        }
        else
        {
            var percentage = amount / 100.0;
            s_cap.Amount = new FormattedDouble(percentage).Number;
        }
    }

    public static void SetType(PriceType type)
    {
        s_cap.Type = type;
        s_cap.SetAmount(s_cap.Amount);
    }

    private Cap()
    {
    }

    public static Cap GetCapInstance()
    {
        return s_cap;
    }

    public bool ValidDiscount(double price, double discount)
    {
        return CapAmount(price) >= discount;
    }

    public double CapAmount(double price)
    {
        if (s_cap.Type == PriceType.AbsoluteValue)
        {
            return s_cap.Amount;
        }
        else
        {
            var amount = new FormattedDouble(price * s_cap.Amount).Number;
            return amount;
        }
    }
}