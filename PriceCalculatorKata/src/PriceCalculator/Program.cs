using System;
using PriceCalculator.Enumerations;

namespace PriceCalculator;

public class Program
{
    private static Store _store = new(Tax.GetTax(), RelativeDiscount.GetDiscountInstance(), new List<IProduct>());

    public static string DisplayMenu()
    {
        var message = string.Empty;
        message += "\n\nWelcome to Price Calculator Kata \n";
        message += "press 1 : Change Tax value \n";
        message += "press 2 : Add new product \n";
        message += "press 3 : Change Universal Discount value\n";
        message += "press 4 : Display a Report\n";
        message += "press 5 : Set discount for a specific product\n";
        message += "press 6 : Set precedence of universal discount \n";
        message += "press 7 : Set precedence discount for a specific product\n";
        message += "press 8: Add Expense for a specific product";
        message += "press # : to exit";

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
                ChangeDiscount();
                break;

            case "4":
                Reporting();
                break;

            case "5":
                GetDiscountInfo();
                break;

            case "6":
                SetUniversalDiscountPrecedence();
                break;

            case "7":
                SetUpcDiscountPrecedence();
                break;
            
            case "8":
                GetExpeenseInfo();
                break;

            default:
                break;
        }
    }

    private static void ChangeTax()
    {
        Console.WriteLine("Enter the new Tax");
        var newValue = Console.ReadLine() ?? string.Empty;
        _store.SetTax(newValue);
    }

    private static void GetProductInfo()
    {
        Console.WriteLine("Enter product's name");
        var productName = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter product's UPC");
        var productUPC = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter product's price");
        var productPrice = Console.ReadLine() ?? string.Empty;

        AddProduct(productName, productUPC, productPrice);
    }

    private static void AddProduct(string name, string upc, string price)
    {
        if (!Product.ValidEntry(name, upc, price)) return;
        var p = new Product(name, int.Parse(upc), double.Parse(price), new Discount(),new List<IExpenses>());
        _store.AddProduct(p);
    }

    private static void ChangeDiscount()
    {
        Console.WriteLine("Enter the new Discount");
        var newValue = Console.ReadLine() ?? string.Empty;
        _store.SetUniversalDiscount(newValue);
    }

    private static void Reporting()
    {
        var report = new Report(_store.GetProducts());
        Console.WriteLine(report.Reporting());
    }

    private static void GetDiscountInfo()
    {
        Console.WriteLine("Enter product's UPC");
        var productUPC = Console.ReadLine() ?? string.Empty;
        var parsed = int.TryParse(productUPC, out var upc);
        if (!parsed)
        {
            Console.WriteLine("Should be integer number ");
            return;
        }

        Console.WriteLine("Enter Discount's Amount");
        var productDiscount = Console.ReadLine() ?? string.Empty;
        _store.SetSpecialDiscount(upc, productDiscount);
    }

    private static void SetUniversalDiscountPrecedence()
    {
        Console.WriteLine(
            "Enter a if you want to calculate the tax regardless of the discount amount otherwise enter b");
        var input = Console.ReadLine() ?? string.Empty;

        switch (input)
        {
            case "a":
                _store.SetUniversalDiscountPrecedence(Precedence.AfterTax);
                break;
            case "b":
                _store.SetUniversalDiscountPrecedence(Precedence.BeforeTax);
                break;
            default:
                break;
        }
    }

    private static void SetUpcDiscountPrecedence()
    {
        Console.WriteLine("Enter UPC of the product");
        var productUPC = Console.ReadLine() ?? string.Empty;
        var parsed = int.TryParse(productUPC, out var upc);
        if (!parsed) return;
        Console.WriteLine(
            "Enter a if you want to calculate the tax regardless of the discount amount otherwise enter b");
        var input = Console.ReadLine() ?? string.Empty;
        switch (input)
        {
            case "a":
                _store.SetUpcDiscountPrecedence(upc, Precedence.AfterTax);
                break;
            case "b":
                _store.SetUpcDiscountPrecedence(upc, Precedence.BeforeTax);
                break;
            default:
                break;
        }
    }

    private static void GetExpeenseInfo()
    {
        Console.WriteLine("Enter product's UPC");
        var productUPC = Console.ReadLine() ?? string.Empty;
        var parsed = int.TryParse(productUPC, out var upc);
        if (!parsed)
        {
            Console.WriteLine("Should be integer number ");
            return;
        }

        Console.WriteLine("Enter Expense's description");
        var expenseDescribtion = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Enter Expense's Amount");
        var expenseAmount = Console.ReadLine() ?? string.Empty;
        double amount;
        var valid = double.TryParse(expenseAmount, out amount);
        if (!valid) return;
        Console.WriteLine("Enter a if the amount is absolute value, p if it is a percentage of price");
        var expenseType = Console.ReadLine() ?? string.Empty;
        if (expenseType != "a" && expenseType != "p") return;
        IExpenses expense;
        if (expenseType == "a")
        {
            expense = new Expense(expenseDescribtion, amount, QuantityType.AbsoluteValue);
        }
        else
        {
            expense=new Expense(expenseDescribtion, amount, QuantityType.Percentage);
        }
        _store.SetExpenseForProduct(upc, expense);
        
    }


    public static void Main(string[] args)
    {
        var input = string.Empty;
        while (true)
        {
            Console.WriteLine(DisplayMenu());
            input = Console.ReadLine() ?? string.Empty;
            if (input.Length != 1) continue;
            if (input == "#") break;
            PerformAction(input);
        }
    }
}