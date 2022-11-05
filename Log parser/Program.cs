using System.IO;
using System.Linq;

class MainClass
{
    static void Main(string[] args)
    {
        string path = args[0];

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

        Console.WriteLine(logFiles.Aggregate((A, B) => A + '\n' + B));

        foreach (string logFile in logFiles)
        {
            try
            {
               File.ReadAllText(logFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

    }
}