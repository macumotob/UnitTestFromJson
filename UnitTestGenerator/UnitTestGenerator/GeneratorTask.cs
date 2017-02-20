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
            foreach (string key in dic.Keys)
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
        public List<object> AssemblyNames
        {
            get
            {
                if (_assemblies == null)
                {
                    _assemblies = new List<object>();
                }
                return _assemblies;
            }
            set
            {
                _assemblies = value;
            }
        }
        private List<Assembly> _modules;
        public List<Assembly> Modules
        {
            get
            {
                if (_modules == null)
                {
                    _modules = new List<Assembly>();
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
            List<object> tasks = (List<object>)dic["Tasks"];
            _LoadTasks(tasks);
        }
        private void _LoadModules()
        {
            foreach (var file in _assemblies)
            {
                Assembly ass = Assembly.LoadFile(file.ToString());
                Modules.Add(ass);
            }
        }
        private void _Generate(GeneratorTask task)
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + task.JsonFile;
            if (!System.IO.File.Exists(file))
            {
                throw new Exception("File not found : " + file);
            }
            string json = System.IO.File.ReadAllText(file, Encoding.UTF8);
            string outputFile = this.OutPutFolder + task.OutputFile;

            if (true)
            {
                CodeItems codes = new CodeItems();
                _assemblies.ForEach(item => codes.RegisterAssembly((string)item));
                Type type = codes.FindType(task.TypeName);
                codes.Parse(json, type);
                codes.GenarateFile(outputFile, this.NameSpace);// task.TypeName);
            }
            else
            {
                _assemblies.ForEach(item => CodeItems.Instance.RegisterAssembly((string)item));
                Type type = CodeItems.Instance.FindType(task.TypeName);
                CodeItems.Instance.Parse(json, type);
                CodeItems.Instance.GenarateFile(outputFile, this.NameSpace);// task.TypeName);
            }

        }
        private void _LoadTasks(List<object> list)
        {
            list.ForEach(item =>
            {
                Dictionary<string, object> dic = (Dictionary<string, object>)item;
                GeneratorTask task = new GeneratorTask();
                task.Parse(dic);
                _Generate(task);
                //_Generate(task);
            });
        }
        private Type _FindType(string typeName)
        {
            Type type = null;
            _modules.ForEach(m =>
            {
                if (type == null)
                {
                    type = m.GetType(typeName);
                }
            });
            return type;
        }
    }
}
