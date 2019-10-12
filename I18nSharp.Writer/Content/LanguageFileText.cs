using System;

namespace I18nSharp.Writer.Content
{
    [Serializable]
    public class LanguageFileText : LanguageFileContent
    {
        public string Content { get; set; }

        public LanguageFileText()
        {
        }

        public LanguageFileText(string content)
        {
            Content = content;
        }
    }
}