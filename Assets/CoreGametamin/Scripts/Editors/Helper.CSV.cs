#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    public delegate void CSVCallback(List<Dictionary<string, object>> data);
    public static partial class CSVHelper
    {
        public static readonly char Pipe = '|';
        public static void LoadCSV(this ScriptableObject sender, string filename, CSVCallback callback)
        {
            var path = $"CSVs/{filename}";
            var textAsset = Resources.Load<TextAsset>(path);
            //Assert.IsNotNull(textAsset, $"{path} not found!");
            if (textAsset != null)
            {
                var text = textAsset.text;
                callback(CSVReader.GetListFromContent(text));
                EditorUtility.SetDirty(sender);
                AssetDatabase.SaveAssets();
            }
        }
        public static void LoadCSV2(this ScriptableObject sender, string path, CSVCallback callback)
        {
            var textAsset = Resources.Load<TextAsset>(path);
            //Assert.IsNotNull(textAsset, $"{path} not found!");
            if (textAsset != null)
            {
                callback(CSVReader.GetListFromContent(textAsset.text));
                EditorUtility.SetDirty(sender);
                AssetDatabase.SaveAssets();
            }
        }
        public static void LoadCSV3(this ScriptableObject sender, string filename, CSVCallback callback)
        {
            var path = $"CSVs/{filename}";
            var textAsset = Resources.Load<TextAsset>(path);
            //Assert.IsNotNull(textAsset, $"{path} not found!");
            if (textAsset != null)
            {
                var text = textAsset.text;
                callback(CSVReader.GetListFromContent3(text));
                EditorUtility.SetDirty(sender);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif
