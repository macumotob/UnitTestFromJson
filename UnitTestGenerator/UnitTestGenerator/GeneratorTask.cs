using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGenerator
{
    public class GeneratorTask
    {
        public string TypeName { get; set; }
        public string OutputFile { get; set; }
        public string JsonFile { get; set; }
        public bool BreakIfExists { get; set; }
        public void Parse(Dictionary<string, object> dic)
        {
            Type type = this.GetType();
            foreach(string key in dic.Keys)
            {
                var p = type.GetProperty(key);
                if (p.PropertyType.Name == "Boolean")
                {
                    p.SetValue(this, bool.Parse(dic[key].ToString()));
                }
                else
                {
                    p.SetValue(this, dic[key]);
                }
            }
        }
    }
    public class GeneratorTasks
    {
        private List<object> _assemblies { get; set; }
        private List<Assembly> _modules;
        public List<Assembly> Modules
        {
            get
            {
                if(_modules == null)
                {
                    _modules =  new List<Assembly>();
                }
                return _modules;
            }
            set
            {
                _modules = value;
            }
        }
        public string OutPutFolder { get; set; }
        public string NameSpace { get; set; }

        public void Parse(Dictionary<string, object> dic)
        {
            _assemblies = (List<object>)dic["Assemblies"];
            OutPutFolder = (string)dic["OutPutFolder"];
            _LoadModules();

            NameSpace = (string)dic["NameSpace"];
            OutPutFolder = OutPutFolder.Replace("{$App}", AppDomain.CurrentDomain.BaseDirectory);
            if (!Directory.Exists(OutPutFolder))
            {
                Directory.CreateDirectory(OutPutFolder);
            }
            List<object>  tasks =(List<object>)dic["Tasks"];
            _LoadTasks(tasks);
        }
        private void _LoadModules()
        {
            foreach(var file in _assemblies)
            {
                Assembly ass = Assembly.LoadFile(file.ToString());
                Modules.Add(ass);
            }
        }
        private void _LoadTasks(List<object> list)
        {
            list.ForEach(item => {
                Dictionary<string, object> dic = (Dictionary<string, object>)item;
                GeneratorTask task = new GeneratorTask();
                task.Parse(dic);
                _Generate(task);
            });
        }
        private Type _FindType(string typeName)
        {
            Type type = null;
            _modules.ForEach(m => {
                if (type == null) { 
                    type = m.GetType(typeName);
                }
            });
            return type;
        }
        private void _Generate(GeneratorTask task)
        {
            Type type = _FindType(task.TypeName);
            string output = OutPutFolder + "\\" + task.OutputFile;
            if(File.Exists(output) && task.BreakIfExists)
            {
                return;
            }
            string sourceFile = AppDomain.CurrentDomain.BaseDirectory + "\\" + task.JsonFile;
            if (!File.Exists(sourceFile))
            {
                return;
            }
            
            StreamWriter sw = new StreamWriter(output);

            if(type != null)
            {
                sw.WriteLine("//  UnitTest class : " + type.FullName);
                sw.WriteLine("using System;");
                sw.WriteLine("using System;");
                sw.WriteLine("using System.Collections.Generic;");
                sw.WriteLine("using System.Linq;");
                sw.WriteLine("using System.Text;");
                sw.WriteLine("using System.Threading.Tasks;");
                sw.WriteLine("using Microsoft.VisualStudio.TestTools.UnitTesting;");

                sw.WriteLine();
                sw.WriteLine("namespace " + NameSpace);
                sw.WriteLine("{");
                sw.WriteLine("[TestClass]");
                sw.WriteLine("public partial class Test" + type.Name);
                sw.WriteLine("{");
                sw.WriteLine(" public " + type.FullName + " CreateInstance()");
                sw.WriteLine("{");


                string body = Generator.Instance.GenerateFromFile(sourceFile, type);
                sw.Write(body);
                sw.WriteLine("; return document;");
                sw.WriteLine("}");
                sw.WriteLine("} //end of class");
                sw.WriteLine("} //end of namespace");
            }
            else
            {
                sw.WriteLine("//  UnitTest class ");
                sw.WriteLine("// type " + task.TypeName + "NOT FOUND");
            }
            sw.Close();
            sw.Dispose();
        }
    }
}
