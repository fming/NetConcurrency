using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WaitPulseDemo
{
    class Program
    {

        static readonly object _locker = new object();
        static bool _go;

        static void Main(string[] args)
        {
            // The new thread will block, because _go==false.
            new Thread(Work).Start();

            Console.ReadLine();            // Wait for user to hit Enter

            lock (_locker)                 // Let's now wake up the thread by
            {                              // setting _go=true and pulsing.
                _go = true;
                Monitor.Pulse(_locker); // Notifies a thread in the waiting queue of a change in the locked object's state.
            }


        }


        static void Work()
        {
            lock (_locker)
            {
                while (!_go)
                    Monitor.Wait(_locker); //Releases the lock on an object and blocks the current thread until it reacquires the lock
            }

            Console.WriteLine("Woken!!!");
        }

    }
}
