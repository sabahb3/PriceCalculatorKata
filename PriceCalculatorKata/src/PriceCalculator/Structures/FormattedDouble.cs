namespace PriceCalculator.Structures;

public struct FormattedDouble
{
    private readonly double _number;

    public FormattedDouble(double num)
    {
        _number = num;
    }

    public double CalculatedNumber => Math.Round(_number, 4);
    public double FinalNumber => Math.Round(_number, 2);
}