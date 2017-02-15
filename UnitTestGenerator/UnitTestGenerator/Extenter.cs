using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGenerator
{
    static class Extenter
    {
        public static bool _IsSimple(this Type type)
        {
            if (type == null)
            {
                return false;
            }
            if (type.BaseType.Name == "Enum")
            {
                return true;
            }
            else if (type.Name == "String")
            {
                return true;
            }
            else if (type.Name == "Boolean")
            {
                return true;
            }
            return false;
        }

    }
}
