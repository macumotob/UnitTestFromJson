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

        private List<string> _typeNames = new List<string>();
        public List<CodeItems> Items = new List<CodeItems>();

        private void _GetGenericTypeList(Type type)
        {
            Type tmp = type;
            int i;
            int n = tmp.FullName.IndexOf('`');
            string name = n == -1 ? tmp.FullName : tmp.FullName.Substring(0, n);
            _typeNames.Add(name);
            for (i = 0; i < type.GenericTypeArguments.Length; i++)
            {
                tmp = type.GenericTypeArguments[i];
                _GetGenericTypeList(tmp);
            }
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
            _typeNames.Clear();
            bool isGeneric = type.IsGenericType;
            if (isGeneric)
            {
                _GetGenericTypeList(type);
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
