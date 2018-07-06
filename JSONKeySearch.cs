using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONSearch
{

    public static class JsonExtensions
    {
        public static List<JToken> FindTokens(this JToken containerToken, string name)
        {
            List<JToken> matches = new List<JToken>();
            FindTokens(containerToken, name, matches);
            return matches;
        }

        private static void FindTokens(JToken containerToken, string name, List<JToken> matches)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    if (child.Name == name)
                    {
                        matches.Add(child.Value);
                    }
                    FindTokens(child.Value, name, matches);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    FindTokens(child, name, matches);
                }
            }
        }
    }

    class JSONKeySearch
    {
        static void Main(string[] args)
        {
            string json = @"{
              ""contact"": {
                ""name"": ""Vyshag"",
                  ""title"": ""SE"",
                  ""OtherDetails"": {
                    ""Personal"": {
                      ""ShortName"": ""Vys"",
                      ""Hobby"": ""Reading""
                    },
                              ""Professional"": {
                        ""ID"": ""1234"",
                        ""Experience"": ""4 years""
                      }
                  }
              }
            }";

            JObject jo = JObject.Parse(json);
            //Give search value here
            string KeyToSearch = "contact";
            foreach (JToken token in jo.FindTokens(KeyToSearch))
            {
                if (token.Children().Count() > 0)
                {
                    PrintValuesFromJsonObject(((JObject)(token)));
                }
                else
                {
                    PrintValueFromString(((JProperty)(token.Parent)));
                }

            }
            Console.Read();
        }

        private static void PrintValuesFromJsonObject(JObject inner)
        {
            if (inner.Children().Count() > 0)
            {
                List<string> keys = inner.Properties().Select(p => p.Name).ToList();
                foreach (string key in keys)
                {
                    if (inner[key].Children().Count() > 0)
                    {
                        PrintValuesFromJsonObject((JObject)inner[key]);
                    }
                    else
                    {
                        PrintValueFromString(((JProperty)(inner[key].Parent)));
                    }
                }
            }
            else
            {
                PrintValueFromString(((JProperty)(inner.Parent)));
            }
        }

        private static void PrintValueFromString(JProperty JsonProperty)
        {
            string Key = JsonProperty.Name.ToString();
            string Value = JsonProperty.Value.ToString();
            Console.WriteLine(Key + " : " + Value);

        }
    }
}
