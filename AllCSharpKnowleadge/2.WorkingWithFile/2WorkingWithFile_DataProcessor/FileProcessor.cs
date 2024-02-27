using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2WorkingWithFile_DataProcessor
{
    public class FileProcessor
    {
        private const string BackupDirectoryName = "backup";
        private const string InprogressDirectoryName = "processing";
        private const string CompletedDirectoryName = "complete";
        public string InputFilePath { get; set; }
        public FileProcessor(string FilePath)
        {
            InputFilePath = FilePath;
        }

        public void Process() 
        {
            Console.WriteLine($"Begin process of {InputFilePath}");

            #region check if file exist
            if (!File.Exists(InputFilePath))
            {
                Console.WriteLine($"ERROR: file {InputFilePath} does not exist.");
                return;
            }
            #endregion

            #region Get parent directory of path
            string? rootDirectoryPath = new DirectoryInfo(InputFilePath).Parent?.Parent?.FullName;
            if ( rootDirectoryPath == null ) 
            {
                throw new InvalidOperationException($"Cannot determine root directory path.");
            }
            Console.WriteLine($"Root data path is {rootDirectoryPath ?? "unknown directory path"}");
            #endregion

            #region Check if backup dir exists
            string backupDirectoryPath = Path.Combine(rootDirectoryPath, BackupDirectoryName);
            if (!Directory.Exists(backupDirectoryPath))
            {
                Console.WriteLine($"Creating {backupDirectoryPath}");
                Directory.CreateDirectory(backupDirectoryPath);
            }

            //copy file to backup dir
            string inputFileName = Path.GetFileName(InputFilePath);
            string backupFilePath = Path.Combine(backupDirectoryPath, inputFileName);
            Console.WriteLine($"Copying {InputFilePath} to {backupFilePath}");
            File.Copy(InputFilePath, backupFilePath, true);
            #endregion

            #region Scenario 2: create new directory named processing , move file from in directory to new processing directory, check if the file is exist in processing directory, we will write to console the error

            Directory.CreateDirectory(Path.Combine(rootDirectoryPath, InprogressDirectoryName));
            string processingFilePath = Path.Combine(rootDirectoryPath, InprogressDirectoryName, inputFileName);

            if (File.Exists(processingFilePath))
            {
                Console.WriteLine($"File from {processingFilePath} was existedddd");
                //return;
            }
            else
            {
                Console.WriteLine($"Moving {InputFilePath} to {processingFilePath}");
                File.Move(InputFilePath, processingFilePath, true);
            }
            #endregion

            #region getting file extention from a file name
            string extention = Path.GetExtension(InputFilePath);
            switch (extention)
            {
                case ".txt": 
                    ProcessTextFile(processingFilePath);
                    break;
                default:
                    Console.WriteLine("Is not properly extention");
                    break;
            }

            #endregion

            #region Move file after processing is complete
            string completedDirectoryPath = Path.Combine(rootDirectoryPath, CompletedDirectoryName);
            if (!Directory.Exists(completedDirectoryPath))
            {
                Directory.CreateDirectory(completedDirectoryPath);
            }

            string fileNameWithCompletedExtention = Path.ChangeExtension(inputFileName, ".complete");
            string completedFileName = $"{Guid.NewGuid()}_{fileNameWithCompletedExtention}";
            string completedFilePath = Path.Combine(completedDirectoryPath, completedFileName);

            Console.WriteLine($"Moving {processingFilePath} to {completedFilePath}");
            File.Move (processingFilePath, completedFilePath, true);
            #endregion

            #region Deleting Directory
            string? inProgressDirectoryPath = Path.GetDirectoryName(processingFilePath);
            if (Directory.Exists(inProgressDirectoryPath))
            {
                //second parameter is true mean it will delete sub-directory first and then direct parent directory
                Directory.Delete(inProgressDirectoryPath, true);
            }


            #endregion

        }

        private void ProcessTextFile(string processingFilePath)
        {
            Console.WriteLine($"Processing Text file {processingFilePath}");

            //Read in and process
        }
    }
}
