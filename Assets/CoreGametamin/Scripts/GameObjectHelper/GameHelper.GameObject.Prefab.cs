#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class GameObjectHelper
    {
        const string _variantName = "NewPrefab";
        public static GameObject CreateVariantsPrefab(this GameObject origin, string folder, string variantsName = _variantName)
        {
            GameObject objSource = PrefabUtility.InstantiatePrefab(origin) as GameObject;
            var variant = PrefabUtility.SaveAsPrefabAsset(objSource, $"{folder}/{variantsName}.prefab");
            Object.DestroyImmediate(objSource);
            return variant;
        }
        public static GameObject CreateCopyPrefab(this GameObject origin, string folder, string variantsName = _variantName)
        {
            GameObject objSource = PrefabUtility.InstantiatePrefab(origin) as GameObject;
            PrefabUtility.UnpackPrefabInstance(objSource, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            var variant = PrefabUtility.SaveAsPrefabAsset(objSource, $"{folder}/{variantsName}.prefab");
            Object.DestroyImmediate(objSource);
            return variant;
        }
        public static bool IsPrefabMode(this GameObject prefab)
        {
            return EditorSceneManager.IsPreviewSceneObject(prefab);
        }
    }
}
#endif