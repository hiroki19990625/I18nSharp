using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using I18nSharp.Writer;
using I18nSharp.Writer.Content;
using UnityEditor;
using UnityEngine;

namespace I18nSharp.UnityEditor
{
    public class LanguageFileEditorWindow : EditorWindow
    {
        private LanguageFileEditorWindowData _data;

        private string _newLanguageCultureField = "";
        private string _newLanguageCreatePathField = "Assets/Plugins/I18nSharp/Resources/Languages";

        private bool _renameMode;
        private string _renameItemKey;
        private string _renameKey;
        private string _createKey = "New Key";
        private string _createValue = "Text";

        private LanguageFile _languageFile;

        [MenuItem("I18nSharp/Editor Window")]
        public static void Open()
        {
            if (!EditorPrefs.HasKey(nameof(LanguageFileEditorPathWindow)))
                EditorPrefs.SetString(nameof(LanguageFileEditorPathWindow),
                    "Assets/Plugins/I18nSharp/LanguageFileEditorWindow.asset");

            string path = EditorPrefs.GetString(nameof(LanguageFileEditorPathWindow));
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(new FileInfo(path).DirectoryName ?? throw new InvalidOperationException());

                LanguageFileEditorWindowData d = CreateInstance<LanguageFileEditorWindowData>();
                AssetDatabase.CreateAsset(d, path);
            }

            LanguageFileEditorWindowData data = AssetDatabase.LoadAssetAtPath<LanguageFileEditorWindowData>(path);

            LanguageFileEditorWindow window = CreateInstance<LanguageFileEditorWindow>();
            window._data = data;
            window.Show();
        }

        public void OnGUI()
        {
            _data.selectedToolbar = GUILayout.Toolbar(_data.selectedToolbar, new[]
            {
                "Editor",
                "New Language File",
                "Code Generator(TODO)"
            });

            if (_data.selectedToolbar == 0)
            {
                _data.selectedLanguage =
                    EditorGUILayout.ObjectField("LanguageFile", _data.selectedLanguage, typeof(TextAsset), false) as
                        TextAsset;

                if (_data.selectedLanguage != null)
                {
                    if (_languageFile == null ||
                        (_languageFile != null && _languageFile.GetCulture() != _data.selectedLanguage.name))
                        _languageFile = new LanguageFile(_data.selectedLanguage.text);

                    EditorGUILayout.LabelField(_data.selectedLanguage.name);
                    _data.languageFileEditorListViewPosition =
                        EditorGUILayout.BeginScrollView(_data.languageFileEditorListViewPosition, GUI.skin.box);
                    {
                        string removeKey = null;
                        foreach (KeyValuePair<string, LanguageFileContent> valuePair in _languageFile
                            .LanguageFileDictionary
                            .LanguageFileContents)
                        {
                            EditorGUILayout.BeginHorizontal(GUI.skin.box);
                            if (valuePair.Value is LanguageFileText text)
                            {
                                EditorGUILayout.LabelField(valuePair.Key);
                                text.Content = EditorGUILayout.TextArea(text.Content, EditorStyles.textArea);

                                EditorGUILayout.Space();

                                if (!(_renameMode && _renameItemKey == valuePair.Key) &&
                                    GUILayout.Button("Rename", GUILayout.Width(100)))
                                {
                                    _renameMode = true;
                                    _renameItemKey = valuePair.Key;
                                    _renameKey = valuePair.Key;
                                }

                                if (_renameMode && _renameItemKey == valuePair.Key)
                                {
                                    EditorGUILayout.LabelField("Rename Item...");
                                }

                                if (GUILayout.Button("x", GUILayout.Width(25)))
                                {
                                    removeKey = valuePair.Key;

                                    GUIUtility.keyboardControl = 0;
                                }
                            }

                            EditorGUILayout.EndHorizontal();
                        }

                        if (removeKey != null)
                            _languageFile.LanguageFileDictionary.LanguageFileContents.Remove(removeKey);
                    }
                    EditorGUILayout.EndScrollView();

                    if (!_renameMode)
                    {
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        {
                            EditorGUILayout.LabelField("Add Content");
                            _createKey = EditorGUILayout.TextField("Key", _createKey);
                            EditorGUILayout.LabelField("Value");
                            _createValue = EditorGUILayout.TextArea(_createValue);
                        }
                        EditorGUILayout.EndVertical();

                        if (GUILayout.Button("Add"))
                        {
                            if (_languageFile.LanguageFileDictionary.LanguageFileContents.ContainsKey(_createKey))
                            {
                                EditorUtility.DisplayDialog("Error", "Created Key", "OK");
                                return;
                            }

                            if (string.IsNullOrWhiteSpace(_createKey))
                            {
                                EditorUtility.DisplayDialog("Error", "Invalid Key", "OK");
                                return;
                            }

                            GUIUtility.keyboardControl = 0;

                            _languageFile[_createKey] = new LanguageFileText(_createValue);
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Rename Mode");

                        _renameKey = EditorGUILayout.TextField("Rename Key", _renameKey);
                        if (GUILayout.Button("Rename"))
                        {
                            if (_languageFile.LanguageFileDictionary.LanguageFileContents.ContainsKey(_renameKey))
                            {
                                EditorUtility.DisplayDialog("Error", "Created Key", "OK");
                                return;
                            }

                            if (string.IsNullOrWhiteSpace(_renameKey))
                            {
                                EditorUtility.DisplayDialog("Error", "Invalid Key", "OK");
                                return;
                            }

                            LanguageFileContent content = _languageFile[_renameItemKey];
                            _languageFile.LanguageFileDictionary.LanguageFileContents.Remove(_renameItemKey);
                            _languageFile[_renameKey] = content;

                            GUIUtility.keyboardControl = 0;

                            _renameMode = false;
                            _renameItemKey = null;
                        }

                        if (GUILayout.Button("Cancel"))
                        {
                            GUIUtility.keyboardControl = 0;

                            _renameMode = false;
                            _renameItemKey = null;
                        }
                    }

                    if (GUILayout.Button("Save"))
                    {
                        File.WriteAllText(AssetDatabase.GetAssetPath(_data.selectedLanguage),
                            _languageFile.SaveToString());

                        AssetDatabase.Refresh();
                    }
                }
            }
            else if (_data.selectedToolbar == 1)
            {
                _newLanguageCultureField = EditorGUILayout.TextField("Culture", _newLanguageCultureField);

                if (GUILayout.Button("Select Culture"))
                {
                    string[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                        .Select(culture => culture.Name)
                        .Where(cName =>
                            cName.IndexOf(_newLanguageCultureField, StringComparison.CurrentCultureIgnoreCase) != -1)
                        .ToArray();

                    GenericMenu menu = new GenericMenu();
                    foreach (string culture in cultures)
                    {
                        menu.AddItem(new GUIContent(culture), false, obj => _newLanguageCultureField = obj as string,
                            culture);
                    }

                    menu.ShowAsContext();
                }

                _newLanguageCreatePathField = EditorGUILayout.TextField("CreatePath", _newLanguageCreatePathField);

                if (GUILayout.Button("Create"))
                {
                    Directory.CreateDirectory(_newLanguageCreatePathField);

                    LanguageFile file = new LanguageFile();
                    file.LanguageFileDictionary.CultureString = _newLanguageCultureField;
                    File.WriteAllText($"{_newLanguageCreatePathField}/{_newLanguageCultureField}.json",
                        file.SaveToString());

                    AssetDatabase.Refresh();
                }
            }
        }
    }
}