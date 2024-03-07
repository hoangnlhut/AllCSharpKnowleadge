using _2WorkingWithFile_DataProcessor;
using ApprovalTests;
using ApprovalTests.Reporters;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2WorkingWithFile.Tests
{
    public class CsvFileProcessorShould
    {
        [Fact]
        public void TestWritingCsvFile()
        {
            var inputFile = @"E:\LEARNING\SELF_TRAINING_FOLDER\AllCSharpKnowledge\AllCSharpKnowleadge\AllCSharpKnowleadge\2.WorkingWithFile\2WorkingWithFile_DataProcessor\psdata\in\myFile.csv";
            var outputPath = @"E:\LEARNING\SELF_TRAINING_FOLDER\AllCSharpKnowledge\AllCSharpKnowleadge\AllCSharpKnowleadge\2.WorkingWithFile\2WorkingWithFile_DataProcessor\psdata\out";
            var outputFile = @"E:\LEARNING\SELF_TRAINING_FOLDER\AllCSharpKnowledge\AllCSharpKnowleadge\AllCSharpKnowleadge\2.WorkingWithFile\2WorkingWithFile_DataProcessor\psdata\out\myFileOuput.csv";

            // Create a mock input File
            var csvLines = new StringBuilder();
            csvLines.AppendLine("OrderNumber,CustomerNumber,Description,Quantity");
            csvLines.AppendLine("42, 100001, Shirt, II");
            csvLines.AppendLine("43, 100001, Shorts, I");
            csvLines.AppendLine("@ This is a comment");
            csvLines.AppendLine("");
            csvLines.AppendLine("44, 300003, Cap, V");

            var mockInputFile = new MockFileData(csvLines.ToString());

            // setup mock file system starting state
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(inputFile, mockInputFile);
            mockFileSystem.AddDirectory(outputPath);

            // Create TextFileProcessor with mock file system
            var sut = new CsvFileProcessor(inputFile, outputFile, mockFileSystem);

            // Process test file
            sut.Process();

            // Check mock file system for outut file
            Assert.True(mockFileSystem.FileExists(outputFile));

            // Check content of output file in mock file system
            var processedFile = mockFileSystem.GetFile(outputFile);

            var data = processedFile.TextContents.Split(Environment.NewLine);

            Assert.Equal("OrderNumber,Customer,Amount", data[0]);
            Assert.Equal("42,100001,2", data[1]);
            Assert.Equal("43,100001,1", data[2]);
            Assert.Equal("44,300003,5", data[3]);
        }

        [Fact]
        // this attribute tells Approval Tests how to report on failing tests.
        [UseReporter(typeof(DiffReporter))]
        public void OutputProcessedOrderCsvData_ApprovalTest()
        {
            var inputFile = @"E:\LEARNING\SELF_TRAINING_FOLDER\AllCSharpKnowledge\AllCSharpKnowleadge\AllCSharpKnowleadge\2.WorkingWithFile\2WorkingWithFile_DataProcessor\psdata\in\myFile.csv";
            var outputPath = @"E:\LEARNING\SELF_TRAINING_FOLDER\AllCSharpKnowledge\AllCSharpKnowleadge\AllCSharpKnowleadge\2.WorkingWithFile\2WorkingWithFile_DataProcessor\psdata\out";
            var outputFile = @"E:\LEARNING\SELF_TRAINING_FOLDER\AllCSharpKnowledge\AllCSharpKnowleadge\AllCSharpKnowleadge\2.WorkingWithFile\2WorkingWithFile_DataProcessor\psdata\out\myFileOuput.csv";

            // Create a mock input File
            var csvLines = new StringBuilder();
            csvLines.AppendLine("OrderNumber,CustomerNumber,Description,Quantity");
            csvLines.AppendLine("42, 100001, Shirt, II");
            csvLines.AppendLine("43, 100001, Shorts, I");
            csvLines.AppendLine("@ This is a comment");
            csvLines.AppendLine("");
            csvLines.AppendLine("44, 300003, Cap, V");

            var mockInputFile = new MockFileData(csvLines.ToString());

            // setup mock file system starting state
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(inputFile, mockInputFile);
            mockFileSystem.AddDirectory(outputPath);

            // Create TextFileProcessor with mock file system
            var sut = new CsvFileProcessor(inputFile, outputFile, mockFileSystem);

            // Process test file
            sut.Process();

            // Check mock file system for outut file
            Assert.True(mockFileSystem.FileExists(outputFile));

            // Check content of output file in mock file system
            var processedFile = mockFileSystem.GetFile(outputFile);


            Approvals.Verify(processedFile.TextContents);
        }
    }
}
