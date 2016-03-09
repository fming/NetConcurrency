using System;
using System.Threading;
using System.Threading.Tasks;
namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();

        }

        public static async void Test()
        {
            new Thread(Go).Start(); // .NET 1.0
            Task.Factory.StartNew(Go); // .NET 4.0 introduce the TPL
            await Task.Run(new Action(Go)); // .NET 4.5 add new "Run" function

        }

        public static void Go()
        {
            Console.WriteLine("I am a thread");
        }

    }
}
