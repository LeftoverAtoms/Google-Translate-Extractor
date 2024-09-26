using Extensions;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace GTE;

internal static class Program
{
    private static string AppData { get; } = Path.Combine(Directory.GetCurrentDirectory(), "Data");

    private static void Main()
    {
        PrintAttributes();

        foreach (string file in Directory.GetFiles(AppData, "*.json"))
        {
            JSON.Deserialize(file);
        }

        // Wait until each document has been processed.
        while (JSON.IsProcessing)
        {
            continue;
        }

        ConsoleColor.DarkYellow.WriteLine("Processed documents");

        // Groups:
        foreach (var (type, sequences) in JSON.Data)
        {
            // Sequences:
            foreach (var sequence in sequences)
            {
                // Variants:
                foreach (var (language, variant) in sequence.Variants)
                {
                    string path = Path.Combine("Sounds", type, language).ToTitleCase();

                    // Subtitles:
                    for (int i = 0; i < variant.Subtitles.Length; i++)
                    {
                        // Combine path and file name.
                        string count = (i + 1).ToString("00");
                        string filePath = Path.Combine(path, $"{sequence.Name}_{count}.mp3");

                        // Download each sequence variant subtitle.
                        Task task = Google.Download(language, variant.Subtitles[i], filePath);
                        Task.Run(async () => await task);
                    }
                }
            }
        }

        Console.Read();
    }

    private static void PrintAttributes()
    {
        string message = $"Google Translate Extractor (Version {GetVersion()}) by Adam Calvelage";
        ConsoleColor.White.WriteLine(message);
        Console.WriteLine();
    }

    private static string GetVersion()
    {
        var assembly = Assembly.GetEntryAssembly();
        if (assembly == null)
        {
            return "N/A";
        }

        var version = assembly.GetName().Version;
        if (version == null)
        {
            return "N/A";
        }

        string str = version.ToString();
        int end = str.LastIndexOf('.');
        return str.Substring(0, end);
    }

    public static async void Write(Stream stream, string filePath)
    {
        string path = Path.GetDirectoryName(filePath);
        string name = Path.GetFileName(filePath);

        if (path == null || null == name)
        {
            return;
        }

        // Create missing directory.
        bool hasPath = Directory.Exists(path);
        if (!hasPath)
        {
            Directory.CreateDirectory(path);
        }

        // Write stream to file.
        using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await stream.CopyToAsync(file);
            await file.DisposeAsync();
            ConsoleColor.DarkCyan.WriteLine($"Wrote {stream.Length / 1000} KB -> ..\\{filePath}");
        }
    }
}