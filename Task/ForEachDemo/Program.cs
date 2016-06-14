using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ForEachDemo
{
    //foreach (int i in list)
    //    f(i);
    //Parallel.ForEach(list, f);

    class Program
    {
        static void Main(string[] args)
        {
            IList<string> data = new List<string>() { "I", "test1", "test2", "test3", "test4" };

            Stopwatch sw = new Stopwatch();
            sw.Start();
            Parallel.ForEach(data, (value) =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("Thread: " + Thread.CurrentThread.ManagedThreadId + "content: "+ value);

            });
            Console.WriteLine("Main Thread: " + Thread.CurrentThread.ManagedThreadId);

            Console.WriteLine("done " +
                sw.Elapsed.TotalMilliseconds.ToString());
            sw.Reset();
            Console.ReadLine();

        }
    }
}
