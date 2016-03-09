using System;
using System.Threading;
using System.Collections.Generic;
namespace ConsoleApplication3
{
    class Program
    {

        static void Main(string[] args)
        {

        }

        static void Test()
        {
            double value = 10;
            bool b = value.CompareTo(16) > 0;
        }

        static void Test1()
        {
            double value = 10;
            bool b = value > 16;
        }

    }
}