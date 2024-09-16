#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Gametamin.Core
{
    public partial class AtlasEditorSettings
    {
        static string[] _AtlasFolders =
        {
            "Assets"
        };
        static List<SpriteAtlas> _atlases;
        static List<SpriteAtlas> _Atlases
        {
            get
            {
                if (_atlases.GetCountSafe() <= 0)
                {
                    _atlases = new();
                    LoadAllAltases();
                }
                return _atlases;
            }
        }
        static List<AtlasConfig> _atlaseConfigs;
        static List<AtlasConfig> _AtlaseConfigs
        {
            get
            {
                if (_atlaseConfigs.GetCountSafe() <= 0)
                {
                    _atlaseConfigs = new();
                    LoadAllAtlasConfigs();
                }
                return _atlaseConfigs;
            }
        }
        static List<Sprite> _sprites;
        static List<Sprite> _Sprites
        {
            get
            {
                if (_sprites.GetCountSafe() <= 0)
                {
                    _sprites = new();
                    LoadAllSprites();
                }
                return _sprites;
            }
        }
        public static Sprite GetSprite(string spriteName)
        {
            return GetSpriteInternal(spriteName);
        }
        static Sprite GetSpriteInternal(string spriteName)
        {
            Sprite result = default;
            _Sprites.ForBreakable(item =>
            {
                var exist = item.name.EqualsSafe(spriteName);
                if (exist)
                {
                    result = item;
                }
                return exist;
            });
            if (result == null)
            {
                Debug.Log($"No sprite {spriteName} Found!");
            }
            return result;
        }
        public static SpriteAtlas GetSpriteAtlas(string atlasName)
        {
            return GetSpriteAtlasInternal(atlasName);
        }
        static SpriteAtlas GetSpriteAtlasInternal(string atlasName)
        {
            SpriteAtlas result = default;
            _Atlases.ForBreakable(atlas =>
            {
                var exist = atlas.name.EqualsSafe(atlasName);
                if (exist)
                {
                    result = atlas;
                }
                return exist;
            });
            if (result == null)
            {
                Debug.Log($"No atlas {atlasName} Found!");
            }
            return result;
        }
        public static void GetSpriteAtlasConfig(string atlasName, Action<AtlasConfig> callback)
        {
            callback?.Invoke(GetSpriteAtlasConfigInternal(atlasName));
        }
        public static AtlasConfig GetSpriteAtlasConfig(string atlasName)
        {
            return GetSpriteAtlasConfigInternal(atlasName);
        }
        static AtlasConfig GetSpriteAtlasConfigInternal(string atlasName)
        {
            AtlasConfig result = default;
            _AtlaseConfigs.ForBreakable(atlasConfig =>
            {
                var exist = false;
                if (!atlasConfig.IsNullSafe())
                {
                    exist = atlasConfig.AtlasName.EqualsSafe(atlasName);
                    if (exist)
                    {
                        result = atlasConfig;
                    }
                }
                return exist;
            });
            if (result == null)
            {
                Debug.Log($"No atlas config {atlasName} Found!");
            }
            return result;
        }
        static void LoadAllAltases()
        {
            _AtlasFolders.FindAssetDatabase<SpriteAtlas>(result =>
            {
                _atlases.Add(result);
            });
        }
        static void LoadAllAtlasConfigs()
        {
            _AtlasFolders.FindAssetDatabase<AtlasConfig>(result =>
            {
                _atlaseConfigs.Add(result);
            });
        }
        static void LoadAllSprites()
        {
            _AtlasFolders.FindAssetDatabase<Sprite>(result =>
            {
                _sprites.Add(result);
            });
        }
        public static void Reload()
        {
            _atlaseConfigs.SafeClear();
            _atlases.SafeClear();
        }
        [ContextMenu("ForceLoad")]
        static void ForceLoad()
        {
            _atlaseConfigs.SafeClear();
            _atlases.SafeClear();
            _sprites.SafeClear();
        }
    }
}
#endif
