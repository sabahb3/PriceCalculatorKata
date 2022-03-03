namespace PriceCalculator
{
    public interface ITax
    {
        int TaxValue { get; }
        bool isValidTax(string tax);
        void setTax(string newValue);
    }
}