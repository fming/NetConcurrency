using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PLINQDemo
{
    class OrderTest
    {
        static string[] words = {
            "Day",
            "Car",
            "Land",
            "Road",
            "Mountain",
            "River",
            "Sea",
            "Shore",
            "Mouse"
        };

        public static void Test()
        {
            //TestLINQ();
            TestParallel();
            //TestParallelSequence();
            //TestParallelOrder();
        }

        static void TestLINQ()
        {
            var query = from word in words
                        where (word.Contains("a"))
                        select word;

            foreach (var result in query)
            {
                Console.WriteLine(result);
            }

            Console.ReadLine();
        }

        // parallel, expect unsequence
        static void TestParallel()
        {
            var query = from word in words.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                        where (word.Contains("a"))
                        select word;

            foreach (var result in query)
            {
                Console.WriteLine(result);
            }

            Console.ReadLine();
        }

        // parallel, expect sequence
        static void TestParallelSequence()
        {
            var query = from word in words.AsParallel().AsSequential()
                        where (word.Contains("a"))
                        select word;

            foreach (var result in query)
            {
                Console.WriteLine(result);
            }

            Console.ReadLine();
        }

        static void TestParallelOrder()
        {
            var query = from word in words.AsParallel()
                        where (word.Contains("a"))
                        orderby word ascending
                        select word;
            var t1 = Task.Factory.StartNew(() => { Thread.Sleep(50000); });
            foreach (var result in query)
            {
                Console.WriteLine(result);
            }

            Console.ReadLine();
        }
    }
}
