using System;
using System.Threading;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(Go);
            Console.ReadKey();
        }

        static void Go(object obj)
        {
            Console.WriteLine(Thread.CurrentThread.IsBackground);
            for (int i = 0; i < 8; i++) Console.Write("y");
        }
    }
}