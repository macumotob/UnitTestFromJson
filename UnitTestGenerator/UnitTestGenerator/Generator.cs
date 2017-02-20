

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
        //internal Module _module;
        //public string GenerateFromFile(string jsonFile, Type type)
        //{

        //    if (!System.IO.File.Exists(jsonFile))
        //    {
        //        throw new Exception(string.Format("Error : File {0} not found!", jsonFile));
        //    }
        //    string json = System.IO.File.ReadAllText(jsonFile, Encoding.UTF8);

        //    return Generate(json, type);
        //}
        //public string Generate(string json, Type type)
        //{
        //    if (string.IsNullOrEmpty(json))
        //    {
        //        throw new Exception(string.Format("Error : json is null or empty"));
        //    }
        //    JsonParser parser = new JsonParser();
        //    var dic = parser.Parse(json);

        //    _module = type.Module;
        //    string s = string.Format("{0} document = ", type.FullName);

        //    s += _generateCode(dic, type);

        //    return s + "\r\n\r\n";
        //}

        //private bool _isSimple(Type type)
        //{
        //    string[] simpleTypes = { "System.String", "System.Int32", "System.Int64" , "System.Boolean" };
        //    string[] complexNames = { "IDictionary`2", "List`1" ,"ExpandoObject"};
        //    if (simpleTypes.Contains(type.FullName))
        //    {
        //        return true;
        //    }
        //    if (complexNames.Contains(type.Name))
        //    {
        //        return false;
        //    }
        //    if (type.IsEnum)
        //    {
        //        return true;
        //    }
        //    if (type == null)
        //    {
        //        return false;
        //    }
        //    if(type.Name == "IDictionary`2" || type.Name == "List`1")
        //    {
        //        return false;
        //    }
        //    if (type.Name == "Nullable`1")
        //    {
        //        if (type.GenericTypeArguments != null && type.GenericTypeArguments.Length == 1)
        //        {
        //            if (type.GenericTypeArguments[0].Name == "DateTime")
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    var x = this._module.Assembly.GetType(type.FullName);
        //    if(x == null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        //private string _simpleValue(Type type, object value)
        //{
        //    if (type.IsEnum)
        //    {
        //        object x = Enum.Parse(type, value.ToString());
        //        return type.FullName + "." + x.ToString();
        //    }
        //    if (type.Name == "String")
        //    {
        //        return "\"" + (value == null ? "" : value.ToString() ) + "\"";
        //    }
        //    if (type.Name == "Boolean")
        //    {
        //        return value.ToString();
        //    }
        //    if (type.Name == "Int32")
        //    {
        //        return value == null ? "0" : value.ToString();
        //    }
        //    if (type.Name == "Int64")
        //    {
        //        return value == null ? "0" : value.ToString();
        //    }
        //    if (type.Name == "Int64")
        //    {
        //        return value == null ? "0" : value.ToString();
        //    }
        //    if(type.Name == "Nullable`1")
        //    {
        //        if(type.GenericTypeArguments != null && type.GenericTypeArguments.Length == 1)
        //        {
        //            if(type.GenericTypeArguments[0].Name == "DateTime")
        //            {
        //                return " DateTime.Parse(\"" + value.ToString() + "\")";
        //            }
        //        }
        //    }
        //    if(type.Name == "DateTime")
        //    {
        //        return " DateTime.Parse(\"" + value.ToString() + "\")";
        //    }
        //    throw new NotImplementedException();
        //}

        //private static string _pascal(string s)
        //{
        //    int i = 0;
        //    string name = "";
        //    while(i < s.Length)
        //    {
        //        if(i == 0 && s[i] != '_')
        //        {
        //            name += char.ToUpper(s[i]);
        //            i++;
        //            continue;
        //        }

        //        if(s[i] == '_')
        //        {
        //            i++;
        //            if( i < s.Length)
        //            {
        //                name += char.ToUpper(s[i]);
        //            }
        //            else
        //            {
        //                return name + "_";
        //            }
        //        }
        //        else
        //        {
        //            name += s[i];
        //        }
        //        i++;
        //    }
        //    return name;
        //}
        //public static Type GetPropertyType(Type type,string propName, out string name)
        //{
        //    name = propName;
        //    PropertyInfo pi = type.GetProperty(propName);
        //    if(pi == null)
        //    {
        //        name = _pascal(propName);
        //        pi = type.GetProperty(name);
        //        if(pi != null)
        //        {
        //            return pi.PropertyType;
        //        }
        //        return null;
        //    }
        //    return pi.PropertyType;
        //}
        //private bool _IsList(Type type)
        //{
        //    return type.FullName.IndexOf("List`") >= 0;
        //}
        //private string _generateFromDictionary(Dictionary<string, object> dic, Type type, string itemTypeName = null)
        //{
        //    string s = " new ";
        //    if (itemTypeName == null)
        //    {
        //        if (type.Name == "IDictionary`2")
        //        {
        //            if (type.IsGenericType)
        //            {
        //                s += "Dictionary<";
        //                int j = 0;
        //                foreach (var item in type.GenericTypeArguments)
        //                {
        //                    s += (j++ == 0 ? "" : ",") + item.FullName;
        //                }
        //                s += ">";
        //            }
        //            else
        //            {

        //            }
        //        }
        //        else
        //        {
        //            s += type.FullName;
        //        }
        //    }
        //    else
        //    {
        //        s += itemTypeName;
        //    }
        //    s += "()\r\n{\r\n";
        //    int i = 0;
        //    string propName;
        //    foreach (var key in dic.Keys)
        //    {
        //        s += i++ == 0 ? "" : ",";
        //        object value = dic[key];
        //        Type propType = GetPropertyType(type,key,out propName);
        //        if (propName == "Properties")
        //        {

        //        }

        //        if (propType == null)
        //        {
        //            if(type.Name == "ExpandoObject")
        //            {
        //                s += string.Format("\r\n // {0} =  ", key, value.ToString());
        //            }
        //            else
        //            {
        //                s += string.Format("\r\n // element {0} not found", key);
        //            }
        //        }
        //        else
        //        {
        //            if (_isSimple(propType))
        //            {
        //                s += string.Format("\r\n{0} = {1}", propName, _simpleValue(propType, value));
        //            }
        //            else
        //            {
        //                if (value == null || value.ToString() == "null")
        //                {
        //                    s += string.Format("\r\n{0} = null", propName);
        //                }
        //                else
        //                {
        //                    bool isGeneric = propType.IsGenericType;
        //                    if (isGeneric && _IsList(propType))
        //                    {
        //                        List<object> list = value as List<object>;
        //                        if (list == null || list.Count == 0)
        //                        {
        //                            s += string.Format("\r\n{0} = null", propName);
        //                        }
        //                        else
        //                        {
        //                            GenericListInfo info = GenericListInfo.Parse(propType, _module);
        //                            s += string.Format("\r\n{0} = {1}", propName, _generateFromList(list, propType, info));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        s += string.Format("\r\n{0} = {1}", propName, _generateCode(value, propType));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    s += "\r\n}\r\n";
        //    return s;
        //}
        //internal static List<string> _GetGenericTypeList(Type pi)
        //{
        //    Type tmp = pi;
        //    List<string> names = new List<string>();
        //    int i;
        //    while (tmp != null)
        //    {
        //        i = tmp.FullName.IndexOf('`');
        //        string nm = i == -1 ? tmp.FullName : tmp.FullName.Substring(0, i);
        //        names.Add(nm);
        //        if (tmp.GenericTypeArguments == null)
        //        {
        //            break;
        //        }
        //        Type[] g = tmp.GenericTypeArguments;
        //        if (g == null || g.Length == 0)
        //        {
        //            break;
        //        }
        //        if (g.Length == 1)
        //        {
        //            tmp = g[0];
        //        }
        //        else
        //        {
        //            string s = "";
        //            for(int j = 0; j < g.Length; j++)
        //            {
        //                s += j > 0 ? "," : "";
        //                i = g[j].FullName.IndexOf('`');
        //                s += i == -1 ? g[j].FullName : g[j].FullName.Substring(0, i);
        //            }
        //            names.Add(s);
        //            tmp = null;
        //        }
        //    }
   
        //    return names;
        //}
//        internal static string _MakeGenericClassName(List<string> names)
//        {
////            names.RemoveAt(0);
//            string s = "";
//            for (int i = 0; i < names.Count; i++)
//            {
//                s += i == names.Count - 1 ? names[i] : names[i] + "<";
//            }
//            for (int i = 0; i < names.Count; i++)
//            {
//                s += i == names.Count - 1 ? "" : ">";
//            }
//            return s;
//        }

        //private string _generateFromList(List<object> list,Type type, GenericListInfo info)
        //{
        //    int i = 0;
        //    string s = "new " + info.ListTypeName + "(){\r\n";
        //    foreach (var item in list)
        //    {
        //        s += i++ == 0 ? "" : ",";
        //        Dictionary<string, object> dic = item as Dictionary<string, object>;
        //        if (info.ItemType == null && dic == null)
        //        {
        //            s += "null";
        //        }
        //        else
        //        {
        //            s += _generateFromDictionary(dic, info.ItemType,info.ItemTypeName);
        //        }
        //    }
        //    s += "\r\n}\r\n";
        //    return s;
        //}
        //private string _generateCode(object data, Type type)//, PropertyInfo propertyInfo = null)
        //{
        //    Dictionary<string, object> dic1 = (data as Dictionary<string, object>);
        //    if(dic1 != null)
        //    {
        //        return _generateFromDictionary(data as Dictionary<string, object>, type);
        //    }
        //    //List<object> list1 = (data as List<object>);
        //    //if(list1 != null)
        //    //{
        //    //    return _generateFromList(list1 as List<object>, type);
        //    //}
        //    //if(type == null)
        //    //{
        //    //    return "null";
        //    //}
        //    throw new NotImplementedException();
        //}
    }
}
