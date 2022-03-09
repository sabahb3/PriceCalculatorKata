namespace PriceCalculator;

public class RelativeDiscount : IDiscount
{
    private static readonly RelativeDiscount s_discountInstance = new();

    private RelativeDiscount()
    {
    }

    public int DiscountValue { get; private set; }

    public static RelativeDiscount GetDiscountInstance()
    {
        return s_discountInstance;
    }

    public void SetDiscount(string discount)
    {
        if (isValidDiscount(discount))
            DiscountValue = int.Parse(discount);
    }

    private bool isValidDiscount(string discount)
    {
        int result;
        if (string.IsNullOrWhiteSpace(discount)) return false;
        if (!int.TryParse(discount, out result)) return false;
        if (result < 0) return false;
        return true;
    }
}