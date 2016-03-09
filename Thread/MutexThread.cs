using System;
using System.Threading;
using System.Collections.Generic;
namespace ConsoleApplication3
{
    // what's the difference between lock and Mutex
    // A lock is specific to the AppDomain, 
    //while Mutex to the Operating System allowing you to perform cross-process 
    //locking and synchronization
    class Program
    {
        private static Mutex mutex = new Mutex();
        
        private const int numhits = 1;
        private const int numThreads = 4;

        private static void ThreadProcess()
        {
            
            for (int i = 0; i < numhits; i++)
            {
                UseCsharpcorner();
            }
        }

        private static void UseCsharpcorner()
        {
            mutex.WaitOne(); // waiter until it is safe to enter
            Console.WriteLine("{0} has entered in the C_sharpcorner.com", Thread.CurrentThread.Name);

            // Place code to access non-reentrant resources here.

            Thread.Sleep(500); // Wait until it is safe to enter.
            Console.WriteLine("{0} is leaving the C_shartcorner.com", Thread.CurrentThread.Name);

            mutex.ReleaseMutex(); // Release the Mutex
        }



        static void Main(string[] args)
        {
            for (int i = 0; i < numThreads; i++)
            {
                Thread mycorner = new Thread(new ThreadStart(ThreadProcess));
                mycorner.Name = String.Format("Thread{0}", i + 1);
                mycorner.Start();

            }
            Console.ReadLine();
        }



    }
}