using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gametamin.Core
{
    public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var type = typeof(T);
                    // Try to load in "Resources"
                    _instance = Resources.Load<T>(type.Name);
                    if (_instance != null) return _instance;
                    // Namespace + class name
                    _instance = Resources.Load<T>(type.FullName);
                    if (_instance != null) return _instance;
#if UNITY_EDITOR
                    _instance = AssetDatabaseHelper.FindAssetDatabase<T>(type.Name, null);
                    if (_instance != null)
                    {
                        //Log.Warning($"Load <b>{type.Name}</b> using FindAssetDatabase!");
                        return _instance;
                    }
#endif
                    //Log.Warning($"Can't load {type.Name}!");
                }
                return _instance;
            }
        }
        public static T GetInstance(string name)
        {
            if (_instance == null)
            {
                _instance = Resources.Load<T>(name);
            }
            return _instance;
        }
#if UNITY_EDITOR
        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
