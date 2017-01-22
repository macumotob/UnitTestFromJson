

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGenerator
{
    public class Generator<T>
    {

        public string GenerateFromFile(string jsonFile)
        {

            if (!System.IO.File.Exists(jsonFile))
            {
                throw new Exception(string.Format("Error : File {0} not found!", jsonFile));
            }
            string json = System.IO.File.ReadAllText(jsonFile, Encoding.UTF8);

            return Generate(json);
        }
        public string Generate(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new Exception(string.Format("Error : json is null or empty"));
            }
            JsonParser parser = new JsonParser();
            var dic = parser.Parse(json);
            Type type = typeof(T);

            return null;
        }
    }
}
