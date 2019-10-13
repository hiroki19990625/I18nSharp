using System.Collections.Generic;
using UnityEngine;

namespace I18nSharp.UnityEditor
{
    public class LanguageFileEditorWindowData : ScriptableObject
    {
        public int selectedToolbar;
        public Vector2 languageFileEditorListViewPosition;
        public TextAsset selectedLanguage;
        public string codeGeneratePath = "Assets/Plugins/I18nSharp/Scripts/I18n-GenerateCode.cs";
        public string resourcePath = "Languages";
        public string defaultCulture;
        public List<TextAsset> generateCodeLanguages;
    }
}