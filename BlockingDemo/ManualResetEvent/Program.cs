
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManualResetEventDemo
{
    class Program
    {

        //static ManualResetEvent manual1 = new ManualResetEvent(false);
        static ManualResetEventSlim manual1 = new ManualResetEventSlim(false);
        //static EventWaitHandle manual1 = new EventWaitHandle(false, EventResetMode.ManualReset);


        static void Main(string[] args)
        {
            new Thread(Waiter).Start();
            Thread.Sleep(1000);                  // Pause for a second...

            while (true)
            {
                Console.WriteLine("Press enter key to wake up the waiter");
                Console.ReadLine();
                manual1.Set();                    // Wake up the Waiter.
                
            }

        }

        static void Waiter()
        {
            while (true)
            {
                Console.WriteLine("Waiting...");
                manual1.Wait();
                //manual1.WaitOne();                // Wait for notification
                Console.WriteLine("Notified");
                manual1.Reset();

            }
        }
    }
}
