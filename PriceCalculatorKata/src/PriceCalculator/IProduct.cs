namespace PriceCalculator
{
    public interface IProduct
    {
        public string Name { get; set; }
        public int UPC { get; set; }
        public double Price { get; set; }

        public string DisplayProductDescription();
         public string DisplayProductDescription(Enumerations.ProductDescription type);
        public double CalculateTaxValue();

    }
}