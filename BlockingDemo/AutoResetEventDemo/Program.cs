using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoResetEventDemo
{
    class BasicWaitHandle
    {
        static EventWaitHandle _waitHandle = new AutoResetEvent(false);

        static void Main()
        {
            new Thread(Waiter).Start();            
            Thread.Sleep(1000);                  // Pause for a second...

            Console.WriteLine("Press enter key to wake up the waiter");
            Console.ReadLine();
            _waitHandle.Set();                    // Wake up the Waiter.

            Console.ReadLine();

        }

        static void Waiter()
        {
            Console.WriteLine("Waiting...");
            _waitHandle.WaitOne();                // Wait for notification
            Console.WriteLine("Notified");
        }
    }

}
