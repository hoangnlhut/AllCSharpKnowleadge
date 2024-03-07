using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Abstractions;

namespace _2WorkingWithFile_DataProcessor
{
    public class BinaryFileProcessor
    {
        private readonly IFileSystem _fileSystem;
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }
        public BinaryFileProcessor(string inputPath, string outputPath, IFileSystem fileSystem)
        {
            InputFilePath = inputPath;
            OutputFilePath = outputPath;
            _fileSystem = fileSystem;
        }
        public BinaryFileProcessor(string inputPath, string outputPath) : this(inputPath, outputPath, new FileSystem())
        {
            
        }

        public void Process() 
        {
            #region reading and writing binary file to memory
            //byte[] data = File.ReadAllBytes(InputFilePath);

            //byte largest = data.Max();

            //byte[] newData = new byte[data.Length + 1];
            //Array.Copy(data, newData, data.Length);

            //newData[^1] = largest;

            //File.WriteAllBytes(OutputFilePath, newData);
            #endregion

            #region reading and writing binary using Stream 
            //var openToReadFromOption = new FileStreamOptions { Mode = FileMode.Open };
            //using FileStream inputFileStream = File.Open(InputFilePath, openToReadFromOption);

            //using FileStream outputFileStream = File.Create(OutputFilePath);

            //const int endOfStream = -1;
            //int largestByte = 0;

            ////Read next byte (as an int): returns -1 if end of stream
            //int currentByte = inputFileStream.ReadByte();
            //while (currentByte != endOfStream)
            //{
            //    outputFileStream.WriteByte((byte)currentByte);

            //    if (currentByte > largestByte)
            //    {
            //        largestByte = currentByte;
            //    }
            //    currentByte = inputFileStream.ReadByte();
            //}
            //outputFileStream.WriteByte((byte)largestByte);
            #endregion

            #region reading and writing BinaryReader and BinaryWriter
            //var openToReadOption = new FileStreamOptions { Mode = FileMode.Open };
            //using FileStream inputFileStream = File.Open(InputFilePath, openToReadOption);
            ////instead using file stream directly to read individual bytes,
            ////we can use Binaryreader to read bytes and convert them to .NET data types
            //using var binaryReader = new BinaryReader(inputFileStream);

            //using FileStream outputFileStream = File.Create(OutputFilePath);
            //using var binaryWriter = new BinaryWriter(outputFileStream);

            //byte largestByte = 0;
            //while(binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
            //{
            //    var currentByte = binaryReader.ReadByte();
            //    binaryWriter.Write(currentByte);

            //    if (currentByte > largestByte)
            //    {
            //        largestByte = currentByte;
            //    }
            //}
            //binaryWriter.Write(largestByte);
            #endregion

            #region using FileSystem Abstraction for testing
            using var inputFileStream = _fileSystem.File.Open(InputFilePath, FileMode.Open);
            using var binaryReader = new BinaryReader(inputFileStream);

            using var outputFileStream = _fileSystem.File.Create(OutputFilePath);
            using var binaryWriter = new BinaryWriter(outputFileStream);

            byte largestByte = 0;
            while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
            {
                var currentByte = binaryReader.ReadByte();
                binaryWriter.Write(currentByte);

                if (currentByte > largestByte)
                {
                    largestByte = currentByte;
                }
            }
            binaryWriter.Write(largestByte);
            #endregion
        }
    }
}
