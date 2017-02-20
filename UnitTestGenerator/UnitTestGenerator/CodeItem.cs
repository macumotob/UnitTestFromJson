using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGenerator
{
    class CodeItem
    {
        public string ElementName { get; set; }
        public string ObjectPropertyName { get; set; }
        public object Value { get; set; }
        public Type PropertyType { get; set; }
        public bool IsList { get; set; }
        public bool IsDictionary{ get; set; }
        public bool IsPrimitive
        {
            get
            {
                return _IsSimpleType();
            }
        }
        private bool _IsSimpleType()
        {
            if (PropertyType == null)
            {
                return true;
            }
            if (PropertyType.IsEnum)
            {
                return true;
            }
            string[] simpleTypes = { "System.String", "System.Int32", "System.Int64", "System.Boolean", "System.DateTime" };
            if(simpleTypes.Contains(PropertyType.FullName))
            {
                return true;
            }

            List<string> list = Utils.GenericType2List(PropertyType);
            if(list.Count > 1)
            {
                if (list[0] == "System.Collections.Generic.List" || list[0] == "System.Collections.Generic.IDictionary")
                {
                    IsList = list[0] == "System.Collections.Generic.List";
                    IsDictionary = list[0] == "System.Collections.Generic.IDictionary";
                }
                else
                {
                    if(list.Count == 2 && list[0] == "System.Nullable" && list[1] == "System.DateTime")
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private string _ListValue2Code()
        {
            string s = "";
            List<object> list = Value as List<object>;
            foreach (var item in list)
            {
                if (item is Dictionary<string, object>)
                {
                    s += s == "" ? "" : ",\r\n";
                    s += "    new {";
                    Dictionary<string, object> d = item as Dictionary<string, object>;
                    int index = 0;
                    foreach (string key in d.Keys)
                    {
                        s += index++ == 0 ? "" : ",";
                        s += key + " = " + Utils.ObjectToString(d[key]);
                    }
                    s += "}";
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            s = " new List<dynamic>() {\r\n" + s;
            s += "\r\n}";
            return s;
        }
        private string _Value2String()
        {
            if (Value == null)
            {
                return "null";
            }
            
            Type valueType = Value.GetType();
            if (valueType.Name == "List`1")
            {
                return _ListValue2Code();
            }
            return Utils.ObjectToString(Value);
        }
        public string SimpleValue()
        {
            if (PropertyType == null)
            {
                return _Value2String();
            }
            if (PropertyType.IsEnum)
            {
                if (Utils.IsNumber(Value.ToString()))
                {
                    int envalue;
                    int.TryParse(Value.ToString(), out envalue);
                    string name = Enum.GetName(this.PropertyType, envalue);
                    return PropertyType.FullName + "." + name;
                }
                else
                {
                    return PropertyType.FullName + "." + Value.ToString();
                }
                
            }

            if (PropertyType.Name == "String")
            {
                return "\"" + (Value == null ? "" : Value.ToString()) + "\"";
            }
            if (PropertyType.Name == "Boolean")
            {
                return Value.ToString();
            }
            if (PropertyType.Name == "Int32")
            {
                return Value == null ? "0" : Value.ToString();
            }
            if (PropertyType.Name == "Int64")
            {
                return Value == null ? "0" : Value.ToString();
            }
            if (PropertyType.Name == "Nullable`1")
            {
                if (PropertyType.GenericTypeArguments != null && PropertyType.GenericTypeArguments.Length == 1)
                {
                    if (PropertyType.GenericTypeArguments[0].Name == "DateTime")
                    {
                        return " DateTime.Parse(\"" + Value.ToString() + "\")";
                    }
                }
            }
            if (PropertyType.Name == "DateTime")
            {
                return " DateTime.Parse(\"" + Value.ToString() + "\")";
            }
            return null;
        }
        public override string ToString()
        {
            return ObjectPropertyName + " : " + ElementName + " : " + PropertyType.Name;
        }
    }
}
