using PriceCalculator.Enumerations;

namespace PriceCalculator;

public interface IDiscount
{
    int DiscountValue { get; }
    void SetDiscount(string discount);
}