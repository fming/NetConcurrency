using System;
using System.Threading;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(WriteY);
            t.Start();
            Console.WriteLine(t.IsBackground);
            // Simultaneously , do something on the main thread
            for (int i = 0; i < 8; i++) Console.Write("x");
        }

        static void WriteY()
        {
            for (int i = 0; i < 8; i++) Console.Write("y");
        }
    }
}