using System;

namespace GTE
{
	// Interface
	public static partial class Log
	{
		public static void Info(object message, bool pause = false)
		{
			Console.Write(message + "\n");

			if (pause)
			{
				Console.Read();
				Console.Write('\b');
			}
		}
		public static void Warning(object message, bool pause = false)
		{
			Console.Write("Warning: " + message + '\n');

			if (pause)
			{
				Console.Read();
				Console.Write('\b');
			}
		}
		public static void Error(object message, bool pause = false)
		{
			Console.Write("Error: " + message + '\n');

			if (pause)
			{
				Console.Read();
				Console.Write('\b');
			}
		}
	}
}