using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGenerator
{

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
                        info.Items.Add(x1);
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
                Type propType = Utils.GetPropertyType(type, elementName, out propName);

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
                    for (int i = 0; i < info.Items.Count; i++)
                    {
                        sw.WriteLine("\r\n  {");
                        code = "  " + info.ItemTypeName + " x = new " + info.ItemTypeName + "();";
                        sw.WriteLine(code);
                        info.Items[i]._GenerateCode("x", sw);
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
                        if (x._items.Count == 0)
                        {
                            code = "\r\n  " + name + " = null;";
                        }
                        else
                        {
                            code = "\r\n  " + name + " = new " + (item.PropertyType == null ? " NULL" : item.PropertyType.FullName) + "();";
                        }
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

