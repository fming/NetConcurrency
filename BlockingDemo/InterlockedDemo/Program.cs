using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterlockedDemo
{
    class Atomicity
    {
        static int _x, _y;
        static long _z;

        static void Test()
        {
            long myLocal;
            _x = 3;             // Atomic
            _z = 3;             // Nonatomic on 32-bit environs (_z is 64 bits)
            myLocal = _z;       // Nonatomic on 32-bit environs (_z is 64 bits)
            _y += _x;           // Nonatomic (read AND write operation)
            _x++;               // Nonatomic (read AND write operation)
        }
    }

    class Program
    {
        const int MAX = 5;
        static void Main(string[] args)
        {
            int k = 0;
            while (true)
            {
                Console.WriteLine("Test unsafe");
                for (int i = 0; i < 100; i++)
                {
                    var t = new Thread(ThreadUnsafe.Go);
                    t.Start();
                }

                Thread.Sleep(1000);
                Console.WriteLine("may output 0, Press Enter to verify: ");

                k = 0;
                while (k++ < MAX)
                {
                    Console.ReadKey();
                    Console.WriteLine(ThreadUnsafe._x);
                }


                Console.WriteLine("Test Lock");
                for (int i = 0; i < 100; i++)
                {
                    var t = new Thread(ThreadSafe.Go);
                    t.Start();
                }
                Thread.Sleep(1000);
                Console.WriteLine("expect to output 0, Press Enter to verify: ");
                k = 0;
                while (k++ < MAX)
                {
                    Console.ReadKey();
                    Console.WriteLine(ThreadSafe._x);
                }

                Console.WriteLine("Test Interlock");
                for (int i = 0; i < 100; i++)
                {
                    var t = new Thread(InterlockSafe.Go);
                    t.Start();
                }
                Thread.Sleep(1000);
                Console.WriteLine("expect to output 0, Press Enter to verify: ");
                k = 0;
                while (k++ < MAX)
                {
                    Console.ReadKey();
                    Console.WriteLine(InterlockSafe._x);
                }

            }
        }
    }


    class ThreadUnsafe
    {
        public static int _x = 10000;
        public static void Go()
        {
            for (int i = 0; i < 100; i++)
            {
                _x--;
            }
        }
    }


    class ThreadSafe
    {
        public static int _x = 10000;
        private static object locker = new object();
        public static void Go()
        {


            for (int i = 0; i < 100; i++)
            {
                lock (locker)
                {
                    _x--;
                }
            }
        }
    }

    class InterlockSafe
    {
        public static int _x = 10000;
        public static void Go()
        {
            for (int i = 0; i < 100; i++)
            {
                Interlocked.Decrement(ref _x);

                //// Simple increment/decrement operations:
                //Interlocked.Increment(ref _x);                              // 1
                //Interlocked.Decrement(ref _x);                              // 0

                //// Add/subtract a value:
                //Interlocked.Add(ref _x, 3);                                 // 3

                //// Read a 64-bit field:
                //Console.WriteLine(Interlocked.Read(ref _x));               // 3

                //// Write a 64-bit field while reading previous value:
                //// (This prints "3" while updating _x to 10)
                //Console.WriteLine(Interlocked.Exchange(ref _x, 10));       // 10

                //// Update a field only if it matches a certain value (10):
                //Console.WriteLine(Interlocked.CompareExchange(ref _x,
                //                                                123, 10);      // 123

            }
        }
    }

}
