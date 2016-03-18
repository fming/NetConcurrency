using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LockingDemo
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    class ThreadUnSafe
    {
        static int _value1 = 1, _value2 = 1;

        static void Go()
        {
            if (_value2 != 0)
                Console.WriteLine(_value1 / _value2);
            _value2 = 0;
        }
    }

    class ThreadSafe
    {
        static int _value1 = 1, _value2 = 1;
        static readonly object _locker = new object();

        static void Go()
        {
            lock (_locker)
            {
                if (_value2 != 0)
                    Console.WriteLine(_value1 / _value2);
                _value2 = 0;
            }
        }

    
        // C#1 , C#2, C#3
        static void Go1()
        {
            Monitor.Enter(_locker);
            // exception here

            try
            {
                if (_value2 != 0)
                    Console.WriteLine(_value1 / _value2);
                _value2 = 0;
            }
            finally
            {
                Monitor.Exit(_locker);
            }
        }

        // more strong (lock will be translated to like)
        static void Go2()
        {
            bool lockTaken = false;
            try
            {
                // C# 4.0 
                Monitor.Enter(_locker, ref lockTaken);
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_locker);
                }
            }
        }



    }
}

