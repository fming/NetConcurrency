using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var dayName = Task.Run<string>(() => { return GetDayOfThisWeek(); });
            Console.WriteLine("Today is : {0}", dayName.Result);
        }

        static string GetDayOfThisWeek()
        {
            return "saturday";
        }
    }
}