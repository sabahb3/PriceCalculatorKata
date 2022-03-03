namespace PriceCalculator
{
    public class Product : IProduct
    {

        private ITax tax;
        private IDiscount relativeDiscount;
        public Product(String name, int upc, double price)
        {
            Name = name ?? " ";
            UPC = upc;
            Price = price;
            tax = Tax.GetTax();
            relativeDiscount = RelativeDiscount.GetDiscountInstance();
        }

        public string Name { get; set; } = string.Empty;
        public int UPC { get; set; }
        public double Price { get; set; }

        public string DisplayProductDescription()
        {
            string productDescription = string.Empty;
            productDescription = $"\nBook with name = \"{this.Name}\", UPC = {this.UPC}, price ={string.Format("{0:0.##}", this.Price)}.\n";
            productDescription += $"Product price reported as ${string.Format("{0:0.##}", this.Price)} before tax";
            productDescription += $" and ${string.Format("{0:0.##}", this.CalculatePriceAfterTax())} after {tax.TaxValue}% tax";
            return productDescription;
        }
        public string DisplayProductDescription(Enumerations.ProductDescription type)
        {
            string productDescription = string.Empty;
            if (type == Enumerations.ProductDescription.RelativeDiscount) productDescription = DisplayProductDescriptionWithRelativeDiscount();
            else productDescription = DisplayProductDescription();
            return productDescription;
        }
        public string DisplayProductDescriptionWithRelativeDiscount()
        {
            string productDescription = string.Empty;
            productDescription = $"\nBook with name = \"{this.Name}\", UPC = {this.UPC}, price ={string.Format("{0:0.##}", this.Price)}.\n";
            productDescription += $"Tax= {tax.TaxValue}%, discount={relativeDiscount.DiscountValue}%";
            productDescription += $"Tax amount = ${string.Format("{0:0.##}", this.CalculateTaxValue())}, Discount amount = ${string.Format("{0:0.##}", this.CalculateDiscountValue())}\n";
            productDescription += $" Price before ${string.Format("{0:0.##}", this.Price)}, price after = ${string.Format("{0:0.##}", this.CalculatePriceAfterDiscount())}";
            return productDescription;
        }
        public double CalculatePriceAfterTax()
        {

            return this.Price + CalculateTaxValue();
        }
        public double CalculateTaxValue()
        {
            return this.Price * (tax.TaxValue / 100.0);
        }

        public double CalculateDiscountValue()
        {
            return this.Price * (relativeDiscount.DiscountValue / 100.0);
        }

        public double CalculatePriceAfterDiscount()
        {
            return CalculatePriceAfterTax() - CalculateDiscountValue();
        }

        public static bool validEntry(string name, string upc, string price)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            int UPC;
            if (!int.TryParse(upc, out UPC)) return false;
            double p;
            if (!double.TryParse(price, out p)) return false;
            return true;
        }

    }
}