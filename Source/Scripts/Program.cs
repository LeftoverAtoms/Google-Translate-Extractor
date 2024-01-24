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

        public static void Write(Stream stream, string path, string extension)
        {
            Directory.CreateDirectory(path);
            using (FileStream file = new FileStream(path + extension, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(file);
                Log.Info($"Wrote '{extension}' [{stream.Length} bytes]");
            }

            BytesWritten += stream.Length;
        }
        public static void Write(string text, string path, string extension)
        {
            Directory.CreateDirectory(path);
            using (FileStream file = new FileStream(path + extension, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.Write(text);
                Log.Info($"Wrote '{extension}' [{text.Length} bytes]");
            }

            BytesWritten += text.Length;
        }

        static void Main(/*string[] args*/)
        {
            Log.Info(string.Format(Text.Intro, Assembly.GetExecutingAssembly().GetName().Version), ConsoleColor.Blue);

            Log.Info("Specify the absolute path of Unity Assets folder to register pre-existing guids.\n");

            string? assetPath;

            while (true)
            {
                assetPath = Console.ReadLine();

                if (assetPath == null || assetPath == "")
                {
                    Console.CursorTop -= 1;
                    Log.Info("Ignoring pre-existing Unity guids!\n");
                    break;
                }

                if (GitGuid.Init(assetPath) == true)
                {
                    break;
                }
            }

            Config.Parse(out Locale[]? data);

            foreach (Locale locale in data)
            {
                foreach (Sequence sequence in locale.Sequence)
                {
                    new Thread(new ThreadStart(() => Google.Download(sequence, locale))).Start();
                }
            }

            // Wait for all downloads to complete.
            while (Google.Downloads > 0) ;

            // Wait for threads to print their messages.
            Thread.Sleep(2000);
            Console.WriteLine();

            // Display elapsed time.
            if (BytesWritten > 0)
            {
                string time = (Google.WaitTime * 0.001f).ToString("0.00");
                string kilobytes = (BytesWritten * 0.001f).ToString("0.0");

                Log.Info($"Spent {time} seconds writing {kilobytes} KB!");
                Log.Pause();
            }
            else
            {
                Log.Error("Failed to write any files!");
                Log.Pause();
            }
        }
    }
}