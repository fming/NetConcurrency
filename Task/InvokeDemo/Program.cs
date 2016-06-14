using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InvokeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestInvoke();
            TestInvoke1();

            Console.ReadKey();
        }

        static void F1() { Console.WriteLine("F1()"); tok.Cancel(); }
        static void F2() { Console.WriteLine("F2()"); }
        static void F3() { Console.WriteLine("F3()"); }

        static void TestInvoke()
        {
            Parallel.Invoke(F1, F2, F3);
        }

        static CancellationTokenSource tok = new CancellationTokenSource();

        static void TestInvoke1()
        {
            ParallelOptions op = new ParallelOptions();
            op.CancellationToken = tok.Token;

            try
            {
                Parallel.Invoke(op, F1, F2, F3);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancelled!");
            }
        }

    }
}
