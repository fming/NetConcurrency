using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PLINQDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            PartitionTest.Test();
            //OrderTest.Test();

            //CalculationPrime();
            //UpcaseString();
            //SpellCheck();

            //PingDemo();

            Console.WriteLine("Waiting......");
            Console.ReadLine();

        }

        static void CalculationPrime()
        {
            IEnumerable<int> numbers = Enumerable.Range(3, 100000 - 3);

            var parallelQuery =
              from n in numbers.AsParallel()
              where Enumerable.Range(2, (int)Math.Sqrt(n)).All(i => n % i > 0)
              select n;

            int[] primes = parallelQuery.ToArray();

            foreach (int prime in primes)
            {
                Console.Write(" " + prime);
            }
        }

        // Can't make ordered
        static void UpcaseString()
        {
            var result = "abcdef".AsParallel().Select(c => char.ToUpper(c)).ToArray();
            foreach (char ch in result)
            {
                Console.Write(" " + ch);
            }
        }

        static void SpellCheck()
        {
            if (!File.Exists("WordLookup.txt"))    // Contains about 150,000 words
                new WebClient().DownloadFile(
                  "http://www.albahari.com/ispell/allwords.txt", "WordLookup.txt");

            var wordLookup = new HashSet<string>(
              File.ReadAllLines("WordLookup.txt"),
              StringComparer.InvariantCultureIgnoreCase);


            var random = new Random();
            string[] wordList = wordLookup.ToArray();

            string[] wordsToTest = Enumerable.Range(0, 1000000)
              .Select(i => wordList[random.Next(0, wordList.Length)])
              .ToArray();

            wordsToTest[12345] = "woozsh";     // Introduce a couple
            wordsToTest[23456] = "wubsie";     // of spelling mistakes.

            Stopwatch sw = new Stopwatch();

            sw.Start();
            var query = wordsToTest.Select((word, index) => new IndexedWord { Word = word, Index = index })
                .Where(iword => !wordLookup.Contains(iword.Word))
                .OrderBy(iword => iword.Index);

            //        var query = wordsToTest.AsParallel().Select((word, index) => new IndexedWord { Word = word, Index = index })
            //.Where(iword => !wordLookup.Contains(iword.Word))
            //.OrderBy(iword => iword.Index);

            Console.WriteLine("done " +
                  sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();
            sw.Start();

            foreach (var item in query)
            {
                Console.WriteLine(item.Word + " " + item.Index);
            }

            Console.WriteLine("done " +
                  sw.Elapsed.TotalMilliseconds.ToString());

            sw.Reset();
        }


        static void PingDemo()
        {
            var query = from site in new[]
            {
                "www.albahari.com",
                "www.linqpad.net",
                "www.oreilly.com",
                "www.takeonit.com",
                "stackoverflow.com",
                "www.rebeccarey.com"
            }
            .AsParallel().WithDegreeOfParallelism(6)
                        let p = new Ping().Send(site)
                        select new
                        {
                            site,
                            Result = p.Status,
                            Time = p.RoundtripTime
                        };

            foreach (var data in query)
            {
                Console.WriteLine("site:" + data.site + " Result:" + data.Result + " Time:" + data.Time);
            }
        }
    }

    internal class IndexedWord
    {
        public int Index { get; set; }
        public string Word { get; set; }
    }
}
