using UnityEditor;
using UnityEngine;

namespace I18nSharp.UnityEditor
{
    public class LanguageFileEditorPathWindow : EditorWindow
    {
        private string path;

        [MenuItem("I18nSharp/Window Path")]
        public static void Open()
        {
            if (!EditorPrefs.HasKey(nameof(LanguageFileEditorPathWindow)))
                EditorPrefs.SetString(nameof(LanguageFileEditorPathWindow),
                    "Assets/Plugins/I18nSharp/LanguageFileEditorWindow.asset");

            LanguageFileEditorPathWindow window = CreateInstance<LanguageFileEditorPathWindow>();
            window.titleContent = new GUIContent("LanguageFileWindowPath");
            window.position = new Rect(100, 100, 600, 50);
            window.minSize = new Vector2(600, 50);
            window.maxSize = new Vector2(600, 50);
            window.path = EditorPrefs.GetString(nameof(LanguageFileEditorPathWindow));
            window.ShowUtility();
        }

        public void OnGUI()
        {
            path = EditorGUILayout.TextField("Window Path", path);
            if (GUILayout.Button("Save"))
            {
                EditorPrefs.SetString(nameof(LanguageFileEditorPathWindow), path);
                Close();
            }
        }
    }
}