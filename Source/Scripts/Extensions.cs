using System;
using System.Globalization;

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

        public static string ToTitleCase(this string source)
        {
            source = source.ToLower();
            var culture = CultureInfo.CurrentUICulture;
            return culture.TextInfo.ToTitleCase(source);
        }
    }
}