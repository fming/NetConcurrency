using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLINQDemo
{
    static class PartitionTest
    {

        /// <summary>
        /// There are four kinds of partitioning:
        /// 1 range partitioning: be used for indexable source
        /// 2 chunk partitioning: can be used for any source
        /// </summary>

        public static void Test()
        {
            StreamReader reader = new System.IO.StreamReader(".\\data\\words.txt");
            string[] words = reader.ReadToEnd().Split(' ');
            Console.WriteLine("Loading data finished, press enter key to execute parallel");
            Console.ReadLine();
            Console.WriteLine("There are {0} words in all.", words.Length);

            SumTest(words);


            Console.ReadLine();
        }

        static void SumTest(string[] words)
        {

            var query = (from word in words.AsParallel()
                         where (word.Contains('a'))
                         select CountLetters(word)).Sum();

            Console.WriteLine("There are {0} words that contains a in all",query);
        }


        static int CountLetters(string key)
        {
            return 1;
            //int n = 0;
            //for (int i = 0; i < key.Length; i++)
            //{
            //    if (Char.IsLetter(key, i))
            //    {
            //        n++;
            //    }
            //}

            //return n;
        }



    }
}
