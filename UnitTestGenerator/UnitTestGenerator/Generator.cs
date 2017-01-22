

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGenerator
{
    public class Generator
    {

        public static Generator Instance = new Generator();
        Module _module;
        public string GenerateFromFile(string jsonFile, Type type)
        {

            if (!System.IO.File.Exists(jsonFile))
            {
                throw new Exception(string.Format("Error : File {0} not found!", jsonFile));
            }
            string json = System.IO.File.ReadAllText(jsonFile, Encoding.UTF8);

            return Generate(json, type);
        }
        public string Generate(string json, Type type)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new Exception(string.Format("Error : json is null or empty"));
            }
            JsonParser parser = new JsonParser();
            var dic = parser.Parse(json);

            _module = type.Module;
            string s = string.Format("{0} document = ", type.FullName);

            s += _generateCode(dic, type);

            //foreach (var pi in type.GetProperties())
            //{
            //    string name = pi.Name;
            //    string type_name = pi.PropertyType.Name;
            //}

            return s + "\r\n;\r\n";
        }

        //public string Generate(string json)
        //{
        //    if (string.IsNullOrEmpty(json))
        //    {
        //        throw new Exception(string.Format("Error : json is null or empty"));
        //    }
        //    JsonParser parser = new JsonParser();
        //    var dic = parser.Parse(json);
            
        //    Type type = typeof(T);
        //    _module = type.Module;
        //    string s = string.Format("{0} document = ", type.FullName);

        //    s += _generateCode(dic,type);

        //    //foreach (var pi in type.GetProperties())
        //    //{
        //    //    string name = pi.Name;
        //    //    string type_name = pi.PropertyType.Name;
        //    //}

        //    return s + "\r\n;\r\n";
        //}
        private string _begin(int level)
        {
            string s = "";
            s.PadLeft(level);
            s += Environment.NewLine + "{" + Environment.NewLine;
            return s;
        }
        //private string _generateConstructor(Type type)
        //{
        //    if (type.BaseType.Name == "Enum")
        //    {
        //        return "";
        //    }
        //    else if (type.Name == "String")
        //    {
        //        return "";
        //    }
        //    else if (type.Name == "Boolean")
        //    {
        //        return "";
        //    }
        //    else if (type.Name == "List`1")
        //    {
        //        if (type.IsGenericType)
        //        {
        //            if(type.GenericTypeArguments != null)
        //            {
        //                Type [] g = type.GenericTypeArguments;
        //                if(g != null && g.Length == 1)
        //                {
        //                    Type t = g[0];
        //                    return string.Format(" new List<{0}> ()", t.FullName);
        //                }
        //                else
        //                {
        //                    throw new NotImplementedException();
        //                }
        //            }
        //            else
        //            {
        //                throw new NotImplementedException();
        //            }
        //        }
        //        else
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }
        //    else
        //    {
        //        return string.Format(" new {0} ()", type.FullName);
        //    }

        //}
        private bool _isSimple(Type type)
        {
            if (type == null)
            {
                return false;
            }
            if (type.IsEnum)
            {
                return true;
            }
            if (type.Name == "String")
            {
                return true;
            }
            if (type.Name == "Boolean")
            {
                return true;
            }
            if(type.Name == "Int32")
            {
                return true;
            }
            return false;
        }
        private string _simpleValue(Type type, object value)
        {
            if (type.BaseType.Name == "Enum")
            {
                return type.FullName + "." + value.ToString();
            }
            if (type.Name == "String")
            {
                return "\"" + (value == null ? "" : value.ToString() ) + "\"";
            }
            if (type.Name == "Boolean")
            {
                return value.ToString();
            }
            if (type.Name == "Int32")
            {
                return value == null ? "0" : value.ToString();
            }
            throw new NotImplementedException();
        }

        private string _pascal(string s)
        {
            int i = 0;
            string name = "";
            while(i < s.Length)
            {
                if(s[i] == '_')
                {
                    i++;
                    if( i < s.Length)
                    {
                        name += char.ToUpper(s[i]);
                    }
                    else
                    {
                        return name + "_";
                    }
                }
                else
                {
                    name += s[i];
                }
                i++;
            }
            return name;
        }
        private Type _getPropertyType(Type type,string propName, out string name)
        {
            name = propName;
            PropertyInfo pi = type.GetProperty(propName);
            if(pi == null)
            {
                name = _pascal(propName);
                pi = type.GetProperty(name);
                if(pi != null)
                {
                    return pi.PropertyType;
                }
                return null;
            }
            return pi.PropertyType;
        }
        private string _generateFromDictionary(Dictionary<string, object> dic, Type type)
        {
            string s = " new ";
            if(type.Name == "IDictionary`2")
            {
                if (type.IsGenericType)
                {
                    s += "Dictionary<";
                    int j = 0;
                    foreach(var item in type.GenericTypeArguments)
                    {
                        s += (j++ == 0 ? "" : ",") + item.FullName;
                    }
                    s += ">";
                }
                else
                {

                }
            }
            else
            {
                s += type.FullName;
            }
            s += "()\r\n{\r\n";
            int i = 0;
            string propName;
            foreach (var key in dic.Keys)
            {
                s += i++ == 0 ? "" : ",";
                object value = dic[key];
                Type propType = _getPropertyType(type,key,out propName);
                if (propType == null)
                {
                    s += string.Format("\r\n // element {0} not found", key);
                }
                else
                {
                    if (_isSimple(propType))
                    {
                        s += string.Format("\r\n{0} = {1}", propName, _simpleValue(propType, value));
                    }
                    else
                    {
                        s += string.Format("\r\n{0} = {1}", propName, _generateCode(value, propType));
                    }
                }
            }
            s += "\r\n}\r\n";
            return s;
        }
        private string _generateFromList(List<object> list,Type type)
        {
            string s = "";
            bool isGeneric = type.IsGenericType;
            string itemClassName = null;
            Type itemType = null;
            if (isGeneric)
            {
                itemClassName = type._ItemClassName();
                itemType = _module.GetType(itemClassName);
                s += " new List<" + itemClassName + ">()\r\n{\r\n";  //_generateConstructor(propertyInfo);
            }
            else
            {
                s += " new List<object>()\r\n{\r\n";
            }
            int i = 0;
            foreach (var item in list)
            {
                s += i++ == 0 ? "" : ",";
                if (isGeneric)
                {
                    s += _generateCode(item, itemType);
                }
                else
                {

                }
            }
            s += "\r\n}\r\n";
            return s;
        }
        private string _generateCode(object data, Type type)//, PropertyInfo propertyInfo = null)
        {
            Dictionary<string, object> dic1 = (data as Dictionary<string, object>);
            if(dic1 != null)
            {
                return _generateFromDictionary(data as Dictionary<string, object>, type);
            }
            List<object> list1 = (data as List<object>);
            if(list1 != null)
            {
                return _generateFromList(list1 as List<object>, type);
            }

            string itemClassName = null;
            bool isGeneric = type.IsGenericType;//propertyInfo._IsGeneric();
            if (isGeneric)
            {
                itemClassName = type._ItemClassName();
            }
            return null;
        }
    }
}
