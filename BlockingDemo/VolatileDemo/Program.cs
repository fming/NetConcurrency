using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VolatileDemo
{


    class Program
    {
         /*volatile*/ static bool complete = false;
        static void Main(string[] args)
        {
            
            var t = new Thread(() =>
            {
                bool toggle = false;
                while (!complete) toggle = !toggle;
            });
            t.Start();
            Thread.Sleep(1000);
            complete = true;
            t.Join();        // Blocks indefinitely

        }
    }



    /*The volatile keyword instructs the compiler to generate an acquire-fence on every read from that field, 
    and a release-fence on every write to that field.An acquire-fence prevents other reads/writes from being moved before the fence;
    a release-fence prevents other reads/writes from being moved after the fence.These “half-fences” are faster than full fences because they give the runtime and hardware more scope for optimization.*/
}
