using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JsonSharpLib
{
    public class JsonValue : IJsonElement
    {
        public readonly object Value;

        public JsonValue(object value)
        {
            Value = value;
        }

        public static JsonValue Parse(string inputString)
        {
            var inputSB = new StringBuilder(inputString.Trim(JsonParser.trimChars));
            if (inputSB.Length > 0 && inputSB[0] == '"' && inputSB[inputSB.Length - 1] == '"')
            {
                inputSB.Remove(0, 1);
                inputSB.Remove(inputSB.Length - 1, 1);
                var value = Regex.Replace(inputSB.ToString(), @"\\(?<char>.)", "${char}"); // Заменяем escape-последовательности на символы
                return new JsonValue(value);
            }
            if (inputSB.ToString().Contains('.'))
                return new JsonValue(float.Parse(inputSB.ToString(), CultureInfo.InvariantCulture));
            if (inputSB.ToString() == "true") return new JsonValue(true);
            if (inputSB.ToString() == "false") return new JsonValue(false);
            if (inputSB.ToString() == "null") return new JsonValue(null);
            return new JsonValue(long.Parse(inputSB.ToString()));
        }

        public string Encode()
        {
            if (Value is string)
                return '"' + ((string)Value).Replace("\\", "\\\\").Replace("\"", @"\""").Replace("\n","\\n").Replace("\r","\\r").Replace("\t","\\t") + '"';
            return Value.ToString();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
