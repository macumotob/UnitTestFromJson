using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGenerator
{
    class CodeListInfo
    {
        public string ListTypeName { get; set; }
        public string ItemTypeName { get; set; }
        public Type ItemType { get; set; }
        private List<string> _typeNames = null;

        public List<CodeItems> _items = new List<CodeItems>();
        private List<string> _GetGenericTypeList(Type pi)
        {
            Type tmp = pi;
            List<string> names = new List<string>();
            int i;
            while (tmp != null)
            {
                i = tmp.FullName.IndexOf('`');
                string nm = i == -1 ? tmp.FullName : tmp.FullName.Substring(0, i);
                names.Add(nm);
                if (tmp.GenericTypeArguments == null)
                {
                    break;
                }
                Type[] g = tmp.GenericTypeArguments;
                if (g == null || g.Length == 0)
                {
                    break;
                }
                if (g.Length == 1)
                {
                    tmp = g[0];
                }
                else
                {
                    string s = "";
                    for (int j = 0; j < g.Length; j++)
                    {
                        s += j > 0 ? "," : "";
                        i = g[j].FullName.IndexOf('`');
                        s += i == -1 ? g[j].FullName : g[j].FullName.Substring(0, i);
                    }
                    names.Add(s);
                    tmp = null;
                }
            }

            return names;
        }
        private string _MakeGenericClassName(List<string> names)
        {
            //            names.RemoveAt(0);
            string s = "";
            for (int i = 0; i < names.Count; i++)
            {
                s += i == names.Count - 1 ? names[i] : names[i] + "<";
            }
            for (int i = 0; i < names.Count; i++)
            {
                s += i == names.Count - 1 ? "" : ">";
            }
            return s;
        }

        public void Parse(Type type, Module module)
        {
            bool isGeneric = type.IsGenericType;
            if (isGeneric)
            {
                _typeNames = _GetGenericTypeList(type);
                ListTypeName = _MakeGenericClassName(_typeNames);

                //itemType = _module.GetType(itemClassName);
                ItemType = module.GetType(_typeNames[1]);
                if (ItemType == null)
                {

                }
                _typeNames.RemoveAt(0);
                ItemTypeName = _MakeGenericClassName(_typeNames);
            }
        }
    }

    class CodeItems
    {
        public static CodeItems Instance = new CodeItems();

        private List<CodeItem> _items = new List<CodeItem>();
        private List<Assembly> _assemblies = new List<Assembly>();

        public Type TargetType;
        public void RegisterAssembly(string dllFileName)
        {
            Assembly ass = Assembly.LoadFile(dllFileName);
            _assemblies.Add(ass);

            //            Type type = ass.GetType(typeName);
        }
        protected CodeItems Clone()
        {
            CodeItems x = new CodeItems();
            _assemblies.ForEach(ass =>
           {
               x._assemblies.Add(ass);
           });
            return x;
        }
        public bool HasUndefinedTypes()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].PropertyType == null)
                {
                    return true;
                }
            }
            return false;
        }

        public Type FindType(string typeName)
        {
            for (int i = 0; i < _assemblies.Count; i++)
            {
                Type type = _assemblies[i].GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }
        private CodeItem _Add(string elementName, string objectPropertyName, Type propertyType, object value)
        {
            CodeItem item = new CodeItem()
            {
                ElementName = elementName,
                ObjectPropertyName = objectPropertyName,
                PropertyType = propertyType,
                Value = value
            };
            _items.Add(item);
            return item;
        }
        protected void _ParseList(CodeItem item)
        {
            CodeListInfo info = new CodeListInfo();
            info.Parse(item.PropertyType, _assemblies[0].Modules.First());
            //Type listItemType = FindType(listInfo.ItemTypeName);
            List<object> list = item.Value as List<object>;
            if (info.ItemTypeName == "System.String")
            {
                item.Value = list;
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    object listItem = list[i];
                    CodeItems x1 = this.Clone();
                    if (listItem is Dictionary<string, object>)
                    {
                        x1._Parse(listItem as Dictionary<string, object>, info.ItemType);
                        info._items.Add(x1);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            item.Value = info;
        }
        protected void _Parse(Dictionary<string, object> dic, Type type)
        {
            string propName = null;
            object value = null;
            foreach (var elementName in dic.Keys)
            {
                value = dic[elementName];
                Type propType = Generator.GetPropertyType(type, elementName, out propName);
                CodeItem item = _Add(elementName, propName, propType, value);
                if (item.IsPrimitive)
                {
                }
                else
                {
                    CodeItems x = this.Clone();
                    if (item.Value is Dictionary<string, object>)
                    {
                        x._Parse(item.Value as Dictionary<string, object>, item.PropertyType);
                        item.Value = x;
                    }
                    else if (item.Value is List<object>)
                    {
                        _ParseList(item);
                    }
                    else if (item.Value == null || item.Value.ToString() == "null")
                    {

                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                }
            }
        }
        public void Parse(string json, Type type)
        {
            TargetType = type;
            if (string.IsNullOrEmpty(json))
            {
                throw new Exception(string.Format("Error : json is null or empty"));
            }
            JsonParser parser = new JsonParser();
            var dic = parser.Parse(json);
            _Parse(dic, type);
        }
        protected void _GenerateCode(string docName, StreamWriter sw)
        {
            bool isExpando = HasUndefinedTypes();
            if (isExpando)
            {
                sw.WriteLine("// EXPANDO OBJECT");
            }
            foreach (CodeItem item in _items)
            {
                if (item.IsPrimitive)
                {
                    sw.WriteLine("  " + docName + "." + item.ObjectPropertyName + " = " + item.SimpleValue() + ";");
                }
            }
            foreach (CodeItem item in _items)
            {
                if (item.IsPrimitive)
                {
                    continue;
                }
                string name = docName + "." + item.ObjectPropertyName;
                string code = "// Code for :  " + name + " as  " + (item.PropertyType == null ? " NULL" : item.PropertyType.FullName);
                if (item.Value as CodeListInfo != null)
                {
                    CodeListInfo info = item.Value as CodeListInfo;

                    code = "\r\n   " + name + " = new " + info.ListTypeName + "();";
                    sw.WriteLine(code);
                    for (int i = 0; i < info._items.Count; i++)
                    {
                        sw.WriteLine("\r\n  {");
                        code = "  " + info.ItemTypeName + " x = new " + info.ItemTypeName + "();";
                        sw.WriteLine(code);
                        info._items[i]._GenerateCode("x", sw);
                        sw.WriteLine("  " + name + ".Add(x);");
                        sw.WriteLine("\r\n  }");
                    }

                }
                else if (item.Value as CodeItems != null)
                {
                    CodeItems x = item.Value as CodeItems;
                    if (x.HasUndefinedTypes())
                    {
                        string expName = "tmp";
                        code = "\r\n  " + name + " = new System.Dynamic.ExpandoObject();";
                        sw.WriteLine(code);
                        sw.WriteLine("  {");
                        sw.WriteLine("    dynamic " + expName +" = " + name + ";");
                        x._GenerateCode(expName, sw);
                        sw.WriteLine("  }");
                    }
                    else
                    {
                        code = "\r\n  " + name + " = new " + (item.PropertyType == null ? " NULL" : item.PropertyType.FullName) + "();";
                        sw.WriteLine(code);
                        x._GenerateCode(name, sw);
                    }
                }
                else
                {
                    if (item.Value == null || item.Value.ToString() == "null")
                    {
                        code = "  " + name + " = null;";
                        sw.WriteLine(code);
                    }
                    else
                    {
                        sw.WriteLine(code);
                    }
                }


            }
        }
        public void GenarateFile(string fullFileName, string namespaceName)
        {

            if (TargetType == null)
            {
                return;
            }
            using (StreamWriter sw = new StreamWriter(fullFileName))
            {

                string documentName = "doc";

                sw.WriteLine("//  UnitTest class : " + TargetType.FullName);
                sw.WriteLine("using System;");
                sw.WriteLine("using System.Collections.Generic;");
                sw.WriteLine("using System.Linq;");
                sw.WriteLine("using System.Text;");
                sw.WriteLine("using System.Threading.Tasks;");
                sw.WriteLine("using Microsoft.VisualStudio.TestTools.UnitTesting;");

                sw.WriteLine();
                sw.WriteLine("namespace " + namespaceName);
                sw.WriteLine("{");
                //sw.WriteLine("[TestClass]");
                sw.WriteLine("public partial class Test" + TargetType.Name);
                sw.WriteLine("{");
                sw.WriteLine(" public " + TargetType.FullName + " CreateInstance()");
                sw.WriteLine("{");
                sw.WriteLine(" " + TargetType.FullName + " " + documentName  + " = new " + TargetType.FullName + "();");

                _GenerateCode(documentName, sw);

                sw.WriteLine("  return doc;");
                sw.WriteLine("}");
                sw.WriteLine("} //end of class");
                sw.WriteLine("} //end of namespace");
                sw.Close();
                sw.Dispose();
            }
        }
    }
}

