using System;
using System.Collections.Generic;
using System.Globalization;

namespace I18nSharp
{
    [Serializable]
    public class LanguageFileDictionary
    {
        public Dictionary<string, ILanguageFileContent<object>> LanguageFileContents { get; } =
            new Dictionary<string, ILanguageFileContent<object>>();

        public string CultureString { get; }
    }
}