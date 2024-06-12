﻿using Extensions;
using System;
using System.IO;
using System.Reflection;

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
                        return version.ToString().Substring(2);
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
            ConsoleColor.DarkGreen.WriteLine("Processed JSON Documents");

            // Print all subtitles.
            foreach (var sequenceGroup in JSON.Data)
            {
                ConsoleColor.Cyan.WriteLine(sequenceGroup.Key);
                foreach (var sequence in sequenceGroup.Value)
                {
                    ConsoleColor.DarkCyan.WriteLine("  " + sequence.Key);
                    foreach (var variants in sequence.Value.Variants)
                    {
                        var path = Path.Combine(sequenceGroup.Key.ToTitleCase(), variants.Key.ToTitleCase());
                        Directory.CreateDirectory(path);
                        ConsoleColor.DarkYellow.WriteLine("    " + variants.Key);
                        foreach (var subtitle in variants.Value)
                        {
                            ConsoleColor.Gray.WriteLine("      " + subtitle);
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
        }

        public static async void Write(Stream stream, string filepath)
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
                    ConsoleColor.DarkGreen.WriteLine($"Wrote '{name}' [{stream.Length} bytes]");
                }
            }
        }
    }
}