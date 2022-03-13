using PriceCalculator.Enumerations;
using PriceCalculator.Structures;

namespace PriceCalculator;

public class Cap:ICap
{
    private static readonly Cap s_cap = new();

    public double Amount { get; private set; }
    public PriceType Type { get; private set; }

    public static void SetAmount(double amount)
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
    }
    private Cap()
    {
    }

    public static Cap GetCapInstance()
    {
        return s_cap;
    }

    public static bool ValidDiscount(double discount)
    {
        return s_cap.Amount >= discount;
    }

   
}