namespace PriceCalculator
{
    public class Product
    {
        public Product(String name, int upc, double price){
            Name=name??" ";
            UPC=upc;
            Price=price;
        }
        public string Name { get; set; }= string.Empty;
        public int UPC { get; set; }
        public double Price { get; set; }        
    }
}