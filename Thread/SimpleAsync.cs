using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main Thread Id :{0}", Thread.CurrentThread.ManagedThreadId);
            Test();


            //var task = Task.Run(() =>
            //{
            //    return GetName1();
            //});

            //task.GetAwaiter().OnCompleted(() =>
            //    {
            //        var name = task.Result;
            //        Console.WriteLine("My name is : {0}", name);
            //    });

            Console.WriteLine("Main thread is finished");

            Console.ReadLine();
        }

        static string GetName1()
        {
            Thread.Sleep(2000);
            return "GetName1 is done";
        }

        #region Test
        static async Task Test()
        {
            Console.WriteLine("Before calling Test, Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);

            
            await GetName();
           
            Console.WriteLine("End calling Test, Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);

           // Console.WriteLine("Get result from GetName: {0}", await name);
        }

        static async Task<string> GetName()
        {
            Console.WriteLine("Begining calling Task.Run, current thread Id is: {0}", Thread.CurrentThread.ManagedThreadId);

            await Task.Delay(1000);

            Console.WriteLine("Before calling Task.Run, current thread Id is: {0}", Thread.CurrentThread.ManagedThreadId);

            return await Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("'GetName' Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);
                return "Jesse";
            });
        }
        #endregion


    }
}