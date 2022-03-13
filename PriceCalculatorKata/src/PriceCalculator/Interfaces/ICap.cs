using PriceCalculator.Enumerations;

namespace PriceCalculator;

public interface ICap
{
    double Amount { get; }
    PriceType Type { get; }
}