using System;
using System.Collections.Generic;
using I18nSharp.Writer.Content;

namespace I18nSharp.Writer
{
    [Serializable]
    public class LanguageFileDictionary
    {
        public Dictionary<string, LanguageFileContent> LanguageFileContents { get; set; } =
            new Dictionary<string, LanguageFileContent>();

        public string CultureString { get; set; }

        public LanguageFileContent this[string key]
        {
            get => LanguageFileContents[key];
            set => LanguageFileContents[key] = value;
        }
    }
}