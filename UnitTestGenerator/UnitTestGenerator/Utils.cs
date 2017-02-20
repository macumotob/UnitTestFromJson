using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGenerator
{
    static class Utils
    {
        public static bool IsNumber(string s)
        {
            int comaCount = 0;
            int i = 0;
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            while (i < s.Length)
            {
                if (!Char.IsDigit(s[i]))
                {
                    if (s[i] == '.')
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
        public static string ObjectToString(object value)
        {
            if (value == null)
            {
                return "null";
            }
            string s = value.ToString();
            if (s == "null" || s == "true" || s == "false")
            {
                return s;
            }
            if (Utils.IsNumber(s))
            {
                return s;
            }
            return "\"" + value.ToString() + "\"";
        }
        private static string _ToPascal(string s)
        {
            int i = 0;
            string name = "";
            while (i < s.Length)
            {
                if (i == 0 && s[i] != '_')
                {
                    name += char.ToUpper(s[i]);
                    i++;
                    continue;
                }

                if (s[i] == '_')
                {
                    i++;
                    if (i < s.Length)
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

        public static Type GetPropertyType(Type type, string propName, out string name)
        {
            name = propName;
            PropertyInfo pi = type.GetProperty(propName);
            if (pi == null)
            {
                name = _ToPascal(propName);
                pi = type.GetProperty(name);
                if (pi != null)
                {
                    return pi.PropertyType;
                }
                return null;
            }
            return pi.PropertyType;
        }

        private static void _GetGenericTypeList(Type type, List<string> list)
        {
            Type tmp = type;
            int i;
            int n = tmp.FullName.IndexOf('`');
            string name = n == -1 ? tmp.FullName : tmp.FullName.Substring(0, n);
            list.Add(name);
            for (i = 0; i < type.GenericTypeArguments.Length; i++)
            {
                tmp = type.GenericTypeArguments[i];
                _GetGenericTypeList(tmp, list);
            }
        }
        public static List<string> GenericType2List(Type type)
        {
            List<string> list = new List<string>();
            _GetGenericTypeList(type, list);
            return list;
        }

    }// end of class
}
