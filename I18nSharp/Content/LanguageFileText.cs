using System;

namespace I18nSharp.Content
{
    [Serializable]
    public class LanguageFileText : LanguageFileContent
    {
        public string Content { get; }

        internal LanguageFileText()
        {
        }

        internal LanguageFileText(string content)
        {
            Content = content;
        }
    }
}