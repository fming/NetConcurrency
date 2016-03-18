using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //!! [1:] 
            Console.WriteLine("Main Thread Id :{0}", Thread.CurrentThread.ManagedThreadId);
            Test(); 
            Console.ReadLine();
        }

        static string GetName1()
        {
            Thread.Sleep(2000);
            return "GetName1 is done";
        }

        static async Task Test()
        {
            //! [2:]
            Console.WriteLine("Before calling Test, Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);

            await GetName();
            
            //!! [3:] why here the thread is not equal to thread [2:]
            Console.WriteLine("End calling Test, Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);
            
        }

        static async Task<string> GetName()
        {
            await Task.Delay(1000);

            Console.WriteLine("GetName Done");
            return "Hello";
        }
        
    }
}
