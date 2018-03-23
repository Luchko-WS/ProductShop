using System;

namespace Utils
{
    public static class ConsoleHelper
    {
        private static ConsoleColor foreColorBefore = Console.ForegroundColor;
        private static ConsoleColor backColorBefore = Console.BackgroundColor;

        private static void WriteInConsoleWithColor(string descr, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(descr);
            Console.ForegroundColor = foreColorBefore;
        }

        public static void WhiteInfo(string descr)
        {
            WriteInConsoleWithColor(descr, ConsoleColor.White);
        }

        public static void WhiteSuccess(string descr)
        {
            WriteInConsoleWithColor(descr, ConsoleColor.Green);
        }

        public static void WhiteDanger(string descr)
        {
            WriteInConsoleWithColor(descr, ConsoleColor.Red);
        }

        public static void WhiteTips(string descr)
        {
            WriteInConsoleWithColor(descr, ConsoleColor.Yellow);
        }
    }
}
