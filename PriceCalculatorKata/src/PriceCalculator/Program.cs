using System;
namespace PriceCalculator
{
    public class Program
    {
        private static List<IProduct> products = new List<IProduct>();
        private static ITax tax = Tax.GetTax();

        public static string DisplayMenu()
        {
            String message = string.Empty;
            message += "\n\nWelcome to Price Calculator Kata \n";
            message += "press 1 : Change Tax value \n";
            message += "press 2 : Add new prduct \n";
            message += "press 3: Display product's description \n";

            return message;
        }

        public static void PerformAction(string choice)
        {
            switch (choice)
            {
                case "1":
                    ChangeTax();
                    break;

                case "2":
                    GetProductInfo();
                    break;

                case "3":
                    DisplayProducts();
                    break;

                default:
                    break;
            }
        }

        private static void ChangeTax()
        {
            Console.WriteLine("Enter the new Tax");
            string newValue = Console.ReadLine() ?? string.Empty;
            tax.setTax(newValue);
        }

        private static void GetProductInfo()
        {
            Console.WriteLine("Enter prduct's name");
            string productName = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter product's UPC");
            string productUPC = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter product's price");
            string productPrice = Console.ReadLine() ?? string.Empty;

            AddProduct(productName, productUPC, productPrice);

        }
        private static void AddProduct(string name, string upc, string price)
        {
            if (!Product.validEntry(name, upc, price)) return;
            var p = new Product(name, int.Parse(upc), double.Parse(price));
            products.Add(p);
        }
        private static void DisplayProducts()
        {
            foreach (var product in products) Console.WriteLine(product.DisplayProductDescription());
        }


        public static void Main(string[] args)
        {
            string input = string.Empty;
            while (true)
            {
                Console.WriteLine(DisplayMenu());
                input = Console.ReadLine() ?? string.Empty;
                if (input.Length != 1) continue;
                PerformAction(input);
            }
        }

    }
}