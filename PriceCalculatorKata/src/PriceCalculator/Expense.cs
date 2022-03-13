using PriceCalculator.Enumerations;
using PriceCalculator.Structures;

namespace PriceCalculator;

public class Expense : IExpenses
{
    public Expense(string description, double amount, PriceType type)
    {
        Description = description;
        Type = type;
        if (type == PriceType.AbsoluteValue)
        {
            Amount = new FormattedDouble(amount).CalculatedNumber;
        }
        else
        {
            var percentage = amount / 100.0;
            Amount = new FormattedDouble(percentage).CalculatedNumber;
        }
    }

    public string Description { get; set; }
    public double Amount { get; private set; }

    public void SetAmount(double amount)
    {
        if (Type == PriceType.AbsoluteValue)
        {
            Amount = new FormattedDouble(amount).CalculatedNumber;
        }
        else
        {
            var percentage = amount / 100.0;
            Amount = new FormattedDouble(percentage).CalculatedNumber;
        }
    }

    public PriceType Type { get; set; }
}