using System.Collections.Generic;
using I18nSharp.Content;
using Newtonsoft.Json.Linq;

namespace I18nSharp
{
    public static class JsonManager
    {
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