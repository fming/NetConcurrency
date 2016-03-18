using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    [Synchronization]
    public class AutoLock : ContextBoundObject
    {
        public void Demo()
        {
            Console.Write("Start...");
            Thread.Sleep(1000);
            Console.WriteLine("end");
        }

        public void Test()
        {
            new Thread(Demo).Start();
            new Thread(Demo).Start();
            new Thread(Demo).Start();
            Console.ReadLine();
        }

        public static void Main()
        {
            new AutoLock().Test();

            Console.ReadLine();
        }
    }
}
