using System;

namespace Utils
{
    public static class ConsoleHelper
    {
        private static ConsoleColor foreColorBefore = Console.ForegroundColor;
        private static ConsoleColor backColorBefore = Console.BackgroundColor;

        public static void WhiteSuccess(string descr)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(descr);
            Console.ForegroundColor = foreColorBefore;
        }

        public static void WhiteDanger(string descr)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(descr);
            Console.ForegroundColor = foreColorBefore;
        }

        public static void WhiteInfo(string descr)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(descr);
            Console.ForegroundColor = foreColorBefore;
        }
    }
}
