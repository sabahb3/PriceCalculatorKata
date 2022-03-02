namespace PriceCalculator
{
    public class Tax
    {
        private static readonly Tax s_taxInstance = new Tax();
        private Tax() { }
        public static Tax GetTax()
        {
            return s_taxInstance;
        }
        public int TaxValue { get; private set; } = 20;

        public bool isValidTax(string tax)
        {
            int result;
            if (string.IsNullOrWhiteSpace(tax)) return false;
            if (!int.TryParse(tax, out result)) return false;
            if (result <= 0) return false;
            return true;

        }
        public void setTax(string newValue)
        {
            if (!isValidTax(newValue)) return;
            TaxValue = int.Parse(newValue);
        }
    }
}