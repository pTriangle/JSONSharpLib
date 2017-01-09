using JsonSharpLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputString = @"{
                ""firstName"":""Иван"",
                ""lastName"":""Иванов"",
                ""test"":""test1\ntest2\ntest3\\"",
                ""address"":{
                    ""streetAddress"":""Московское ш., 101, кв.101"",
                    ""city"":""Ленинград"",
                    ""postalCode"":101101
                },
                ""phoneNumbers"":[
                    ""812 123-1234"",
                    ""916 123-4567""
                ]
            }";
            var json = (JsonObject)JsonParser.Parse(inputString);
            Console.WriteLine("First name: {0}", json["firstName"]);
            Console.WriteLine("Last name: {0}", json["lastName"]);
            var address = (JsonObject)json["address"];
            foreach (var el in address.Keys) Console.WriteLine("{0} = {1}", el, address[el]);
            var phoneNumbers = (JsonArray)json["phoneNumbers"];
            foreach (var el in phoneNumbers) Console.WriteLine("Phone: {0}", el);
            var encodedJson = json.Encode();
            var decodedJson = JsonParser.Parse(encodedJson);
            Debug.Assert(encodedJson == decodedJson.Encode());
            Console.WriteLine("Encoded JSON: {0}", encodedJson);
            
            var testJson = new JsonObject();
            testJson["hello"] = new JsonValue("world");
            testJson["escaped_string"] = new JsonValue(@"Blabla""[123]\{abc}");
            var author = new JsonObject();
            author["name"] = new JsonValue("pTriangle");
            author["age"] = new JsonValue(26);
            var contacts = new JsonArray();
            contacts.Add(new JsonValue("http://moveax.pro"));
            contacts.Add(new JsonValue("root@triangleware.org"));
            author["contacts"] = contacts;
            testJson["author"] = author;
            encodedJson = testJson.Encode();
            decodedJson = JsonParser.Parse(encodedJson);
            Debug.Assert(encodedJson == decodedJson.Encode());
            Console.WriteLine("Encoded JSON: {0}", encodedJson);
            Console.ReadLine();
        }
    }
}
