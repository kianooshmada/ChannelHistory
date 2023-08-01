public class VariablesTagFields
{
    private string _channelIndex = string.Empty;
    private byte _variableID;

    public string ChannelIndex
    {
        get => _channelIndex;
        set => _channelIndex = value;
    }

    public byte VariableID
    {
        get => _variableID;
        set => _variableID = value;
    }
}