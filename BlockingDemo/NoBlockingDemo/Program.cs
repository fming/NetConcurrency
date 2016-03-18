using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoBlockingDemo
{

    class Foo
    {
        int _answer;
        bool _complete;

        void A()
        {
            _answer = 123;
            _complete = true;
        }

        void B()
        {
            if (_complete) Console.WriteLine(_answer);
        }

        /*
        If methods A and B ran concurrently on different threads, might it be possible for B to write “0”? The answer is yes — for the following reasons:
        
        •The compiler, CLR, or CPU may reorder your program's instructions to improve efficiency.
        •The compiler, CLR, or CPU may introduce caching optimizations such that assignments to variables won't be visible to other threads right away.

        C# and the runtime are very careful to ensure that such optimizations don’t break ordinary single-threaded code — or multithreaded code that makes proper use of locks. 
        Outside of these scenarios, you must explicitly defeat these optimizations by creating memory barriers (also called memory fences) to limit the effects of instruction 
        reordering and read/write caching.

        */
    }

    class FooGood
    {
        int _answer;
        bool _complete;

        void A()
        {
            _answer = 123;

            //The simplest kind of memory barrier is a full memory barrier(full fence) which prevents any kind of instruction reordering or caching around that fence.
            //Calling Thread.MemoryBarrier generates a full fence; we can fix our example by applying four full fences as follows:

            Thread.MemoryBarrier();    // Barrier 1
            _complete = true;
            Thread.MemoryBarrier();    // Barrier 2
        }

        void B()
        {
            Thread.MemoryBarrier();    // Barrier 3
            if (_complete) 
            {
                Thread.MemoryBarrier();    // Barrier 4
                Console.WriteLine(_answer);
            }
        }
    }


    class Program
    {
        
        static void Main(string[] args)
        {
            bool complete = false;
            var t = new Thread(() =>
            {
                bool toggle = false;
                while (!complete)
                {
                    //Thread.MemoryBarrier();

                    toggle = !toggle;
                }
            });
            t.Start();
            Thread.Sleep(1000);
            complete = true;
            t.Join();        // Blocks indefinitely
        }
    }

    
    /*
        Do We Really Need Locks and Barriers?

        Working with shared writable fields without locks or fences is asking for trouble. 
        There’s a lot of misleading information on this topic — including the MSDN documentation which states 
        that MemoryBarrier is required only on multiprocessor systems with weak memory ordering, 
        such as a system employing multiple Itanium processors. We can demonstrate that memory barriers are important on ordinary Intel Core-2 and Pentium processors with the following short program.
        You’ll need to run it with optimizations enabled and without a debugger (in Visual Studio, select Release Mode in the solution’s configuration manager, and then start without debugging):
        */
}
