using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CountdownEventDemo
{
    class Program
    {
        static CountdownEvent _countdown = new CountdownEvent(3);

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Press enter key to go:");
                Console.ReadKey();

                _countdown = new CountdownEvent(3);

                new Thread(SaySomething).Start("I am thread 1");
                new Thread(SaySomething).Start("I am thread 2");
                new Thread(SaySomething).Start("I am thread 3");

                //Calling Signal decrements the “count”; calling Wait blocks until the count goes down to zero.
                _countdown.Wait();   // Blocks until Signal has been called 3 times
                Console.WriteLine("All threads have finished speaking!");
            }


        }

        static void SaySomething(object thing)
        {
            Thread.Sleep(1000);
            Console.WriteLine(thing);
            _countdown.Signal();
        }

    }
}
