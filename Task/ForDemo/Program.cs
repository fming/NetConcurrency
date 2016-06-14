using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ForDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //ThreadTest();

            // Test the performance 
            //PerformanceTest();

            //TestLoopState();

            Console.ReadLine();
        }

     

        static void PerformanceTest()
        {
            Console.WriteLine("Sequence");
            int size = 1000000;
            double[] data = new double[size];
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < size; i++)
            {
                data[i] = Math.Pow(
                  new Random().NextDouble(), 0.6);
            }

            Console.WriteLine("done " +
                  sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();

            Console.WriteLine("Parallel.For");
            Console.WriteLine("Process count: " + Environment.ProcessorCount);
            //Console.WriteLine("Core count: " + Environment.Cor);
            sw.Start();

            Parallel.For(0, size, i =>
            {
                data[i] = Math.Pow(
                  new Random().NextDouble(), 0.6);
            });

            Console.WriteLine("done " +
                  sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();
        }

        static void ThreadTest()
        {
            Console.WriteLine("Using C# For Loop \n");

            for (int i = 0; i <= 10; i++)
            {
                Console.WriteLine("i = {0}, thread = {1}", i, Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(10);
            }

            Console.WriteLine("\nUsing Parallel.For \n");

            Parallel.For(0, 10, i =>
            {
                Console.WriteLine("i = {0}, thread = {1}", i,
                Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(10);
            });

            Console.ReadLine();
        }


        static void TestLoopState()
        {
            //foreach (char c in "Hello, world")
            //    if (c == ',')
            //        break;
            //    else
            //        Console.Write(c);

            Parallel.ForEach("Hello, world", (c, loopState) =>
            {
                if (c == ',')
                    loopState.Break();
                else
                    Console.Write(c);
            });

            char[] test = "Hello, world".ToArray();

            //Parallel.For(0, test.Length, (i, loopState) =>
            //{
            //    if (test[i] == ',')
            //        loopState.Break();
            //    else
            //        Console.Write(test[i]);
            //});


        }
    }
}
