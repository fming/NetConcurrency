using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WaitDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Demo1();
            //Demo2();
            //Demo3();
            Demo4();
            Console.ReadKey();
        }

        // Demo: 
        //  WaitAll
        static void Demo1()
        {
            var task1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Task1");
            });

            var task2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Task2");
            });


            //Task.WaitAll(task1, task2);

            Console.WriteLine("after Tasks are completed");

        }

        // Demo:
        //     ContinueWith 1
        static void Demo2()
        {
            Console.ReadLine();
            Console.WriteLine("Start");

            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            var sw = Stopwatch.StartNew();
            var t1 = Task.Factory.StartNew(() => { Console.WriteLine("This task1"); return "hello".ToArray(); }, ct);

            var t3 = t1.ContinueWith((t) =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("This is task2(should be after Task1)");
                // Do something with the result
                // returned by the task's delegate
                for (int i = 0; i < t.Result.Count(); i++)
                {
                    Console.Write(" " + t.Result[i]);
                }
            }).ContinueWith((t) =>
            {
                Console.WriteLine("");
                Console.WriteLine("This is task3(should be after Task2)");
            });

            try
            {
                t3.Wait();
            }
            catch (AggregateException)
            {

            }
        }

        // Demo:
        //     ContinueWith 2
        static void Demo3()
        {
            Console.ReadLine();
            Console.WriteLine("Start");

            var t1 = Task.Factory.StartNew(() => { Console.WriteLine("this is t1"); });
            var t2 = t1.ContinueWith((t) => { Console.WriteLine("this is t2"); });
            var t3 = t1.ContinueWith((t) => { Console.WriteLine("this is t3"); });
            var t4 = t1.ContinueWith((t) => { Console.WriteLine("this is t4"); });
            var t5 = t1.ContinueWith((t) => { Console.WriteLine("this is t5"); });

            Task.WaitAll(t2, t3, t4, t5);

        }

        // Demo:
        // TaskContinuationOptions
        static void Demo4()
        {
            Console.ReadLine();
            Console.WriteLine("Start");

            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            var sw = Stopwatch.StartNew();
            var t1 = Task.Factory.StartNew(() => {
                //Thread.Sleep(1000);
                Console.WriteLine("This task1");
                ct.ThrowIfCancellationRequested();
                return "hello".ToArray(); }, ct);

            //cts.Cancel();
            
            var t2 = t1.ContinueWith((t) =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("This is task2(should be after Task1 with cancel)");
            }, TaskContinuationOptions.OnlyOnCanceled);

            try
            {
                t2.Wait();
            }
            catch (AggregateException)
            {

            }




        }

    }
}
