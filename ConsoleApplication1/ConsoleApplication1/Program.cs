using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Parallel test------------------------------");
            Parallel.Invoke(
                () =>
                {
                    ConvertEllipses();
                },
                () =>
                {
                    ConvertRectangles();
                },
                delegate ()
                {
                    ConvertLines();
                },
                delegate ()
                {
                    ConvertText();
                });


            // Sequence
            Console.WriteLine("Start Sequence test----------------------------");
            ConvertEllipses();
            ConvertRectangles();
            ConvertLines();
            ConvertText();

            Console.ReadLine();

        }

        static void ConvertEllipses()
        {
            Console.WriteLine("1 Ellipses converted.");
        }

        static void ConvertRectangles()
        {
            Console.WriteLine("2 Rectangle converted.");
        }

        static void ConvertLines()
        {
            Console.WriteLine("3 Lines converted.");
        }

        static void ConvertText()
        {
            Console.WriteLine("4 Text converted.");
        }
    }
}
