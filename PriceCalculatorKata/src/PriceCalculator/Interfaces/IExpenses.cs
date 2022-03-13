using PriceCalculator.Enumerations;

namespace PriceCalculator;

public interface IExpenses
{
    string Description { get; set; }
    double Amount { get; }
    PriceType Type { get; set; }
}