namespace GTE
{
	public static class Log
	{
		public static void Info(object message, ConsoleColor color = ConsoleColor.White)
		{
			Console.ForegroundColor = color;
			Console.Write($"{message}\n");
		}
		public static void Warning(object message)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write($"Warning: {message}\n");
		}
		public static void Error(object message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write($"Error: {message}\n");
		}

		public static void Pause()
		{
			Console.Read();
			Console.Write('\b');
		}
	}
}