using PriceCalculator.Enumerations;

namespace PriceCalculator;

public class Store
{
    private List<IProduct> _products;
    private ITax _tax;
    private IRelativeDiscount _discount;

    public Store(ITax tax, IRelativeDiscount discount, List<IProduct> products)
    {
        _tax = tax;
        _discount = discount;
        _products = products;
    }

    public bool AddProduct(Product product)
    {
        if (_products.Contains(product)) return false;
        _products.Add(product);
        return true;
    }

    public void SetUniversalDiscount(string discountAmount)
    {
        _discount.SetDiscount(discountAmount);
    }

    public void SetTax(string taxAmount)
    {
        _tax.SetTax(taxAmount);
    }

    public void RemoveProduct(int upc)
    {
        foreach (var product in _products.Where(product => product.UPC == upc))
            _products.Remove(product);
    }

    public List<IProduct> GetProducts()
    {
        return _products;
    }

    public void SetSpecialDiscount(int upc, string discount)
    {
        foreach (var product in _products.Where(product => product.UPC == upc))
            product.SetSpecialDiscount(discount);
    }

    public void SetExpenseForProduct(int upc, IExpenses expense)
    {
        foreach (var product in _products.Where(product => product.UPC == upc))
            product.AddExpense(expense);
    }

    public void SetCombiningDiscountsWay(CombinedDiscount combinedDiscount)
    {
        _discount.CombiningDiscount = combinedDiscount;
    }
}