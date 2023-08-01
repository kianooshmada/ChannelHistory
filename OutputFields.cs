public class OutputFields : VariablesTagFields
{
    private decimal _minValue;
    private decimal _maxValue;

    private DateTime _minTimeStamp;
    private DateTime _maxTimeStamp;

    public decimal MinValue
    {
        get => _minValue;
        set => _minValue = value;
    }
    public decimal MaxValue
    {
        get => _maxValue;
        set => _maxValue = value;
    }
    public DateTime MinTimeStamp
    {
        get => _minTimeStamp;
        set => _minTimeStamp = value;
    }
    public DateTime MaxTimeStamp
    {
        get => _maxTimeStamp;
        set => _maxTimeStamp = value;
    }
}