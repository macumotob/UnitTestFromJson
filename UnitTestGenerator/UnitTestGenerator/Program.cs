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
        static void Calculate()
        {
            double e = 0.15;
            //e = 0.25;

            int n = 1;
            double N = 1.0;
            int count = 10;
            int a = 0;
            int b = 0;
            while (count > 0)
            {
                while (true)// N % 2 != 0)
                {
                    a = n;
                    b = a;

                    var a1 = n * e;
                    var b1 = a1;

                    var x1 = (2 * a1) + b1;
                    var x2 = a1 + (2 * b1);



                    if (N % 2 == 0)
                    {
                        break;
                    }
                    N = (3 * e * n + 6) / e;
                    n++;
                }

                Console.WriteLine("a:{0}, b: {1} N:{2} e:{3} X:{4} Width : {5}", a, b, N, e, N/2, N*e );
                if(N*e > 11.0)
                {
                    break;
                }
                N++;
                count--;
            }
        }
        private static void Test()
        {
            string dll = @"C:\BP\BeProduct.Core\BeProduct.Core.UnitTest\bin\Debug\BeProduct.Core.dll";
            CodeItems.Instance.RegisterAssembly(dll);
            string typeName = @"BeProduct.Core.DataModel.LineSheet.LineSheetFolder";

            //Assembly ass = Assembly.LoadFile(dll);
            Type type = CodeItems.Instance.FindType(typeName);

            string file = AppDomain.CurrentDomain.BaseDirectory + "\\data\\lineSheetFolder1.json";
            string json = System.IO.File.ReadAllText(file, Encoding.UTF8);

            string outputFile = @"C:\BP\BeProduct.Core\BeProduct.Core.UnitTest\" + "LineSheetFolderPart.cs";

            CodeItems.Instance.Parse(json, type);
            CodeItems.Instance.GenarateFile(outputFile, "BeProduct.Core.UnitTest");
        }
        static void Main(string[] args)
        {
            if (true)
            {
                Test();
                return;
            }
            //Fract.Test();
            //return;
            //Calculate();
            //return;
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\data\\generatorTask.json";

            if (System.IO.File.Exists(file))
            {
                string json = System.IO.File.ReadAllText(file, Encoding.UTF8);

                JsonParser parser = new JsonParser();
                Dictionary<string, object> dic = parser.Parse(json);

             //   GeneratorTasks x = (GeneratorTasks )Creator.Instance.Create(typeof(GeneratorTasks), dic);
             //   GeneratorTasks x2 = (GeneratorTasks)Creator.Instance.Create( AppDomain.CurrentDomain.BaseDirectory + "\\UnitTestGenerator.exe", "UnitTestGenerator.GeneratorTasks", dic);

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
