using System;

namespace I18nSharp.Writer.Content
{
    [Serializable]
    public class LanguageFileText : ILanguageFileContent<string>
    {
        public string Content { get; set; }
    }
}