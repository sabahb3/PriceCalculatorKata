using PriceCalculator.Enumerations;

namespace PriceCalculator;

public interface IRelativeDiscount : IDiscount
{
    public CombinedDiscount CombiningDiscount { get; set; }
}