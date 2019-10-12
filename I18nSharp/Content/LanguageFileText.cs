using System;

namespace I18nSharp
{
    [Serializable]
    public class LanguageFileText : ILanguageFileContent<string>
    {
        public string Content { get; }
    }
}