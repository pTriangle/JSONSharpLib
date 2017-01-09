using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonSharpLib
{
    public static class JsonParser
    {
        internal static char[] trimChars = new char[] { ' ', '\r', '\n', '\t' };

        public static IJsonElement Parse(string inputString)
        {
            inputString = inputString.Trim(trimChars);
            if (inputString.StartsWith("{"))
                return JsonObject.Parse(inputString);
            else if (inputString.StartsWith("["))
                return  JsonArray.Parse(inputString);
            else 
                return JsonValue.Parse(inputString);
            throw new Exception("Invalid JSON element");
        }
    }
}
