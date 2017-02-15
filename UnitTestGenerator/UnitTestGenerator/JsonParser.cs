using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGenerator
{
    class JsonParser
    {
        private void _skipws(string s,ref int i)
        {
            while(i < s.Length && (s[i] == ' ' || s[i] == '\r' ||  s[i] == '\n' || s[i] == '\t'))
            {
                i++;
            }
            //if(i >= s.Length)
            //{
            //    throw new NotImplementedException();
            //}
        }
        private string _readName(string s,ref int i )
        {
            if( i < s.Length &&  s[i] == '"')
            {
                string name = "";
                i++;
                while (i < s.Length && s[i] != '"')
                {
                    name += s[i++];
                }
                if (i < s.Length && s[i] == '"')
                {
                    i++;
                    return name; 
                }

            }
            throw new NotImplementedException();
        }
        private object _readObject(string s,ref int i)
        {
            if(s[i] != '{')
            {
                throw new NotImplementedException();
            }
            i++;
            Dictionary<string, object> dic= new Dictionary<string, object>();
            string name = null;
            object value = null;
            while (i < s.Length)
            {
                _skipws(s, ref i);
                char c = s[i];
                switch (c)
                {
                    case '{':
                        i++;
                        return _readObject(s, ref i);
                    case '"':
                        name = _readName(s, ref i);
                        dic.Add(name, null);
                        break;
                    case ':':
                        i++;
                        _skipws(s, ref i);
                        value = _readValue(s, ref i);
                        dic[name] = value;
                        break;
                    case '}':
                        i++;
                        return dic;

                    case '[':
                    case ']':
                        break;
                    case ',':
                        i++;
                        break;
                    default:
                        break;

                }
            }
            return dic;
        }
        private object _readArray(string s, ref int i)
        {
            List<object> list = new List<object>();
            if(i < s.Length && s[i] != '[')
            {
                throw new NotImplementedException();
            }
            label_value:
            i++;
            char c = s[i];

            _skipws(s, ref i);
            c = s[i];
            switch (s[i])
            {
                case '{':
                    {
                        var value = _readObject(s, ref i);
                        list.Add(value);
                    }
                    break;
                default:
                    {
                        var value = _readValue(s, ref i);
                        list.Add(value);
                    }
                    break;
            }
            _skipws(s, ref i);
            if(i < s.Length && s[i] == ']')
            {
                i++;
                return list;
            }
            if( i < s.Length &&  s[i] == ',')
            {
                goto label_value;
            }
            throw new NotImplementedException();
            
        }
        private object _readValue(string s, ref int i)
        {
            if(i < s.Length)
            {
                char c = s[i];

                if(s[i] == '"')
                {
                    return _readName(s, ref i);
                }
                if(s[i] == '{')
                {
                    return _readObject(s, ref i);
                }
                if(s[i] == '[')
                {
                    object array = _readArray(s, ref i);
                    return array;

                }
                string value = "";
                while (i < s.Length && s[i] != ',' && s[i] != '}' && s[i] != ']' && s[i] != '\r' && s[i] != '\n')
                {
                    value += s[i++];
                }
                return value;
            }
            throw new NotImplementedException();
        }
        public Dictionary<string, object> Parse(string s)
        {
            Dictionary<string, object> root = null;

            int i = 0;
            //string name = null;
            object value = null;
            while (i < s.Length)
            {
                _skipws(s, ref i);
                if (i >= s.Length)
                {
                    break;
                }
                char c = s[i];
                switch (c)
                {
                    case '{':
                        root = (Dictionary<string, object>)_readObject(s, ref i);
                        break;
                    case '[':
                        value = _readArray(s, ref i);

                        break;
                    default:
                        throw new NotImplementedException();

                }
            }
            return root;
        }
    }
}
