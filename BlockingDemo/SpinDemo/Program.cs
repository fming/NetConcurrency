using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpinDemo
{
    class Program
    {
        static int Data = 1;
        static void Main(string[] args)
        {
            for (int i = 0; i < 20; i++)
            {
                Thread t = new Thread(SpinLockTest);
                t.Start();
            }
            Console.ReadKey();
            Console.WriteLine(Data);
            Console.ReadKey();        
        }

        static void SpinLockTest()
        {
            var spinLock = new SpinLock(true);//enable owener tracking

            // you must follow  the robust pattern of providing a lockTaken argument
            bool lockTaken = false;
            try
            {
                //spinLock.Enter(ref lockTaken);
                // Do stuff
                Data = Data + Data + Data;
                Data = Data / 3 ;
                Data = Data + Data + Data;
                Data = Data / 3;
                Data = Data + Data + Data;
                Data = Data / 3;
                Data = Data + Data + Data;
                Data = Data / 3;

                Console.WriteLine("--{0}", Data);
            }
            finally
            {
                if (lockTaken) spinLock.Exit();
            }
        }





        
                
    }
}
