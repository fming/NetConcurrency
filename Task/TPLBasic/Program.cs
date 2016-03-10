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

            var t1 = Task.Factory.StartNew(() => { DoSomeImportantWork(11, 1000); });
            var t2 = Task.Factory.StartNew(() => { DoSomeImportantWork(21, 3000); });
            var t3 = Task.Factory.StartNew(() => { DoSomeImportantWork(31, 2000); });

            Console.WriteLine("Press to a key to quit");
            Console.ReadKey();
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
    }
}
