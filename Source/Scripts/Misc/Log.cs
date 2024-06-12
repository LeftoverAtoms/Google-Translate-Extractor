namespace GTE
{
	public static class Log
	{
		/// <summary>
		/// ...
		/// </summary>
		public static void Info(object message, int newlineCount, ConsoleColor color)
		{
			var str = message.ToString();

			for (int i = 0; i < newlineCount; i++)
			{
				str += '\n';
			}

			Console.ForegroundColor = color;
			Console.Write(str);
		}

		/// <summary>
		/// ...
		/// </summary>
		public static void Pause()
		{
			Console.Read();
			Console.Write('\b');
		}
	}
}