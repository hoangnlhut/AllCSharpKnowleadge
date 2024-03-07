using Xunit;
using System.IO.Abstractions.TestingHelpers;
using _2WorkingWithFile_DataProcessor;

namespace _2WorkingWithFile.Tests
{
    public class TextFileProcessorShould
    {
        [Fact]
        public void MakeSecondLineUpperCase()
        {
            var inputFile = @"E:\LEARNING\SELF_TRAINING_FOLDER\AllCSharpKnowledge\AllCSharpKnowleadge\AllCSharpKnowleadge\2.WorkingWithFile\2WorkingWithFile_DataProcessor\psdata\in\mytext.txt";
            var outputPath = @"E:\LEARNING\SELF_TRAINING_FOLDER\AllCSharpKnowledge\AllCSharpKnowleadge\AllCSharpKnowleadge\2.WorkingWithFile\2WorkingWithFile_DataProcessor\psdata\out";
            var outputFile = @"E:\LEARNING\SELF_TRAINING_FOLDER\AllCSharpKnowledge\AllCSharpKnowleadge\AllCSharpKnowleadge\2.WorkingWithFile\2WorkingWithFile_DataProcessor\psdata\out\mytext.txt";

            // Create a mock input File
            var mockInputFile = new MockFileData("Line 1\nLine 2\nLine 3");

            // setup mock file system starting state
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(inputFile, mockInputFile);
            mockFileSystem.AddDirectory(outputPath);

            // Create TextFileProcessor with mock file system
            var sut = new TextFileProcessor(inputFile, outputFile, mockFileSystem );

            // Process test file
            sut.Process();

            // Check mock file system for outut file
            Assert.True(mockFileSystem.FileExists(outputFile));

            // Check content of output file in mock file system
            var processedFile = mockFileSystem.GetFile(outputFile);

            string[] lines = processedFile.TextContents.Split(Environment.NewLine);

            Assert.Equal("Line 1", lines[0]);
            Assert.Equal("LINE 2", lines[1]);
            Assert.Equal("Line 3", lines[2]);
        }
    }
}