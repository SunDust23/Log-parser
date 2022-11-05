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
                .Where(s => s.EndsWith(".log") || s.EndsWith(".txt")).ToList();
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

        foreach (string logFile in logFiles)
        {
            try
            {
                using (StreamReader sr = new StreamReader(logFile))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        MatchCollection matches = regex.Matches(line);
                        if (matches.Count > 0)
                        {
                            Console.WriteLine(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

    }
}