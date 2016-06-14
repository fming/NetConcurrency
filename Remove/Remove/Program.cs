using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remove
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                return;
            }


            Console.WriteLine(args[0]);

            string path = args[0];
            path = path.Replace('\"',' ');
            Console.ReadKey();
            string[] filePaths = Directory.GetFiles(path);

            try
            {
                if (filePaths != null)
                {
                    foreach (string filePath in filePaths)
                        File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }


            Console.WriteLine("removeing finished");
            Console.ReadKey();

        }
    }
}
