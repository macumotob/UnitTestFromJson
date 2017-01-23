using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGenerator
{
    public class Creator
    {
        public static Creator Instance = new Creator();
        public object Create(Type type,Dictionary<string,object> dic)
        {
            object doc = Activator.CreateInstance(type);
            _setProperties(doc, type, dic);
            return doc;
        }
        public object Create(string assemblyName,string className, Dictionary<string, object> dic)
        {
            Assembly asm = Assembly.LoadFrom(assemblyName);
            Type type = asm.GetType(className);
            return Create(type, dic);
        }
        private bool _isList(PropertyInfo property)
        {
            //TODO use  ImplementedInterfaces
            return property.PropertyType.Name.IndexOf("List`") == 0;
        }
        private void _setFromList(object doc,List<object> list)
        {

        }
        private void _setProperties(object doc, Type type, Dictionary<string, object> dic)
        {
            PropertyInfo[] properties = type.GetProperties();
            foreach(PropertyInfo p in properties)
            {
                if (dic.ContainsKey(p.Name))
                {
                    object value = dic[p.Name];
                    p.SetValue(doc, value);
                }
                else
                {
                    string name = p.PropertyType.Name;
                    if (p.PropertyType.IsGenericType)
                    {
                        if (_isList(p))
                        {
                            if( p.PropertyType.GenericTypeArguments != null && p.PropertyType.GenericTypeArguments.Length > 0)
                            {
                                string listName = "List<";
                                for(int i =0;i < p.PropertyType.GenericTypeArguments.Length; i++)
                                {
                                    Type gp = p.PropertyType.GenericTypeArguments[i].BaseType;
                                    listName += (i == 0 ? "" : ",") + gp.FullName;
                                }
                                listName += ">";
                                Assembly asm = Assembly.GetExecutingAssembly();
                                AssemblyName[] asms = asm.GetReferencedAssemblies();
                                
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }

                }
            }
            

        }
    }
}
