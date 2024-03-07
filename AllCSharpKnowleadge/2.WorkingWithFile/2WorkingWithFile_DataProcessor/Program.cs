using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.Caching;

namespace _2WorkingWithFile_DataProcessor
{
    public class Program
    {
        static ConcurrentDictionary<string, string> Files = new();

        static MemoryCache FilesMemoryCache = MemoryCache.Default;
        static void Main(string[] args)
        {
            #region all process for file and directory
            //Console.WriteLine("Parsing command line options");

            ////Command line validation omiited for brevity
            //var command = args[0];

            //if (command == "--file")
            //{
            //    ProcessFileFromProperty(args);
            //}
            //else if (command == "--dir")
            //{
            //    ProcessPathFromProperty(args);
            //}
            //else
            //{
            //    Console.WriteLine("Invalid command line options");
            //}

            //Console.WriteLine("Press enter to quit");
            //Console.ReadLine();
            #endregion

            #region apply fileSystemWatcher
            ApplyFileSystemWatcher(args);
            #endregion
        }

        private static void ApplyFileSystemWatcher(string[] args)
        {
            string directoryToWatch = args[0];

            if (!Directory.Exists(directoryToWatch))
            {
                Console.WriteLine($"ERROR: {directoryToWatch} does not exist");
                Console.WriteLine("Press enter to quit.");
                Console.ReadLine();
                return;
            }

            #region Processing Existing File
            Console.WriteLine($"Checking {directoryToWatch} for existing files.");
            foreach (var filePath in Directory.EnumerateFiles(directoryToWatch))
            {
                Console.WriteLine($" -- Found {filePath}");
                AddToCache(filePath);
            }
            #endregion

            Console.WriteLine($"Watching directory {directoryToWatch} for changes ");
            using var inputFileWatcher = new FileSystemWatcher(directoryToWatch);
            //using var timer = new Timer(ProcessFiles, null, 0, 1000);

            inputFileWatcher.IncludeSubdirectories = false;
            inputFileWatcher.InternalBufferSize = 32_768; // 32KB
            inputFileWatcher.Filter = "*.*"; // this is the default;
                                             // for copy file to destination folder, overwrite and modify content of file
                                             // for created , renamed or deleted file 
            inputFileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;

            inputFileWatcher.Created += FileCreated;
            inputFileWatcher.Changed += FileChanged;
            inputFileWatcher.Deleted += FileDeleted;
            inputFileWatcher.Renamed += FileRenamed;
            inputFileWatcher.Error += WatcherError;

            inputFileWatcher.EnableRaisingEvents = true;

            Console.WriteLine("Press enter to quit");
            Console.ReadLine();


        }

        private static void WatcherError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"ERROR: file system watching may no longer be active: {e.GetException()}");
        }

        private static void FileRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"* File renamed: from {e.OldName} to {e.Name} - type: {e.ChangeType}");
        }

        private static void FileDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"* File deleted: {e.Name} - type: {e.ChangeType}");
        }

        private static void FileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"* File changed: {e.Name} - type: {e.ChangeType}");
            //ProcessFileFromProperty(e.FullPath);
            //Files.TryAdd(e.FullPath, e.FullPath);\
            AddToCache(e.FullPath);
        }

        private static void FileCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"* File created: {e.Name} - type: {e.ChangeType}");
            //ProcessFileFromProperty(e.FullPath);
            //Files.TryAdd(e.FullPath, e.FullPath);
            AddToCache(e.FullPath);
        }

        private static void AddToCache(string fullPath)
        {
            var item = new CacheItem(fullPath, fullPath);

            var policy = new CacheItemPolicy
            {
                RemovedCallback = ProcessFile,
                SlidingExpiration = TimeSpan.FromSeconds(2),
            };

            FilesMemoryCache.Add(item, policy);
        }

        private static void ProcessFile(CacheEntryRemovedArguments arguments)
        {
           Console.WriteLine($"* Cache item removed: {arguments.CacheItem.Value} because {arguments.RemovedReason}");
            if (arguments.RemovedReason == CacheEntryRemovedReason.Expired)
            {
                ProcessFileFromProperty(arguments.CacheItem.Key);
            }
            else
            {
                Console.WriteLine($"WARNING: {arguments.CacheItem.Key}* was removed unexpectedly for other reason");
            }
        }

        private static void ProcessFiles(object? state)
        {
            foreach (var fileName in Files.Keys)
            {
                if (Files.TryRemove(fileName, out _))
                {
                    ProcessFileFromProperty(fileName);
                }
            }
        }

        private static void ProcessPathFromProperty(string[] args)
        {
            var directoryPath = args[1];
            var fileType = args[2];
            Console.WriteLine($"Directory {directoryPath} selected for {fileType} files");
            ProcessDirectory(directoryPath, fileType);
        }

        private static void ProcessFileFromProperty(string fileFullPath)
        {
            var filePath = fileFullPath;

            //check if path is absolute
            if (!Path.IsPathFullyQualified(filePath))
            {
                Console.WriteLine($"ERROR: path '{filePath}' must be fully qualified.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Single file {filePath} selected");
            ProcessSingleFile(filePath);
        }

        private static void ProcessDirectory(string directoryPath, string fileType)
        {
            //string[] allFiles = Directory.GetFiles(directoryPath); // to get all files 
            switch (fileType)
            {
                case "TEXT":
                    string[] allFiles = Directory.GetFiles(directoryPath, "*.txt"); // get all text file
                    foreach (var file in allFiles)
                    {
                        ProcessSingleFile(file);
                    }
                    break;
                default:
                    Console.WriteLine("Wrong file Type");
                    break;
            }
        }

        private static void ProcessSingleFile(string filePath)
        {
            var fileProcessor = new FileProcessor(filePath);
            fileProcessor.Process();
        }
    }
}