using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            new Thread(Go).Start("Arg1"); // no Anonymous delegate

            new Thread(delegate() { GoGoGo("args1", "args2", "args3"); }); // after we have anoymous delegate

            Task.Run(() =>
                {
                    GoGoGo("args1", "args2", "args3");
                });
            
        }

        static void Go(object name)
        {
            Console.WriteLine("Go : {0}", name);
        }

        static void GoGoGo(string a1, string a2, string a3)
        {
            Console.WriteLine("GoGoGo: {0}, {1}, {2}", a1, a2, a3);
        }
    }
}