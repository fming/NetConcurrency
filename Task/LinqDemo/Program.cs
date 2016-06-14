using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //int result = Sample1();
            //Console.WriteLine(result);

            Sample3();


            Console.ReadKey();
        }

        private static int Sample1()
        {
            List<int> arr = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };

            // Linq is collection of extend methods: like Where, Max, Select, Sum, Any, Average, All, Contact...
            var result = arr.Where(a => { return a > 3; }).Sum();
            // equal with this:
            //result = (from v in arr where v > 3 select v).Sum();


            return result;
        }


        private static void Sample2()
        {
            var keyPairs = new string[6];

            Parallel.For(0, keyPairs.Length,
                          i => keyPairs[i] = RSA.Create().ToXmlString(true));

            string[] keyPairs1 =  ParallelEnumerable.Range(0,6).Select(i => RSA.Create().ToXmlString(true)).ToArray();
        }

        //Optimzing PLINQ
        // if you don't care about the order in which the elements are processed
        private static void Sample3()
        {
            //foreach (int n in parallelQuery)
            //    DoSomething(n);

            "abcdef".AsParallel().Select(c => char.ToUpper(c)).ForAll(Console.Write);
        }
    }
}
