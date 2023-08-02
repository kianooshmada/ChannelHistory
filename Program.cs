
using System.Globalization;
using ExtensionMethods;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            string fileName = GetValidFileNameFromUser();

            List<ValuesTagFields> valuesTagFieldsList = ReadValuesTagDataFromFile(fileName);
            List<VariablesTagFields> variablesTagFieldsList = ReadVariablesTagDataFromFile(fileName);
            List<OutputFields> outputFieldsList = new List<OutputFields>();

            var groupByValuesTagFieldsList = valuesTagFieldsList.GroupBy(d => d.ChannelIndex);

            foreach (var item in groupByValuesTagFieldsList)
            {
                decimal minValue = item.MinByValue().Value;
                DateTime minTimeStamp = item.MinByTimeStamp().TimeStamp;

                decimal maxValue = item.MaxByValue().Value;
                DateTime maxTimeStamp = item.MaxByTimeStamp().TimeStamp;

                byte? variableID = variablesTagFieldsList.Where(v => v.ChannelIndex == item.Key).FirstOrDefault()?.VariableID;

                outputFieldsList.Add(new OutputFields
                {
                    VariableID = variableID ?? 0,
                    MinValue = minValue,
                    MinTimeStamp = minTimeStamp,
                    MaxValue = maxValue,
                    MaxTimeStamp = maxTimeStamp
                });
            }

            WriteInNewTextFile(outputFieldsList);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message, Console.ForegroundColor = ConsoleColor.Red);
        }
    }
    private static string GetValidFileNameFromUser()
    {
        string? fileName;
        do
        {
            Console.WriteLine("Enter the file name:");
            fileName = Console.ReadLine();

            if (string.IsNullOrEmpty(fileName))

                Console.WriteLine("File name can't be empty!", Console.ForegroundColor = ConsoleColor.Yellow);


        } while (string.IsNullOrEmpty(fileName));

        return fileName.Replace(" ", "").ToUpper();
    }


    private static List<ValuesTagFields> ReadValuesTagDataFromFile(string fileName)
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

    private static List<VariablesTagFields> ReadVariablesTagDataFromFile(string fileName)
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

    private static void WriteInNewTextFile(List<OutputFields> outputFieldsList)
    {
        ConsoleKey response;
        do
        {
            Console.Write("Do you want to save the new file in Desktop? [y/n] ");
            response = Console.ReadKey(false).Key;
            if (response != ConsoleKey.Enter)
                Console.WriteLine();

        } while (response != ConsoleKey.Y && response != ConsoleKey.N);

        string filePath = string.Empty;

        if (response == ConsoleKey.N)
        {
            do
            {
                Console.WriteLine("Enter the path for saving the file:");
                filePath = Console.ReadLine()!;
            } while (string.IsNullOrEmpty(filePath));
        }
        else
            filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        using (StreamWriter outputFile = new StreamWriter(Path.Combine(filePath!, "OutputFile.txt")))
        {
            foreach (var outputFields in outputFieldsList)
                outputFile.WriteLine($" Variable ID: {outputFields.VariableID}, Min Value: {outputFields.MinValue}, Min TimeStamp: {outputFields.MinTimeStamp}, Max Value: {outputFields.MaxValue}, Max TimeStamp: {outputFields.MaxTimeStamp}");

            Console.WriteLine("The output file created successfully in the path {0}.", filePath, Console.ForegroundColor = ConsoleColor.Green);
        }
    }
}