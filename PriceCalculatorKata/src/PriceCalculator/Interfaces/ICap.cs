using PriceCalculator.Enumerations;

namespace PriceCalculator;

public interface ICap
{
    double Amount { get; }
    PriceType Type { get; }
    public bool ValidDiscount(double price, double discount);

}