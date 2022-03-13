using PriceCalculator.Enumerations;
using PriceCalculator.Structures;

namespace PriceCalculator;

public class Expense : IExpenses
{
    public Expense(string description, double amount, QuantityType type)
    {
        Description = description;
        Type = type;
        if (type == QuantityType.AbsoluteValue)
        {
            Amount = new FormattedDouble(amount).Number;
        }
        else
        {
            var percentage = amount / 100.0;
            Amount = new FormattedDouble(percentage).Number;
        }
    }

    public string Description { get; set; }
    public double Amount { get; private set; }

    public void SetAmount(double amount)
    {
        if (Type == QuantityType.AbsoluteValue)
        {
            Amount = new FormattedDouble(amount).Number;
        }
        else
        {
            var percentage = amount / 100.0;
            Amount = new FormattedDouble(percentage).Number;
        }
    }

    public QuantityType Type { get; set; }
}