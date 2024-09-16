
#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using UnityEditor;

namespace Gametamin.Core.Localization
{
    [CustomEditor(typeof(LocalizationFactory), true)]
    public class LocalizationFactoryEditor : TextReferenceNameFactoryEditor
    {
        LocalizationFactory _target;
        LocalizationFactory _Target
        {
            get
            {
                if (_target == null)
                {
                    _target = (LocalizationFactory)target;
                }
                return _target;
            }
        }
        protected override bool _Editable => true;
        protected override bool IsRoot => false;
        protected override void OnCustomGUI()
        {
            base.OnCustomGUI();
            EditorGUIHelper.GUIButton("Export", () =>
            {
                _Target.ExportToCSV();
            });
            EditorGUIHelper.GUIButton("Import", () =>
            {
                _Target.ImportFromCSV();
            });

            EditorGUIHelper.GUIButton("Go to Sheet", () =>
            {
                Application.OpenURL("https://docs.google.com/spreadsheets/d/13lr0kBUwAuBntnZr81RMfyU6kLx5Xw0qu3Lc4ZRp-JI/edit#gid=0");
            });
            EditorGUIHelper.GUIButton("Import CSV...", () =>
            {
                var text = LoadCSV();
                if (!string.IsNullOrEmpty(text))
                {
                    Debug.Log($"TODO:\n{text}");
                }
            });
        }

        static string _lastCSVPath;
        static string LoadCSV()
        {
            string text = "";
            var path = EditorUtility.OpenFilePanel("Select File", _lastCSVPath, "csv");
            if (!string.IsNullOrEmpty(path))
            {
                _lastCSVPath = path;
                var fileInfo = new FileInfo(path);
                using (var reader = fileInfo.OpenText())
                {
                    text = reader.ReadToEnd();
                    reader.Close();
                }
            }
            return text;
        }
    }
}
#endif
