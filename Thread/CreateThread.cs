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
        }

        public static void Go()
        {
            Console.WriteLine("I am a thread");
        }

    }
}
