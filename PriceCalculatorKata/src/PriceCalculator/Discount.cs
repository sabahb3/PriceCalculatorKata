using PriceCalculator.Enumerations;

namespace PriceCalculator;

public class Discount : IDiscount
{
    public int DiscountValue { get; private set; }

    public Precedence DiscountPrecedence { get; set; }

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