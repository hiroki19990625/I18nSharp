using System.Collections.Generic;
using System.Globalization;

namespace I18nSharp
{
    public class ObjectI18n
    {
        public LanguageFileDictionary SelectedLanguage { get; private set; }

        private Dictionary<string, LanguageFileDictionary> _languageFiles =
            new Dictionary<string, LanguageFileDictionary>();

        public void AddLanguage(LanguageFile file)
        {
            _languageFiles[file.LanguageFileDictionary.CultureString] = new LanguageFileDictionary();
        }

        public LanguageFileDictionary GetLanguage(string cultureString)
        {
            return _languageFiles[cultureString];
        }

        public LanguageFileDictionary GetLanguage(CultureInfo cultureInfo)
        {
            return _languageFiles[cultureInfo.Name];
        }

        public LanguageFileDictionary GetDefaultLanguage()
        {
            return _languageFiles[CultureInfo.CurrentCulture.Name];
        }

        public void SelectLanguage(string cultureString)
        {
            SelectedLanguage = _languageFiles[cultureString];
        }

        public void SelectLanguage(CultureInfo cultureInfo)
        {
            SelectedLanguage = _languageFiles[cultureInfo.Name];
        }

        public void SelectDefaultLanguage()
        {
            SelectedLanguage = _languageFiles[CultureInfo.CurrentCulture.Name];
        }

        public ILanguageFileContent<object> this[string key] => SelectedLanguage.LanguageFileContents[key];
    }
}