
using System.Globalization;
using ExtensionMethods;

try
{
    string fileName = GetValidFileNameFromUser();

    List<ValuesTagFields> valuesTagFieldsList = ReadValuesTagDataFromFile(fileName);
    List<VariablesTagFields> variablesTagFields = ReadVariablesTagDataFromFile(fileName);

    var valuesTagFieldsListGroupped = valuesTagFieldsList.GroupBy(d => d.ChannelIndex);

    foreach (var item in valuesTagFieldsListGroupped)
    {
        decimal minValue = item.MinByValue().Value;
        DateTime minTimeStamp = item.MinByTimeStamp().TimeStamp;

        decimal maxValue = item.MaxByValue().Value;
        DateTime maxTimeStamp = item.MaxByTimeStamp().TimeStamp;

        var variableID = variablesTagFields.Where(v => v.ChannelIndex == item.Key).FirstOrDefault()?.VariableID;

        Console.WriteLine($"Variable ID: {variableID}, Min Value: {minValue}, Min TimeStamp: {minTimeStamp}, Max Value: {maxValue}, Max TimeStamp: {maxTimeStamp}");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

static string GetValidFileNameFromUser()
{
    string? fileName;
    do
    {
        Console.WriteLine("Enter the file name:");
        fileName = Console.ReadLine();

        if (string.IsNullOrEmpty(fileName))
        {
            Console.WriteLine("File name can't be empty!");
        }

    } while (string.IsNullOrEmpty(fileName));

    return fileName.Replace(" ", "").ToUpper();
}


static List<ValuesTagFields> ReadValuesTagDataFromFile(string fileName)
{
    var valuesTagFieldsList = new List<ValuesTagFields>();
    var cultureInfo = new CultureInfo("en-US");

    using (StreamReader reader = new StreamReader(fileName))
    {
        string? line;
        bool readingValues = false;

        while ((line = reader.ReadLine()) != null)
        {
            if (line.StartsWith("-VALUES-"))
            {
                readingValues = true;
                continue;
            }

            if (readingValues)
            {
                string[] splittedItems = line.Split(';');

                ValuesTagFields valuesTagFields = new ValuesTagFields
                {
                    ChannelIndex = splittedItems[0].Split(':')[0],
                    Value = Convert.ToDecimal(splittedItems[0].Split(':')[1]),
                    TimeStamp = DateTime.ParseExact(splittedItems[2], "dd.MM.yyyy HH:mm:ss.fff", cultureInfo)
                };

                valuesTagFieldsList.Add(valuesTagFields);
            }
        }
    }

    return valuesTagFieldsList;
}

static List<VariablesTagFields> ReadVariablesTagDataFromFile(string fileName)
{
    var variablesTagFieldsList = new List<VariablesTagFields>();

    using (StreamReader reader = new StreamReader(fileName))
    {
        string? line;
        bool readingValues = false;

        while ((line = reader.ReadLine()) != null)
        {
            if (line.StartsWith("-VARIABLES-"))
            {
                readingValues = true;
                continue;
            }
            if (line.StartsWith("-VALUES-"))
            {
                readingValues = false;
                break;
            }

            if (readingValues)
            {
                string[] splittedItems = line.Split("=");

                VariablesTagFields variablesTagFields = new VariablesTagFields
                {
                    ChannelIndex = splittedItems[0],
                    VariableID = Convert.ToByte(splittedItems[1])
                };

                variablesTagFieldsList.Add(variablesTagFields);
            }
        }
    }

    return variablesTagFieldsList;
}