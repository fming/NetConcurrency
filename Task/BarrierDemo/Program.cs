using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BarrierDemo
{
    class Program
    {

        static bool success = false;

        static Barrier barrier = new Barrier(2, (b) =>
        {
            success = ShuffleString.CheckResult( b.CurrentPhaseNumber);
        });


        static void Main(string[] args)
        {
            Task t1 = Task.Factory.StartNew(() => Solve(ShuffleString.words1));
            Task t2 = Task.Factory.StartNew(() => Solve(ShuffleString.words2));

            // Keep the console window open.
            Console.ReadLine();

        }

          
        public static void Solve(string[] wordArray)
        {
            while (success == false)
            {
                ShuffleString.Reorder(wordArray);

                // We need to stop here to examine results
                // of all thread activity. This is done in the post-phase
                // delegate that is defined in the Barrier constructor.
                barrier.SignalAndWait();
            }
        }
        
    }
}
