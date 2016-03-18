using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//namespace MutexProcessDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            //A Mutex can be released only from the same thread that obtained it.
//            var mutex = new Mutex(false, "cross_process_mutex");
//            Console.WriteLine("Waiting!!!!");
//            if (mutex.WaitOne())
//            {
//            }

//            RunProgram();
//            mutex.ReleaseMutex();

//            Console.WriteLine("Already Release mutex");

//        }

//        static void RunProgram()
//        {
//            Console.WriteLine("Running. Press Enter to release the mutex");
//            Console.ReadLine();
//        }

//    }
//}

class OneAtATimePlease
{
    static void Main()
    {
        // Naming a Mutex makes it available computer-wide. Use a name that's
        // unique to your company and application (e.g., include your URL).
        using (var mutex = new Mutex(false, "oreilly.com OneAtATimeDemo"))
        {
            // Wait a few seconds if contended, in case another instance
            // of the program is still in the process of shutting down.

            if (!mutex.WaitOne(TimeSpan.FromSeconds(3), false))
            {
                Console.WriteLine("Another app instance is running. Bye!");
                Thread.Sleep(4000);

                return;
            }
            RunProgram();
        }
    }

    static void RunProgram()
    {
        Console.WriteLine("Running. Press Enter to exit");
        Console.ReadLine();
    }
}
