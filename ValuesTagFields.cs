public class ValuesTagFields : VariablesTagFields
{
    private decimal _value;
    private DateTime _timeStamp;

    public decimal Value
    {
        get => _value;
        set => _value = value;
    }

    public DateTime TimeStamp
    {
        get => _timeStamp;
        set => _timeStamp = value;
    }
}