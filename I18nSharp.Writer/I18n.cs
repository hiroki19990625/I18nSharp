using System.Collections.Generic;
using System.Globalization;

namespace I18nSharp.Writer
{
    public class I18n
    {
        public static LanguageFileDictionary SelectedLanguage { get; private set; }

        private static Dictionary<string, LanguageFileDictionary> _languageFiles =
            new Dictionary<string, LanguageFileDictionary>();

        public static void AddLanguage(LanguageFile file)
        {
            _languageFiles[file.LanguageFileDictionary.CultureString] = file.LanguageFileDictionary;
        }

        public static LanguageFileDictionary GetLanguage(string cultureString)
        {
            return _languageFiles[cultureString];
        }

        public static LanguageFileDictionary GetLanguage(CultureInfo cultureInfo)
        {
            return _languageFiles[cultureInfo.Name];
        }

        public static LanguageFileDictionary GetDefaultLanguage()
        {
            return _languageFiles[CultureInfo.CurrentCulture.Name];
        }

        public static void SelectLanguage(string cultureString)
        {
            SelectedLanguage = _languageFiles[cultureString];
        }

        public static void SelectLanguage(CultureInfo cultureInfo)
        {
            SelectedLanguage = _languageFiles[cultureInfo.Name];
        }
    }
}