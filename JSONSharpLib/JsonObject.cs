using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JsonSharpLib
{
    public class JsonObject : Dictionary<string, IJsonElement>, IJsonElement
    {
        public static JsonObject Parse(string inputString)
        {
            var inputSB = new StringBuilder(inputString.Trim(JsonParser.trimChars));
            if (inputSB.Length > 0 && inputSB[0] == '{') inputSB.Remove(0, 1);
            if (inputSB.Length > 0 && inputSB[inputSB.Length - 1] == '}') inputSB.Remove(inputSB.Length - 1, 1);
            var result = new JsonObject();
            int startPos = 0;
            int opened1 = 0;
            int opened2 = 0;
            bool quotes = false;
            for (int pos = 0; pos < inputSB.Length; pos++)
            {
                char ch = inputSB[pos];
                if (!quotes)
                {
                    switch (ch)
                    {
                        case ',':
                            if (opened1 == 0 && opened2 == 0)
                            {
                                result.ParseAndAddElement(inputSB.ToString().Substring(startPos, pos-startPos));
                                startPos = pos + 1;
                            }
                            break;
                        case '{':
                            opened1++;
                            break;
                        case '}':
                            opened1--;
                            break;
                        case '[':
                            opened2++;
                            break;
                        case ']':
                            opened2--;
                            break;
                        case '"':
                            quotes = true;
                            break;
                    }
                }
                else
                {
                    switch (ch)
                    {
                        case '\\':
                            char ch2 = inputSB[pos + 1];
                            switch(ch2)
                            {
                                case 'n':
                                    inputSB.Remove(pos, 2);
                                    inputSB.Insert(pos, '\n');
                                    break;
                                case 'r':
                                    inputSB.Remove(pos, 2);
                                    inputSB.Insert(pos, '\r');
                                    break;
                                case 't':
                                    inputSB.Remove(pos, 2);
                                    inputSB.Insert(pos, '\t');
                                    break;
                                default:
                                    pos++;
                                    break;
                            }
                            break;
                        case '"':
                            quotes = false;
                            break;
                    }
                }
            }
            result.ParseAndAddElement(inputSB.ToString().Substring(startPos));
            return result;
        }

        internal void ParseAndAddElement(string inputString)
        {
            if (inputString.Length > 0)
            {
                Regex nv = new Regex(@"(("")(?<name>(([^""\\])|(\\[\s\S]))*?)(""))(:)(?<value>[\s\S]*)"); // Ищет имя/значение
                var m = nv.Match(inputString);
                if (m != null)
                {
                    var name = m.Groups["name"].ToString();
                    var value = m.Groups["value"].ToString();
                    this[name] = JsonParser.Parse(value);
                }
            }
        }

        public string Encode()
        {
            var result = new StringBuilder("{");
            bool first = true;
            foreach (var elementName in this.Keys)
            {
                if (first)
                    first = false;
                else result.Append(',');
                result.Append("\"" + elementName + "\"" + ":" + this[elementName].Encode());
            }
            result.Append('}');
            return result.ToString();
        }
    }
}
