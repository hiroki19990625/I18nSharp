using System.Collections.Generic;
using I18nSharp.Writer.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace I18nSharp.Writer
{
    public static class JsonManager
    {
        public static string Serialize(LanguageFileDictionary languageFileDictionary)
        {
            JObject jObject = JObject.FromObject(languageFileDictionary);
            return jObject.ToString(Formatting.Indented);
        }

        public static LanguageFileDictionary Deserialize(string json)
        {
            LanguageFileDictionary languageFileDictionary = new LanguageFileDictionary();
            JObject jObject = JObject.Parse(json);

            languageFileDictionary.CultureString = jObject.Value<string>("CultureString");
            languageFileDictionary.LanguageFileContents = new Dictionary<string, LanguageFileContent>();
            foreach (JToken jToken in jObject.GetValue("LanguageFileContents"))
            {
                JProperty jProperty = (JProperty) jToken;
                languageFileDictionary.LanguageFileContents.Add(jProperty.Name,
                    new LanguageFileText(((JObject) jProperty.Value).Value<string>("Content")));
            }

            return languageFileDictionary;
        }
    }
}