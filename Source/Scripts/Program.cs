using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace GTE
{
	// Interface
	public static partial class Program
	{
		public static void Write(Stream stream, string path, string extension)
		{
			Directory.CreateDirectory(path);
			using (FileStream file = new FileStream(path + extension, FileMode.Create, FileAccess.Write))
			{
				stream.CopyTo(file);
				Log.Info($"Wrote '{extension}' [{stream.Length} bytes]");
			}

			bytesWritten += stream.Length;
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

			bytesWritten += text.Length;
		}
	}

	// Internal
	public static partial class Program
	{
		static long bytesWritten;

		static void Main(string[] args)
		{
			Log.Info(string.Format(Text.Intro, Assembly.GetExecutingAssembly().GetName().Version));

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

				if (GitGuid.Initialize(assetPath) == true)
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
			while (Google.Downloads > 0)
			{ continue; }

			// Wait for threads to print their messages.
			Thread.Sleep(2000);
			Console.WriteLine();

			// Display elapsed time.
			if (bytesWritten > 0)
			{
				string time = (Google.WaitTime * 0.001f).ToString("0.00");
				string kilobytes = (bytesWritten * 0.001f).ToString("0");
				Log.Info($"Spent {time} seconds writing {kilobytes} KB!", true);
			}
			else
			{
				Log.Error("Failed to write any files!", true);
			}
		}
	}
}