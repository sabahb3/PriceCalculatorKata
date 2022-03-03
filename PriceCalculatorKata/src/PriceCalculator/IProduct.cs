namespace PriceCalculator
{
    public interface IProduct
    {
        public string Name { get; set; }
        public int UPC { get; set; }
        public double Price { get; set; }

        public string DisplayProductDescription();
        public double CalculateTaxValue();

    }
}