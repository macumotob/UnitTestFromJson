using System;
using System.Collections.Generic;
using System.Linq;
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


    }// end of class
}
