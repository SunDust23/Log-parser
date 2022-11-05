using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class MainClass
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            throw new ArgumentException("Path or regular exception arguments can't be null");
        }
        string path = args[0];
        Regex regex = new Regex(args[1]);

        List<string> logFiles = new();


        if (Directory.Exists(path))
        {
            logFiles = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => (s.EndsWith(".log") || s.EndsWith(".txt")) && !s.EndsWith("output.txt")).ToList();
        }
        else if (File.Exists(path))
        {
            logFiles.Add(path);
        }
        else
        {
            throw new Exception("File not exists");
        }

        //Console.WriteLine(logFiles.Aggregate((A, B) => A + '\n' + B));

        //Создаём или перезаписываем файл в той же папке с названием output.txt
        string outputPath = Path.Combine(path, "output.txt");
        try
        {
            using StreamWriter sw = new StreamWriter(outputPath, false);

            foreach (string logFile in logFiles)
            {
                try
                {
                    int lineCounts = 0;
                    using var progress = new ProgressBar();
                    using (StreamReader sr = new StreamReader(logFile))
                    {
                        Console.Write("Read file \"" + logFile + "\": ");
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();

                            progress.Report(sr.BaseStream.Position * 1.0 / sr.BaseStream.Length);
                            //Искусственно замедляем чтение файла для  наглядности работы Progress bar
                            Thread.Sleep(1);

                            MatchCollection matches = regex.Matches(line);
                            if (matches.Count > 0)
                            {
                                lineCounts++;
                                //Console.WriteLine(line);
                                sw.WriteLine("{0}:\t{1}", logFile, line);

                            }
                        }
                    }
                    Console.WriteLine("Done!");
                    Console.WriteLine("Founded lines: " + lineCounts);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error opening file for reading");
                    Console.WriteLine(ex.ToString() + "\n");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error opening file for writing");
            Console.WriteLine(ex.ToString() + "\n");
        }
    }
}