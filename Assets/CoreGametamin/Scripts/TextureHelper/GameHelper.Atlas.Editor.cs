#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Gametamin.Core
{
    public static partial class AtlasHelper
    {
        static List<Object> _packables;
        static List<Object> _Packables => _packables ??= new();
        public static Sprite GetSpriteInAsset(this string spriteName)
        {
            return AtlasEditorSettings.GetSprite(spriteName);
        }
        public static Sprite GetSpritePackable(this SpriteAtlas atlas, string spriteName)
        {
            Sprite result = default;
            var spriteObjects = atlas.GetPackables();
            spriteObjects.ForBreakable((item, index) =>
            {
                bool exist = item.name.Equals(spriteName);
                if (exist)
                {
                    result = item.LoadAssetAtPath<Sprite>();
                }
                return exist;
            });
            return result;
        }
        public static void LoadTextureFromAtlas(GameObject item)
        {
            var iloader = item.GetComponentSafe<SpriteLoader>();
            var sprite = GetTextureInSpriteAtlas(iloader.AtlasName, iloader.TextureName);
            iloader.SetSprite(sprite);
        }
        public static void LoadTextureFromAtlas(this GameObject target, string textureName, string atlasName)
        {
            var sprite = GetTextureInSpriteAtlas(atlasName, textureName);
            var isetSprite = target.GetComponentSafe<SpriteLoader>();
            isetSprite.SetSprite(sprite);
        }
        static Sprite GetTextureInSpriteAtlas(this string atlasName, string textureName)
        {
            var atlas = GetSpriteAtlasInAssetDatabase(atlasName);
            Assert.IsNotNull(atlas, $"No atlas has name {atlasName} found!");
            if (atlas != null)
            {
                Sprite result = default;
                var packables = atlas.GetPackables();
                packables.ForBreakable(item =>
                {
                    if (item.name.Equals(textureName))
                    {
                        result = item.LoadAssetAtPath<Sprite>();
                        return true;
                    }
                    return false;
                });
                Assert.IsNotNull(result, $"No texture has name {textureName} found atlas {atlasName}!");
                return result;
            }
            return default;
        }
        public static UnityEngine.Object[] GetPackables(this string atlasName)
        {
            var atlas = GetSpriteAtlasInAssetDatabase(atlasName);
            if (!atlas.IsNullSafe())
            {
                return atlas.GetPackables();
            }
            return default;
        }
        public static void GetTextures(this string atlasName, List<Sprite> results)
        {
            GetTextures(atlasName, results, SearchSpriteAtlasInFolders);
        }
        public static void GetTextures(this string atlasName, List<Sprite> results, string[] paths)
        {
            var atlas = GetSpriteAtlasInAssetDatabase(atlasName);
            if (!atlas.IsNullSafe())
            {
                GetTextures(atlas, results);
            }
        }
        public static void GetTextures(this SpriteAtlas atlas, List<Sprite> results)
        {
            if (!atlas.IsNullSafe())
            {
                var packables = atlas.GetPackables();
                packables.For(item => results.Add(item.LoadAssetAtPath<Sprite>()));
            }
        }
        public static void AddTexturesToAtlas(this SpriteAtlas atlas, string textureFolder, System.Action callback = null)
        {
            if (!atlas.IsNullSafe() && !textureFolder.IsNullOrEmptySafe())
            {
                _Packables.Clear();
                textureFolder.FindAssetsDatabase<Texture>(result =>
                {
                    _Packables.Add(result);
                });
                var array = _Packables.ToArray();
                atlas.AddTexturesToAtlas(array, callback);
            }
        }
        public static void AddTexturesToAtlas(this SpriteAtlas atlas, Object[] packables, System.Action callback = null)
        {
            if (atlas.IsNullSafe())
            {
                return;
            }
            _Packables.Clear();
            if (atlas != null)
            {
                var currentPackables = atlas.GetPackables();
                atlas.Remove(currentPackables);
                EditorGUIHelper.DelayEditor(.2f, TryAdd);
                void TryAdd()
                {
                    _Packables.AddRange(currentPackables);
                    packables.For(item1 =>
                    {
                        var exist = false;
                        _Packables.ForBreakable(item2 =>
                        {
                            exist = item2.name.Equals(item1.name);
                            return exist;
                        });
                        if (!exist)
                        {
                            _Packables.Add(item1);
                        }
                    });
                    atlas.Add(_Packables.ToArray());
                    callback?.Invoke();
                }
            }
        }
        public static void CleanAtlas(this SpriteAtlas atlas, System.Action callback = null)
        {
            if (atlas.IsNullSafe())
            {
                return;
            }
            _Packables.Clear();
            if (atlas != null)
            {
                var packables = atlas.GetPackables();
                atlas.Remove(packables);
                packables.For(item =>
                {
                    if (!item.IsNullSafe())
                    {
                        var exist = false;
                        var itemGUID = item.GetGUID();
                        _Packables.ForBreakable(item2 =>
                        {
                            if (!item2.IsNullSafe())
                            {
                                var item2GUID = item2.GetGUID();
                                exist = itemGUID.EqualsSafe(item2GUID);
                            }
                            return exist;
                        });
                        if (!exist)
                        {
                            _Packables.Add(item);
                        }
                    }
                });
                EditorGUIHelper.DelayEditor(.2f, () =>
                {
                    atlas.Add(_Packables.ToArray());
                    _Packables.Clear();
                    SpriteAtlas[] spriteAtlasses = { atlas };
                    SpriteAtlasUtility.PackAtlases(spriteAtlasses, EditorUserBuildSettings.activeBuildTarget);
                    EditorGUIHelper.DelayEditor(.2f, callback);
                });
            }
        }
        public static void ClearAtlas(this SpriteAtlas atlas, System.Action callback = null)
        {
            if (!atlas.IsNullSafe())
            {
                var packables = atlas.GetPackables();
                atlas.Remove(packables);
                EditorGUIHelper.DelayEditor(.2f, callback);
            }
        }
        public static void RemoveTexturesFromAtlas(this SpriteAtlas atlas, params Object[] packables)
        {
            if (!atlas.IsNullSafe())
            {
                atlas.Remove(packables);
            }
        }
        static readonly string[] SearchSpriteAtlasInFolders = { "Assets/Gametamin.Core/" };
        public static SpriteAtlas GetSpriteAtlasInAssetDatabase(this string atlasName)
        {
            return AtlasEditorSettings.GetSpriteAtlas(atlasName);
        }
        public static AtlasConfig GetAtlasConfigInAssetDatabase(this string atlasName)
        {
            return AtlasEditorSettings.GetSpriteAtlasConfig(atlasName);
        }
    }
}
#endif