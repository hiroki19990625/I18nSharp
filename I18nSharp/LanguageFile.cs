using System;
using System.IO;
using System.Text;
using I18nSharp.Content;

namespace I18nSharp
{
    public class LanguageFile
    {
        public LanguageFileDictionary LanguageFileDictionary { get; private set; }
        public string JsonText { get; }

        public LanguageFile(FileInfo file, Encoding encoding = null)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            if (encoding == null)
                encoding = Encoding.UTF8;
            JsonText = File.ReadAllText(file.FullName, encoding);

            Load();
        }

        public LanguageFile(string jsonText)
        {
            JsonText = jsonText ?? throw new ArgumentNullException(nameof(jsonText));

            Load();
        }

        public LanguageFile(byte[] bytes, Encoding encoding = null)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            if (encoding == null)
                encoding = Encoding.UTF8;
            JsonText = encoding.GetString(bytes);

            Load();
        }

        public LanguageFileContent this[string key] => LanguageFileDictionary[key];

        public T GetContent<T>(string key) where T : LanguageFileContent
        {
            return (T) this[key];
        }

        public string GetCulture()
        {
            return LanguageFileDictionary.CultureString;
        }

        private void Load()
        {
            if (JsonText == null)
                throw new InvalidOperationException();

            LanguageFileDictionary = JsonManager.Deserialize(JsonText);
        }
    }
}