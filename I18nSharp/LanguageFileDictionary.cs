using System;
using System.Collections.Generic;
using I18nSharp.Content;

namespace I18nSharp
{
    [Serializable]
    public class LanguageFileDictionary
    {
        public Dictionary<string, LanguageFileContent> LanguageFileContents { get; internal set; } =
            new Dictionary<string, LanguageFileContent>();

        public string CultureString { get; internal set; }
    }
}