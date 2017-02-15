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
            return simpleTypes.Contains(PropertyType.FullName);
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
                        s += key + " = " + _Object2String(d[key]);
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
        private bool _IsNumber(string s)
        {
            int comaCount = 0;
            int i = 0;
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            while(i < s.Length)
            {
                if (!Char.IsDigit(s[i]))
                {
                    if(s[i] == '.')
                    {
                        comaCount++;
                        if (comaCount > 1) return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                i++;
            }
            return true;
        }
        private string _Object2String(object value)
        {
            if(value == null)
            {
                return "null";
            }
            string s = value.ToString();
            if(s == "null" || s == "true" || s == "false")
            {
                return s;
            }
            if (_IsNumber(s))
            {
                return s;
            }
            return "\"" +value.ToString() + "\"";
        }
        private string _Value2Code()
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
            return _Object2String(Value);
            string value = Value.ToString();

            if (value == "true" || value == "false")
            {
                return value;
            }
            if (Value.GetType().FullName == "System.String")
            {
                return "\"" + Value.ToString() + "\"";
            }

            return Value.ToString();
        }
        public string SimpleValue()
        {
            if (PropertyType == null)
            {
                return _Value2Code();
            }
            if (PropertyType.IsEnum)
            {
                return PropertyType.FullName + "." + Value.ToString();
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
        public string Code
        {
            get
            {
                if (_IsSimpleType())
                {
                    return "." + ObjectPropertyName + " = " + SimpleValue();
                }
                if (PropertyType.IsEnum)
                {
                    return "." + ObjectPropertyName + " = " + SimpleValue();
                }
                return "COMPLEX";
            }
        }
        public override string ToString()
        {
            return ObjectPropertyName + " : " + ElementName + " : " + PropertyType.Name;
        }
    }
}
