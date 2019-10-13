using UnityEngine;

namespace I18nSharp.UnityEditor
{
    public class LanguageFileEditorWindowData : ScriptableObject
    {
        public int selectedToolbar;
        public Vector2 languageFileEditorListViewPosition;
        public TextAsset selectedLanguage;
    }
}