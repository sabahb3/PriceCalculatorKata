namespace PriceCalculator.Structures;

public struct FormattedDouble
{
    private readonly double _number;

    public FormattedDouble(double num)
    {
        _number = num;
    }

    public double Number => Math.Round(_number, 2);
}