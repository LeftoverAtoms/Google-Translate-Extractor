global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.IO;
global using System.Linq;
global using System.Net.Http;
global using System.Reflection;
global using System.Text.Encodings.Web;
global using System.Text.Json;
global using System.Threading;
global using System.Threading.Tasks;

namespace GTE
{
    public static class Program
    {
        static long BytesWritten;

        /// <summary>
        /// Write to file using a stream.
        /// </summary>
        public static void Write(Stream stream, string path, string extension)
        {
            Directory.CreateDirectory(path);

            using (FileStream file = new FileStream(path + extension, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(file);
                Log.Info($"Wrote '{extension}' [{stream.Length} bytes]", 1, ConsoleColor.White);
            }

            BytesWritten += stream.Length;
        }

        /// <summary>
        /// Write to file using a string.
        /// </summary>
        public static void Write(string text, string path, string extension)
        {
            Directory.CreateDirectory(path);

            using (FileStream file = new FileStream(path + extension, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.Write(text);
                Log.Info($"Wrote '{extension}' [{text.Length} bytes]", 1, ConsoleColor.White);
            }

            BytesWritten += text.Length;
        }

        /// <summary>
        /// Main entry point.
        /// </summary>
        static void Main()
        {
            ShowCredits();

            Config.Parse(out Locale[]? data);

            // Download each sequence on a separate thread.
            foreach (Locale locale in data)
            {
                foreach (Sequence sequence in locale.Sequence)
                {
                    new Thread(new ThreadStart(() => Google.Download(sequence, locale))).Start();
                }
            }

            // Wait for downloads to complete.
            while (Google.Downloads > 0)
            {
                continue;
            }

            Thread.Sleep(2500);
            Console.WriteLine();

            // Display stats.
            if (BytesWritten > 0)
            {
                string time = (Google.WaitTime * 0.001f).ToString("0.00");
                string kilobytes = (BytesWritten * 0.001f).ToString("0.0");

                Log.Info($"Spent {time} seconds writing {kilobytes} KB!", 1, ConsoleColor.White);
                Log.Pause();
            }

            // Failed!
            else
            {
                Log.Info("Error: Failed to write any files!", 1, ConsoleColor.Red);
                Log.Pause();
            }
        }

        /// <summary>
        /// Print attributions.
        /// </summary>
        static void ShowCredits()
        {
            // Application version, excluding revisions.
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString()[2..];

            // Format message.
            string message = "Google Translate Extractor (Version " + version + ") by Adam Calvelage";

            // Print message.
            Log.Info(message, 2, ConsoleColor.Blue);
        }
    }
}