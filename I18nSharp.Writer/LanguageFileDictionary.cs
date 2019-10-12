using System;
using System.Collections.Generic;
using I18nSharp.Writer.Content;

namespace I18nSharp.Writer
{
    [Serializable]
    public class LanguageFileDictionary
    {
        public Dictionary<string, ILanguageFileContent<object>> LanguageFileContents { get; set; } =
            new Dictionary<string, ILanguageFileContent<object>>();

        public string CultureString { get; set; }
    }
}