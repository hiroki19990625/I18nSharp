using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace I18nSharp.Writer
{
    public class LanguageFile
    {
        public LanguageFileDictionary LanguageFileDictionary { get; set; }
        public string JsonText { get; set; }

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

        private void Load()
        {
            if (JsonText == null)
                throw new InvalidOperationException();

            LanguageFileDictionary = JsonConvert.DeserializeObject<LanguageFileDictionary>(JsonText);
        }

        public void Save()
        {
            if (LanguageFileDictionary == null)
                throw new InvalidOperationException();

            JsonText = JsonConvert.SerializeObject(LanguageFileDictionary);
        }

        public void SaveFile(FileInfo file, Encoding encoding = null)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            if (encoding == null)
                encoding = Encoding.UTF8;

            Save();

            File.WriteAllText(file.FullName, JsonText, encoding);
        }

        public string SaveToString()
        {
            Save();

            return JsonText;
        }

        public byte[] SaveToBytes(Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            Save();

            return encoding.GetBytes(JsonText);
        }
    }
}