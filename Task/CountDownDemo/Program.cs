using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CountDownDemo
{
    class Program
    {


        static void Main(string[] args)
        {
            // Initialize a queue and a CountdownEvent
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>(Enumerable.Range(0, 10000));
            CountdownEvent cde = new CountdownEvent(10000); // initial count = 10000


            // This is the logic for all queue consumers
            Action consumer = () =>
            {
                int local;

                // decreate CDE count once for each element consumed from queue

                Thread.Sleep(2000);

                while (queue.TryDequeue(out local)) cde.Signal();

                Thread.Sleep(1000);
            };

            Task t1 = Task.Factory.StartNew(consumer);
            Task t2 = Task.Factory.StartNew(consumer);

            // And wait for queue to empty by waiting on cde
            cde.Wait(); // will return when cde count reaches 0

            Console.WriteLine("Done emptying queue.  InitialCount={0}, CurrentCount={1}, IsSet={2}",
            cde.InitialCount, cde.CurrentCount, cde.IsSet);


            Thread.Sleep(2000);

            // Resetting will cause the CountdownEvent to un-set, and resets InitialCount/CurrentCount
            // to the specified value
            cde.Reset(10);

            // AddCount will affect the CurrentCount, but not the InitialCount
            cde.AddCount(2);

            Console.WriteLine("After Reset(10), AddCount(2): InitialCount={0}, CurrentCount={1}, IsSet={2}",
                cde.InitialCount, cde.CurrentCount, cde.IsSet);

            // Now try waiting with cancellation
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.Cancel(); // cancels the CancellationTokenSource
            try
            {
                //Blocks the current thread until the CountdownEvent is set, while observing a CancellationToken.
                cde.Wait(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("cde.Wait(preCanceledToken) threw OCE, as expected");
            }
            finally
            {
                cts.Dispose();
            }
            // It's good for to release a CountdownEvent when you're done with it.
            cde.Dispose();

            // Keep the console window open.
            Console.ReadLine();

         
        }

    }
}
