using System;
namespace PriceCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Product product=new Product("The Little Prince",12345,20.215);
            string displayed=$"Name = {product.Name}, Price= {string.Format("{0:0.##}",product.Price)}, UPC: {product.UPC}";
            Console.WriteLine(displayed);
            product.Name="The Prince";
            Console.WriteLine(product.Name);
        }

    }
}