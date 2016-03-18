using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlockingDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            Thread t1 = new Thread(DoSomeBlockthing);
            t1.Start();

            while (true)
            {
                Console.ReadKey();
                Console.WriteLine("t1, is blocked {0}", IsThreadBlocked(t1));
            }
        }

        static void DoSomeBlockthing()
        {
            Console.WriteLine("before sleep, is blocked {0}", IsThreadBlocked(Thread.CurrentThread));
            // when a thread blocks or unblocks, the operating system performs a context switch, the incurs an overhead of a few microseconds.
            //Thread.Sleep(0);
            Thread.Sleep(5000);

        }

        static void DoSomeSpinthing()
        {
            int i = 0;

            // waste resouce, but it can be efficent when you expect a condition to be statisfied soon(a few microseconds) to avoid overhead and latency of a context switch
            while (i < 10)
            {
                i++;
            }
        }

        static bool IsThreadBlocked(Thread t)
        {
            return (t.ThreadState & ThreadState.WaitSleepJoin) != 0;
        }
    }
}
