namespace PriceCalculator;

public class RelativeDiscount : Discount, IDiscount
{
    private static readonly RelativeDiscount s_discountInstance = new();

    private RelativeDiscount()
    {
    }

    public static RelativeDiscount GetDiscountInstance()
    {
        return s_discountInstance;
    }
}