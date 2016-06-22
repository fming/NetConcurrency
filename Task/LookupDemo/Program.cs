using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//http://developer.51cto.com/art/201104/252816.htm
namespace LookupDemo
{
    public sealed class Product
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public double Value { get; set; }
        public override string ToString() { return string.Format("[{0}: {1} - {2}]", Id, Category, Value); }
    }



    class Program
    {

        public static List<Product> GetList()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Category = "Electronics", Value = 15.0 },
                new Product { Id = 2, Category = "Groceries", Value = 40.0 },
                new Product { Id = 3, Category = "Garden", Value = 210.3 },
                new Product { Id = 4, Category = "Pets", Value = 2.1 },
                new Product { Id = 5, Category = "Electronics", Value = 19.95 },
                new Product { Id = 6, Category = "Pets", Value = 21.25 },
                new Product { Id = 7, Category = "Pets", Value = 5.50 },
                new Product { Id = 8, Category = "Garden", Value = 13.0 },
                new Product { Id = 9, Category = "Automotive", Value = 10.0 },
                new Product { Id = 10, Category = "Electronics", Value = 250.0 },
            };

            return products;
        }


        static void Main(string[] args)
        {
            var products = GetList();

            //我们有一个任务就是按类别列出一个物品清单


            // Verion 1: use GroupBy
            foreach (var group in products.GroupBy(p => p.Category))
            {
                Console.WriteLine(group.Key);
                foreach (var item in group)
                {
                    Console.WriteLine("\t" + item);
                }
            }

            // Version 2: the data is not update, 延迟技术，导致后面的输出没有Garden了
            var groups = products.GroupBy(p => p.Category);
            //删除所有属于Garden的产品  
            //products.RemoveAll(p => p.Category == "Garden");
            //foreach (var group in groups)
            //{
            //    Console.WriteLine(group.Key);
            //    foreach (var item in group)
            //    {
            //        Console.WriteLine("\t" + item);
            //    }
            //}


            // Version 3: 
            var productsByCategory = products.ToLookup(p => p.Category);
            products.RemoveAll(p => p.Category == "Garden");
            foreach (var group in productsByCategory)
            {
                // the key of the lookup is in key property    
                Console.WriteLine(group.Key);
                // the list of values is the group itself.     
                foreach (var item in group)
                {
                    Console.WriteLine("\t" + item);
                }
            }

            Console.ReadLine();
        }
    }
}
