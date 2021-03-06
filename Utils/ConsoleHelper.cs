﻿using System;

namespace Utils
{
    public static class ConsoleHelper
    {
        private static object _locker = new object();
        private static ConsoleColor foreColorBefore = Console.ForegroundColor;
        private static ConsoleColor backColorBefore = Console.BackgroundColor;

        private static void WriteInConsoleWithColor(string descr, ConsoleColor color)
        {
            lock (_locker)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(descr);
                Console.ForegroundColor = foreColorBefore;
            }
        }

        public static void WriteInfo(string descr)
        {
            WriteInConsoleWithColor(descr, ConsoleColor.White);
        }

        public static void WriteSuccess(string descr)
        {
            WriteInConsoleWithColor(descr, ConsoleColor.Green);
        }

        public static void WriteDanger(string descr)
        {
            WriteInConsoleWithColor(descr, ConsoleColor.Red);
        }

        public static void WriteTips(string descr)
        {
            WriteInConsoleWithColor(descr, ConsoleColor.Yellow);
        }
    }
}
