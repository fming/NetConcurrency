using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<object> log = o => Console.WriteLine(o);
            Func<int, int, int> add = (x, y) => x + y;

            var p1 = new Person
            {
                Age = 12,
                Alive = true,
                Name = "lj",
                FavoriteFilms = new[] { "Up", "Avatar" }
            };

            var p2 = new Person() { Age = 28, Name = "cy", Child = p1 };


            var jsonString = JSON.stringify(new[] { p1, p2 });
            log(jsonString == JSON.stringify(new List<Person>() { p1, p2 }));   //true
            log(jsonString);

            Console.ReadLine();
        }       
    }

    [DataContract]
    public class Person
    {
        [DataMember(Order = 0, IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Order = 1)]
        public int Age { get; set; }

        [DataMember(Order = 2)]
        public bool Alive { get; set; }

        [DataMember(Order = 3)]
        public string[] FavoriteFilms { get; set; }

        [DataMember(Order = 4)]
        public Person Child { get; set; }
    }


    public static class JSON
    {

        public static T parse<T>(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }

        public static string stringify(object jsonObject)
        {
            using (var ms = new MemoryStream())
            {
                new DataContractJsonSerializer(jsonObject.GetType()).WriteObject(ms, jsonObject);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }



}
