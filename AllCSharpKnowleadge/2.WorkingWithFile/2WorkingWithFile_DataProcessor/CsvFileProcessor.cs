using _2WorkingWithFile_DataProcessor.Model;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO.Abstractions;

namespace _2WorkingWithFile_DataProcessor
{
    public class CsvFileProcessor
    {
        private readonly IFileSystem _fileSystem;
        public string InputFilePath { get;}
        public string OutputFilePath { get; }
        public CsvFileProcessor(string input, string output , IFileSystem fileSystem)
        {
            InputFilePath = input;
            OutputFilePath = output;
            _fileSystem = fileSystem;

        }

        public CsvFileProcessor(string input, string output) : this(input, output, new FileSystem())
        {
            
        }
        public void Process()
        {
            #region using FileSystem
            //using StreamReader inputReader = File.OpenText(InputFilePath);

            //var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            //{
            //    Comment = '@',
            //    AllowComments = true,
            //    TrimOptions = TrimOptions.Trim,
            //    //below using to read csv file
            //    //IgnoreBlankLines = true, // true value is default,
            //    //HasHeaderRecord = true, // true value is default,
            //    //Delimiter = ",", // "," is default value 
            //    //HeaderValidated = null
            //};
            //using CsvReader csvReader = new CsvReader(inputReader, csvConfiguration);

            #region Writing CSV
            //csvReader.Context.RegisterClassMap<ProcessedOrderMap>();

            //IEnumerable<ProcessedOrder> records = csvReader.GetRecords<ProcessedOrder>();

            //using StreamWriter output = File.CreateText(OutputFilePath);
            //using var csvWriter = new CsvWriter(output, CultureInfo.InvariantCulture);

            //csvWriter.WriteRecords(records);
            #endregion

            #region Reading CSV
            #region Configuring Custom Class Mapping
            //csvReader.Context.RegisterClassMap<ProcessedOrderMap>();

            //IEnumerable<ProcessedOrder> records = csvReader.GetRecords<ProcessedOrder>();

            //foreach (ProcessedOrder processedOrder in records)
            //{
            //    Console.WriteLine($"Order Number: {processedOrder.OrderNumber}");
            //    Console.WriteLine($"Customer : {processedOrder.Customer}");
            //    Console.WriteLine($"Amount: {processedOrder.Amount}");
            //}
            #endregion

            #region base on the csv file with not pre-defined
            //IEnumerable<dynamic> records = csvReader.GetRecords<dynamic>();

            //foreach (var record in records)
            //{
            //    Console.WriteLine(record.OrderNumber);
            //    Console.WriteLine(record.CustomerNumber);
            //    Console.WriteLine(record.Description);
            //    Console.WriteLine(record.Quantity);
            //}
            #endregion

            #region reading csv data with strongly typed way with model class
            //IEnumerable<Order> records = csvReader.GetRecords<Order>();

            //foreach (var order in records)
            //{
            //    Console.WriteLine($"Order Number: {order.OrderNumber}");
            //    Console.WriteLine($"Customer Number: {order.CustomerNumber}");
            //    Console.WriteLine($"Description: {order.Description}");
            //    Console.WriteLine($"Quantity: {order.Quantity}");
            //}
            #endregion
            #endregion
            #endregion

            #region using IFileSystem.Abstractions for testing
            using var inputReader = _fileSystem.File.OpenText(InputFilePath);

            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Comment = '@',
                AllowComments = true,
                TrimOptions = TrimOptions.Trim,
            };
            using CsvReader csvReader = new CsvReader(inputReader, csvConfiguration);

            csvReader.Context.RegisterClassMap<ProcessedOrderMap>();

            IEnumerable<ProcessedOrder> records = csvReader.GetRecords<ProcessedOrder>();

            using var output = _fileSystem.File.CreateText(OutputFilePath);
            using var csvWriter = new CsvWriter(output, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(records);
            #endregion

        }
    }
}
