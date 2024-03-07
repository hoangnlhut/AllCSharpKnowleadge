using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Abstractions;

namespace _2WorkingWithFile_DataProcessor
{
    public class TextFileProcessor
    {
        private readonly IFileSystem _fileSystem;
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }
        public TextFileProcessor(string inputFilePath, string outputFilePath, IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            _fileSystem = fileSystem;
        }

        // if we have dependency injection , we don't have to create new ctor like this
        public TextFileProcessor(string inputFilePath, string outputFilePath) : this(inputFilePath, outputFilePath, new FileSystem())
        {
        }

        public void Process()
        {
            #region Reading and Writing Entire Files Into Memory
            //using read all text
            //string originalText = File.ReadAllText(InputFilePath);
            //string processedText = originalText.ToUpperInvariant();
            //File.WriteAllText(OutputFilePath, processedText);

            //using read all lines
            //try
            //{
            //    string[] lines = File.ReadAllLines(InputFilePath);
            //    //Assume there is a line 2 in file
            //    lines[1] = lines[1].ToUpperInvariant();
            //    File.WriteAllLines(OutputFilePath, lines);
            //}
            //catch (IOException ex)
            //{
            //    // Log / retry
            //    Console.WriteLine(ex.Message);
            //    throw;
            //}

            //// appending text content
            //File.AppendAllText(OutputFilePath, "hoang append text");

            //// appending all line
            //File.AppendAllLines(OutputFilePath, new string[] {"hoang append line1", "hoang append line2" });
            #endregion

            #region Reading and Writing Data Incrementally Using Streams
            //var inputFileOption = new FileStreamOptions { Mode = FileMode.Open };
            //using var inputFileStream = new FileStream(InputFilePath, inputFileOption);
            //using var inputStreamReader = new StreamReader(inputFileStream);

            //var outputFileOption = new FileStreamOptions { Mode = FileMode.CreateNew, Access = FileAccess.Write };
            //using var outputFileStream = new FileStream(OutputFilePath, outputFileOption);
            //using var outputStreamWriter = new StreamWriter(outputFileStream);

            //while(!inputStreamReader.EndOfStream)
            //{
            //    var eachline = inputStreamReader.ReadLine();
            //    var toUpperEachLine = eachline?.ToUpperInvariant();
            //    outputStreamWriter.WriteLine(toUpperEachLine);
            //}
            #endregion

            #region Simplifiying StreamReader and StreamWriter Creation
            //open text method opens a UTF-8 text file for reading
            //using StreamReader inputStreamReader = File.OpenText(InputFilePath);
            //using var outputStreamWriter = new StreamWriter(OutputFilePath);

            //var currentline = 1;
            //while (!inputStreamReader.EndOfStream)
            //{
            //    var eachline = inputStreamReader.ReadLine();
            //    var toUpperEachLine = eachline;
            //    if (currentline == 2)
            //    {
            //        toUpperEachLine = toUpperEachLine?.ToUpperInvariant();
            //    }
            //    outputStreamWriter.WriteLine(toUpperEachLine);
            //    currentline++;
            //}

            #endregion


            #region using code for IFileSystem for testing
            using var inputStreamReader = _fileSystem.File.OpenText(InputFilePath);
            using var outputStreamWriter =  _fileSystem.File.CreateText(OutputFilePath);

            var currentline = 1;
            while (!inputStreamReader.EndOfStream)
            {
                var eachline = inputStreamReader.ReadLine();
                var toUpperEachLine = eachline;
                if (currentline == 2)
                {
                    toUpperEachLine = toUpperEachLine?.ToUpperInvariant();
                }
                outputStreamWriter.WriteLine(toUpperEachLine);
                currentline++;
            }
            #endregion
        }
    }
}
