using PriceCalculator.Enumerations;

namespace PriceCalculator;

public class RelativeDiscount : Discount, IDiscount, IRelativeDiscount
{
    private static readonly RelativeDiscount s_discountInstance = new();
    public CombinedDiscount CombiningDiscount { get; set; }

    private RelativeDiscount()
    {
    }

    public static RelativeDiscount GetDiscountInstance()
    {
        return s_discountInstance;
    }
}