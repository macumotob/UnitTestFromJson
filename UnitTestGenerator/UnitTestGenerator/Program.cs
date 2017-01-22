using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
         string jsonFile = ConfigurationManager.AppSettings["jsonFileLocation"];
            Console.WriteLine("Json Source File : {0}", jsonFile);

            Generator<BeProduct.Core.DataModel.Folder.Folder> generator = new Generator<BeProduct.Core.DataModel.Folder.Folder>();
           string result =  generator.GenerateFromFile(jsonFile);

            Console.WriteLine("\r\n\tpress Q to exit ...");
            while (Console.ReadKey().Key != ConsoleKey.Q) ;
       }
    }
}
