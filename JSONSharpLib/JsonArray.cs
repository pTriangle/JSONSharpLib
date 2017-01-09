using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JsonSharpLib
{
    public class JsonArray : List<IJsonElement>, IJsonElement
    {
        public static JsonArray Parse(string inputString)
        {
            var inputSB = new StringBuilder(inputString.Trim(JsonParser.trimChars));
            if (inputSB.Length > 0 && inputSB[0] == '[') inputSB.Remove(0, 1);
            if (inputSB.Length > 0 && inputSB[inputSB.Length - 1] == ']') inputSB.Remove(inputSB.Length - 1, 1);
            int len = inputSB.Length;
            var result = new JsonArray();
            int startPos = 0;
            int opened1 = 0;
            int opened2 = 0;
            bool quotes = false;
            for (int pos = 0; pos < len; pos++)
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
                            pos++;
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
                Add(JsonParser.Parse(inputString));
        }

        public string Encode()
        {
            var result = new StringBuilder("[");
            bool first = true;
            foreach (var element in this)
            {
                if (first)
                    first = false;
                else result.Append(',');
                result.Append(element.Encode());
            }
            result.Append(']');
            return result.ToString();
        }
    }
}
