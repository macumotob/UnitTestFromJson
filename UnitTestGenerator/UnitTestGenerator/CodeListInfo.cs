using System;
using System.Collections.Generic;
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

        public List<CodeItems> Items = new List<CodeItems>();
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

                ItemType = module.GetType(_typeNames[1]);
                if (ItemType == null)
                {

                }
                _typeNames.RemoveAt(0);
                ItemTypeName = _MakeGenericClassName(_typeNames);
            }
        }
    }
}
