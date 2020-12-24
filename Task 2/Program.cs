using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;


namespace Task_2
{
    class FileProcessing
    {
        protected FileInfo info;
        protected long ProcessingTime {get; set;}
        protected int LinesCount { get { return File.ReadAllLines(info.FullName).Count(); }}

        public FileProcessing(string path)
        {
            info = new FileInfo(path);
            LogCreation();
        }

        protected void LogCreation()
        {
            string logData = "";
            logData += $"{DateTime.Now.ToString()}\t";
            logData += "FILE_CREATION\t";
            logData += $"FileName: {info.Name}\t";
            logData += $"Extension: {info.Extension}\t";
            logData += $"CreationTime: {info.CreationTime}\n";
            File.AppendAllText(Program.logfilePath, logData);
        }

        protected void LogDeletion()
        {
            string logData = "";
            logData += $"{DateTime.Now.ToString()}\t";
            logData += "FILE_DELETION\t";
            logData += $"FileName: {info.Name}\t";
            logData += $"Extension: {info.Extension}\t";
            logData += $"DeletionTime: {info.CreationTime}\n";
            File.AppendAllText(Program.logfilePath, logData);
        }

        public virtual void Process()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            int linesCount = File.ReadAllLines(info.FullName).Count();
            Thread.Sleep(new Random().Next(500, 3000));
            
            stopwatch.Stop();
            ProcessingTime = stopwatch.ElapsedMilliseconds;
        }

        public void DeleteFile()
        {
            File.Delete(info.FullName);
            LogDeletion();
        }
    }

    class JSONProcessing : FileProcessing
    {
        public JSONProcessing(string path) : base(path){ }

        public override void Process()
        {
            base.Process();
            LogProcessing();
        }

        private void LogProcessing()
        {
            string logData = "";
            logData += $"{DateTime.Now.ToString()}\t";
            logData += "PROCESSING_JSON\t";
            logData += $"FileName: {info.Name}\t";
            logData += $"ProcessingStart: {DateTime.Now.ToString()}\t";
            logData += $"LinesInFile: {LinesCount}\t";
            logData += $"ProcessingTime: {ProcessingTime} ms.\n";
            File.AppendAllText(Program.logfilePath, logData);
        }
    }

    class XMLProcessing : FileProcessing
    {
        public XMLProcessing(string path) : base(path){ }

        public override void Process()
        {
            base.Process();
            LogProcessing();
        }

        private void LogProcessing()
        {
            string logData = "";
            logData += $"{DateTime.Now.ToString()}\t";
            logData += "PROCESSING_XML\t";
            logData += $"FileName: {info.Name}\t";
            logData += $"ProcessingStart: {DateTime.Now.ToString()}\t";
            logData += $"LinesInFile: {LinesCount}\t";
            logData += $"ProcessingTime: {ProcessingTime} ms.\n";
            File.AppendAllText(Program.logfilePath, logData);
        }
    }

    class Program
    {
        public static string logfilePath;

        private static void FileHandler(object source, FileSystemEventArgs e)
        {
            FileInfo fileInfo = new FileInfo(e.FullPath);
            
            FileProcessing processing;
            if (fileInfo.Extension == ".json")
                processing = new JSONProcessing(fileInfo.FullName);
            else if (fileInfo.Extension == ".xml")
                processing = new XMLProcessing(fileInfo.FullName);
            else
                processing = new FileProcessing(fileInfo.FullName);
            
            if (fileInfo.Extension == ".json" || fileInfo.Extension == ".xml")
                Task.Run(() => processing.Process());
            else
                processing.DeleteFile();
        } 

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string directoryPath = args[0];

                if (Directory.Exists(directoryPath))
                {
                    logfilePath = Path.Combine(directoryPath, "directory.log");
                    if (!File.Exists(logfilePath))
                    {
                        File.Create(logfilePath);
                        Console.WriteLine("[!] New .log file created.");
                    }
                    else
                        Console.WriteLine($"[!] Using existing {Path.GetFileName(logfilePath)} file.");

                    FileSystemWatcher directoryWatcher = new FileSystemWatcher()
                    {
                        Path = directoryPath,
                        Filter = "*.*"
                    };             
                    directoryWatcher.Created += FileHandler;
                    directoryWatcher.EnableRaisingEvents = true;

                    Console.WriteLine("Press 'q' to stop watching directory!");
                    while (Console.Read() != 'q') ;
                }
                else
                {
                    Console.WriteLine("Directory not exists!");
                    Console.WriteLine("Usage: DirectoryWatcher.exe <directory_path>");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Specify path to directory!");
                Console.WriteLine("Usage: DirectoryWatcher.exe <directory_path>");
                return;
            }
        }
    }
}
