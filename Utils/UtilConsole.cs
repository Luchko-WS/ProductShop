using System;
using System.Threading;

namespace Utils
{
    public static class UtilConsole
    {
        public static void Debug(string descr)
        {
            Console.WriteLine($"Thread id = {Thread.CurrentThread.ManagedThreadId}: {descr}");
        }
    }
}
