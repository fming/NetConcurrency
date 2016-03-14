using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPLBasic
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestRawTask();
            //TestTaskFactory();
            //TestContinueTask();
            //TestTaskWaitAll();

            var intList = new List<int> { 1, 2, 6, 7, 8, 32, 65, 75, 85, 643, 732, 733, 882 };

            // Thread blocking
            Parallel.ForEach(intList, (i) => Console.WriteLine(i));

            // Paraell
            Parallel.For(0, 100, (i) => Console.WriteLine());
            

            Console.WriteLine("Press to a key to quit");
            Console.ReadKey();
        }

        private static void TestTaskWaitAll()
        {
            var t1 = Task.Factory.StartNew(() => { DoSomeImportantWork(11, 1000); });
            var t2 = Task.Factory.StartNew(() => { DoSomeImportantWork(21, 3000); });
            var t3 = Task.Factory.StartNew(() => { DoSomeImportantWork(31, 2000); });

            var taskList = new List<Task> { t1, t2, t3 };

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine("Doing some other work");
                Thread.Sleep(250);

            }

            Task.WaitAll(taskList.ToArray());
        }

        private static void TestContinueTask()
        {
            var t1 = Task.Factory.StartNew(() => { DoSomeImportantWork(11, 1000); }).ContinueWith((prevTask) => { DoSomeOtherImportantWork(1, 1000); });
            var t2 = Task.Factory.StartNew(() => { DoSomeImportantWork(21, 3000); });
        }

        private static void TestTaskFactory()
        {
            var t1 = Task.Factory.StartNew(() => { DoSomeImportantWork(11, 1000); });
            var t2 = Task.Factory.StartNew(() => { DoSomeImportantWork(21, 3000); });
            var t3 = Task.Factory.StartNew(() => { DoSomeImportantWork(31, 2000); });
        }

        private static void TestRawTask()
        {
            Task v1 = new Task(() =>
            {
                DoSomeImportantWork(1, 1000);
            });

            Task v2 = new Task(() =>
            {
                DoSomeImportantWork(2, 1000);
            });

            Task v3 = new Task(() =>
            {
                DoSomeImportantWork(3, 1000);
            });
        }

        private static void DoSomeImportantWork(int id, int sleepTime)
        {
            Console.WriteLine("Task {0} is begining work", id);
            Thread.Sleep(sleepTime);
            Console.WriteLine("Task {0} is completed", id);
        }

        private static void DoSomeOtherImportantWork(int id, int sleepTime)
        {
            Console.WriteLine("Task {0} is begining more work", id);
            Thread.Sleep(sleepTime);
            Console.WriteLine("Task {0} has completed more", id);
        }
    }
}
