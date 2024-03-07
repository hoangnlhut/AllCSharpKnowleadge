using _2WorkingWithFile_DataProcessor;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2WorkingWithFile.Tests
{
    public class BinaryFileProcessorShould
    {
        [Fact]
        public void AddLargestNumber()
        {
            var inputFile = @"E:\LEARNING\SELF_TRAINING_FOLDER\AllCSharpKnowledge\AllCSharpKnowleadge\AllCSharpKnowleadge\2.WorkingWithFile\2WorkingWithFile_DataProcessor\psdata\in\myFile.data";
            var outputPath = @"E:\LEARNING\SELF_TRAINING_FOLDER\AllCSharpKnowledge\AllCSharpKnowleadge\AllCSharpKnowleadge\2.WorkingWithFile\2WorkingWithFile_DataProcessor\psdata\out";
            var outputFile = @"E:\LEARNING\SELF_TRAINING_FOLDER\AllCSharpKnowledge\AllCSharpKnowleadge\AllCSharpKnowleadge\2.WorkingWithFile\2WorkingWithFile_DataProcessor\psdata\out\myFile.data";

            // Create a mock input File
            var mockInputFile = new MockFileData(new byte[] { 0xFF, 0x34, 0x56, 0xD2});

            // setup mock file system starting state
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(inputFile, mockInputFile);
            mockFileSystem.AddDirectory(outputPath);

            // Create TextFileProcessor with mock file system
            var sut = new BinaryFileProcessor(inputFile, outputFile, mockFileSystem);

            // Process test file
            sut.Process();

            // Check mock file system for outut file
            Assert.True(mockFileSystem.FileExists(outputFile));

            // Check content of output file in mock file system
            var processedFile = mockFileSystem.GetFile(outputFile);

            byte[] data = processedFile.Contents;

            Assert.Equal(5, data.Length);
        }
    }
}
