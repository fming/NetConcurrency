using System;
using System.Threading;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(Print);
            t.Start("Hello from it!");
            Print("Hello from the main thread!");
 
        }

        static void Print(object messageObj)
        {
            string message = (string)messageObj;
            Console.WriteLine(message);
        }

    }
}
