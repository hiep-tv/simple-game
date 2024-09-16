using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class LoadTextureHelper
    {
        public static void SetSpriteById(this GameObject target, string atlasName, string textureId)
        {
            atlasName.GetAtlasConfig(atlasConfig =>
            {
                target.SetSpriteById(atlasConfig, textureId);
            });
        }
        public static void SetSpriteByName(this GameObject target, string atlasName, string textureName)
        {
            atlasName.GetAtlasConfig(atlasConfig =>
            {
                target.SetSpriteByName(atlasConfig, textureName);
            });
        }
        public static void SetSpriteById(this GameObject target, AtlasConfig atlasConfig, string textureId)
        {
            var sprite = atlasConfig.GetSpriteByIdSafe(textureId);
            target.SetSpriteSafe(sprite);
        }
        public static void SetSpriteByName(this GameObject target, AtlasConfig atlasConfig, string textureName)
        {
            var sprite = atlasConfig.GetSpriteByNameSafe(textureName);
            target.SetSpriteSafe(sprite);
        }
        public static void SetSpriteSafe(this GameObject target, Sprite sprite)
        {
            if (!target.IsNullSafe() && !sprite.IsNullSafe())
            {
                var spriteLoader = target.GetComponentSafe<SpriteLoader>();
                spriteLoader.SetSpriteSafe(sprite);
            }
        }
        public static void SetSpriteSafe(this SpriteLoader spriteLoader, Sprite sprite)
        {
            if (!spriteLoader.IsNullSafe() && !sprite.IsNullSafe())
            {
                spriteLoader.SetSprite(sprite);
            }
        }
        public static void GetSpriteById(string altasName, string textureId, Action<Sprite> callback)
        {
            GetAtlasConfig(altasName, atlasConfig =>
            {
                callback?.Invoke(atlasConfig.GetSpriteByIdSafe(textureId));
            });
        }
        public static void GetSpriteByName(string altasName, string textureName, Action<Sprite> callback)
        {
            GetAtlasConfig(altasName, atlasConfig =>
            {
                callback?.Invoke(atlasConfig.GetSpriteByNameSafe(textureName));
            });
        }
        public static Sprite GetSpriteByIdSafe(this AtlasConfig atlasConfig, string textureId)
        {
            if (atlasConfig != null)
            {
                return atlasConfig.GetSpriteById(textureId);
            }
            return default;
        }
        public static Sprite GetSpriteByNameSafe(this AtlasConfig atlasConfig, string textureName)
        {
            return atlasConfig.GetSpriteByName(textureName);
        }

        public static string GetTextureName(List<TextureNameReferenceData> datas, string id)
        {
            string result = default;
            datas.ForBreakable(data =>
            {
                if (data.Id.EqualsSafe(id))
                {
                    result = data.TextureName;
                    return true;
                }
                return false;
            });
            return result;
        }
    }
}
