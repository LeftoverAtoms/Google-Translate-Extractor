using Extensions;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace GTE
{
    internal static class Program
    {
        private static string AppData { get; } = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        private static string Version
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();
                if (assembly != null)
                {
                    var version = assembly.GetName().Version;
                    if (version != null)
                    {
                        string str = version.ToString();
                        int end = str.LastIndexOf('.');
                        return str.Substring(0, end);
                    }
                }
                return "N/A";
            }
        }

        private static void Main()
        {
            PrintAttributes();

            foreach (var file in Directory.GetFiles(AppData, "*.json"))
            {
                JSON.Deserialize(file);
            }

            // Wait until application is done processing all documents.
            while (JSON.HasProcesses)
            {
            }
            ConsoleColor.DarkGreen.WriteLine("Processed documents");

            // Sequence Groups:
            foreach (var a in JSON.Data)
            {
                string type = a.Key;
                var sequences = a.Value;

                // Sequence:
                foreach (var b in sequences)
                {
                    string name = b.Key;
                    var variants = b.Value.Variants;

                    // ERROR:
                    if (variants == null)
                    {
                        ConsoleColor.Red.WriteLine($"Sequence: '{name}' variants are invalid");
                        continue;
                    }

                    // Sequence Variants:
                    foreach (var c in variants)
                    {
                        string language = c.Key;
                        string[] subtitles = c.Value;

                        string path = Path.Combine("Sounds", type.ToTitleCase(), language.ToTitleCase());

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        // Sequence Variant Subtitles:
                        for (int i = 0; i < subtitles.Length; i++)
                        {
                            // Save iterator as it will change while the task is running.
                            int index = i;

                            // Combine path and file name.
                            string count = (index + 1).ToString("00");
                            string filepath = Path.Combine(path, $"{name}_{count}.mp3");

                            // Download each sequence variant subtitle.
                            Task.Run(async () =>
                            {
                                var stream = await Google.Request(language, subtitles[index]);
                                Write(stream, filepath);
                            });
                        }
                    }
                }
            }

            Console.Read();
        }

        private static void PrintAttributes()
        {
            string message = $"Google Translate Extractor (Version {Version}) by Adam Calvelage";
            ConsoleColor.Blue.WriteLine(message);
            Console.WriteLine();
        }

        private static async void Write(Stream stream, string filepath)
        {
            string? path = Path.GetDirectoryName(filepath);
            string? name = Path.GetFileName(filepath);

            if (path != null && name != null)
            {
                // Create missing directory.
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Write stream to file.
                using (var file = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(file);
                    await file.DisposeAsync();
                    ConsoleColor.DarkCyan.WriteLine($"Wrote {stream.Length / 1000} KB -> '{filepath.Replace(@"Sounds\", "")}'");
                }
            }
        }
    }
}