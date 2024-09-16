#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Gametamin.Core
{
    public static partial class AssetDatabaseHelper
    {
        public static T FindAssetDatabase<T>(this string assetName, string[] searchInFolders = default) where T : Object
        {
            T result = default;
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name.ToLower()} {assetName}", searchInFolders);
            guids.ForBreakable(guid =>
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = LoadAssetAtPath<T>(path);
                if (asset != null)
                {
                    result = asset;
                    return true;
                }
                return false;
            });
            return result;
        }
        public static void FindAssetDatabase<T>(this string assetName, string[] searchInFolders = default, Action<T> callback = null) where T : Object
        {
            if (searchInFolders != null)
            {
                var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name.ToLower()} {assetName}", searchInFolders);
                guids.For(guid =>
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var asset = LoadAssetAtPath<T>(path);
                    if (asset != null)
                    {
                        callback?.Invoke(asset);
                    }
                });
            }
        }
        public static void FindAssetDatabase<T>(this string[] searchInFolders, Action<T> callback) where T : Object
        {
            FindAssetDatabase<T>(searchInFolders, typeof(T).Name, callback);
        }
        public static void FindAssetDatabase<T>(this string[] searchInFolders, string assetName, Action<T> callback) where T : Object
        {
            var guids = AssetDatabase.FindAssets($"t:{assetName.ToLower()}", searchInFolders);
            guids.For(guid =>
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = LoadAssetAtPath<T>(path);
                if (asset != null)
                {
                    callback?.Invoke(asset);
                }
            });
        }
        static string[] _searchFolder = new string[1];
        public static void FindAssetsDatabase<T>(this string searchInFolders, Action<T> callback) where T : Object
        {
            _searchFolder[0] = searchInFolders;
            _searchFolder.FindAssetsDatabase<T>(callback);
        }
        public static void FindAssetsDatabase<T>(this string[] searchInFolders, Action<T> callback) where T : Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name.ToLower()}", searchInFolders);
            guids.For(guid =>
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = LoadAssetAtPath<T>(path);
                if (asset != null)
                {
                    callback?.Invoke(asset);
                }
            });
        }
        public static void FindGUIDAssetsDatabase<T>(this string[] searchInFolders, Action<T, string> callback = null) where T : Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name.ToLower()}", searchInFolders);
            guids.For(guid =>
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = LoadAssetAtPath<T>(path);
                if (asset != null)
                {
                    callback?.Invoke(asset, guid);
                }
            });
        }
        public static void EditPrefabAtPath(this string path, Action<GameObject> callback)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var prefab = PrefabUtility.LoadPrefabContents(path);
                callback?.Invoke(prefab);
                PrefabUtility.SaveAsPrefabAsset(prefab, path);
                PrefabUtility.UnloadPrefabContents(prefab);
            }
        }
        public static string GetPathInAtlas(SpriteAtlas atlas, string textureName)
        {
            var spriteObjects = UnityEditor.U2D.SpriteAtlasExtensions.GetPackables(atlas);
            foreach (var item in spriteObjects)
            {
                if (item.name.Equals(textureName))
                {
                    return GetAssetPath(item);
                }
            }
            return default;
        }
        public static string GetGUID(this Object item)
        {
            if (item == null) return default;
            return AssetDatabase.AssetPathToGUID(GetAssetPath(item));
        }
        public static string GetGUID(this string path)
        {
            return AssetDatabase.AssetPathToGUID(path);
        }
        public static string GetPathFromGUID(this string guid)
        {
            return AssetDatabase.GUIDToAssetPath(guid);
        }
        public static void ShowProjectFolder(this Object item)
        {
            EditorUtility.FocusProjectWindow();
            Object obj = LoadAssetAtPath<Object>(item);
            Selection.activeObject = obj;
        }
        public static void ShowProjectFolder(this string filePath)
        {
            EditorUtility.FocusProjectWindow();
            Object obj = LoadAssetAtPath<Object>(filePath);
            Selection.activeObject = obj;
        }
        public static void RenameAsset(this Object item, string newName)
        {
            //Assert.IsNotNull(item, $"item has new name {newName} is null!");
            if (!item.name.Equals(newName))
            {
                var result = GetAssetPath(item);
                AssetDatabase.RenameAsset(result, newName);
            }
        }
        public static T LoadAssetAtPath<T>(this Object item) where T : Object
        {
            var result = LoadAssetAtPath<T>(GetAssetPath(item));
            //Assert.IsNotNull(result, $"{item.name} is not stored in AssetDatabase");
            return result;
        }
        public static T LoadAssetAtPath<T>(this string path) where T : Object
        {
            var result = AssetDatabase.LoadAssetAtPath<T>(path);
            //Assert.IsNotNull(result, $"{path} is not stored in AssetDatabase");
            return result;
        }
        public static T LoadAssetByGUID<T>(this string guid) where T : Object
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var result = AssetDatabase.LoadAssetAtPath<T>(path);
            //Assert.IsNotNull(result, $"{path} is not stored in AssetDatabase");
            return result;
        }
        public static Object[] LoadAllAssetsAtPath(this string path)
        {
            return AssetDatabase.LoadAllAssetsAtPath(path);
        }
        public static string GetAssetPath(this Object item)
        {
            //Assert.IsNotNull(item, "Item is null!");
            if (!item.IsNullObjectSafe())
            {
                return AssetDatabase.GetAssetPath(item);
            }
            return string.Empty;
        }
        public static bool OpenAsset(this Object item)
        {
            //Assert.IsNotNull(item, "Item is null!");
            return AssetDatabase.OpenAsset(LoadAssetAtPath<Object>(GetAssetPath(item)));
        }
        public static bool IsSameAsset(this Object item1, Object item2)
        {
            //Assert.IsTrue(item1 == default, "Item 1 is null!");
            //Assert.IsTrue(item2 == default, "Item 2 is null!");
            bool hasNull = item1 == default || item2 == default;
            return !hasNull && GetAssetPath(item1).Equals(GetAssetPath(item2));
        }
        public static void MakeObjectDirty(this Object target, bool save = false)
        {
            EditorUtility.SetDirty(target);
            if (save)
            {
                AssetDatabase.SaveAssetIfDirty(target);
            }
            else
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
        public static T CreateScriptableObject<T>(this string path, string assetName) where T : UnityEngine.Object
        {
            var assetType = typeof(T);
            var fullPath = $"{path}/{assetName}.asset";
            var ins = ScriptableObject.CreateInstance(assetType);
            AssetDatabase.CreateAsset(ins, fullPath);
            return fullPath.LoadAssetAtPath<T>();
        }
        public static void CreateScriptableObject<T>(this string path, string assetName, Action<T> onCreated = null) where T : UnityEngine.Object
        {
            var assetType = typeof(T);
            var fullPath = $"{path}/{assetName}.asset";
            var ins = ScriptableObject.CreateInstance(assetType);
            AssetDatabase.CreateAsset(ins, fullPath);
            onCreated?.Invoke(fullPath.LoadAssetAtPath<T>());
        }
        public static void CreateSpriteAtlas(this string path, string assetName, Action<SpriteAtlas> onCreated = null)
        {
            var fullPath = $"{path}/{assetName}.spriteatlas";
            var exist = fullPath.LoadAssetAtPath<SpriteAtlas>();
            if (!exist.IsNullSafe())
            {
                EditorGUIHelper.ShowGUIConfirm($"Warning", $"The file name {assetName}.spriteatlas already exists!\nDo you want to replace?", action =>
                {
                    if (action)
                    {
                        Create();
                    }
                });
            }
            else
            {
                Create();
            }
            void Create()
            {
                var newAtlas = new SpriteAtlas();
                SpriteAtlasPackingSettings settings = new()
                {
                    enableRotation = false,
                    enableTightPacking = false,
                    padding = 4
                };
                SpriteAtlasExtensions.SetPackingSettings(newAtlas, settings);
                TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings()
                {
                    maxTextureSize = 4096,
                    format = TextureImporterFormat.Automatic,
                    compressionQuality = 50,
                    crunchedCompression = false,
                    name = "DefaultTexturePlatform"
                };
                SpriteAtlasExtensions.SetPlatformSettings(newAtlas, platformSettings);
                AssetDatabase.CreateAsset(newAtlas, fullPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                ShowProjectFolder(fullPath);
                onCreated?.Invoke(fullPath.LoadAssetAtPath<SpriteAtlas>());
            }
        }
        public static bool IsValidFolderSafe(this string path)
        {
            if (path.IsNullOrEmptySafe()) return false;
            return AssetDatabase.IsValidFolder(path);
        }
        public static string CreateFolderAndGetPath(string path, string subfoler)
        {
            return AssetDatabase.CreateFolder(path, subfoler).GetPathFromGUID();
        }
        public static string GetAssetDirectoryPath(this Object item)
        {
            var path = item.GetAssetPath();
            path = path[..(path.LastIndexOf("/"))];
            return path;
        }
    }
}
#endif