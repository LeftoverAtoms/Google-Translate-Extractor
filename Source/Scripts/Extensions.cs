using System;

namespace Extensions
{
	public static class Extensions
	{
        public static void WriteLine(this ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            //Console.ResetColor();
        }
    }
}