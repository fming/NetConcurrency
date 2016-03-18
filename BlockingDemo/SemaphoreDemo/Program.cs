using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//There are two functionally similar versions of this class: Semaphore and SemaphoreSlim.
//The latter was introduced in Framework 4.0 and has been optimized to meet the low-latency demands of parallel programming.
//It’s also useful in traditional multithreading because it lets you specify a cancellation token when waiting.
//It cannot, however, be used for interprocess signaling.

//Semaphore incurs about 1 microsecond in calling WaitOne or Release; SemaphoreSlim incurs about a quarter of that.


namespace SemaphoreDemo
{
    class TheClub
    {

        static SemaphoreSlim _sem = new SemaphoreSlim(3);

        static void Main(string[] args)
        {

            for (int i = 2; i <= 7; i++) new Thread(Enter).Start(i);

            Console.ReadKey();
        }

        static void Enter(object id)
        {
            Console.WriteLine(id + " wants to enter");
            _sem.Wait();
            Console.WriteLine(id + " is in!");
            Thread.Sleep(1000 * (int)id);
            Console.WriteLine(id + " is leaving");
            _sem.Release();
        }
    }
}
