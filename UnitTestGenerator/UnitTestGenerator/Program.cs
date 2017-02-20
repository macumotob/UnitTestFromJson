using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\data\\generatorTask.json";
            if (System.IO.File.Exists(file))
            {
                string json = System.IO.File.ReadAllText(file, Encoding.UTF8);

                JsonParser parser = new JsonParser();
                Dictionary<string, object> dic = parser.Parse(json);
                GeneratorTasks tasks = new GeneratorTasks();
                tasks.Parse(dic);
                return;
            }
            Console.WriteLine("ERROR! FILE : " + file + " NOT FOUND!!!");
            Console.WriteLine("\r\n\tpress Q to exit ...");
            while (Console.ReadKey().Key != ConsoleKey.Q) ;
        }
    }
}
