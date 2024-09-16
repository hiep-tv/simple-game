#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace Gametamin.Core
{
    public static partial class AtlasHelper
    {
        public static void CreateNewAtlas(string _saveFolder, string _atlasName, System.Action<AtlasConfig> onCreated = null, string textureFolder = null)
        {
            AssetDatabaseHelper.CreateSpriteAtlas(_saveFolder, _atlasName, (atlas) =>
            {
                if (atlas != null)
                {
                    PackToAtlas(atlas, textureFolder, () =>
                    {
                        AssetDatabaseHelper.CreateScriptableObject<AtlasConfig>(_saveFolder, $"{_atlasName}config", config =>
                        {
                            if (config != null)
                            {
                                config.Atlas = atlas;
                                config.MakeObjectDirty(true);
                                onCreated?.Invoke(config);
                            }
                        });
                    });
                }
            });
        }
        public static void PackToAtlas(SpriteAtlas atlas, string textureFolder, System.Action onCreated = null)
        {
            if (!atlas.IsNullSafe() && !textureFolder.IsNullOrEmptySafe())
            {
                List<Object> packable = new();
                textureFolder.FindAssetsDatabase<Texture>(result =>
                {
                    packable.Add(result);
                });
                atlas.AddTexturesToAtlas(packable.ToArray(), onCreated);
            }
        }
    }
}
#endif
