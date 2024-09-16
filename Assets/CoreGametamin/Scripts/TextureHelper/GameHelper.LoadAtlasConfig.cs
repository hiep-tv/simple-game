using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class LoadTextureHelper
    {
        public static void GetAtlasConfig(this string atlasName, string otherAtlasName, Action<AtlasConfig, AtlasConfig> callback = null)
        {
            var loadCount = 2;
            AtlasConfig atlas1 = default, atlas2 = default;
            atlasName.GetAtlasConfig(result =>
            {
                atlas1 = result;
                OnLoaded();
            });
            otherAtlasName.GetAtlasConfig(result =>
            {
                atlas2 = result;
                OnLoaded();
            });
            void OnLoaded()
            {
                loadCount--;
                if (loadCount <= 0)
                {
                    callback?.Invoke(atlas1, atlas2);
                }
            }
        }
        public static void GetAtlasConfig(this string atlasName, Action<AtlasConfig> callback)
        {
            if (atlasName.IsNullOrEmptySafe())
            {
                callback?.Invoke(default);
                return;
            }
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                var config = AtlasEditorSettings.GetSpriteAtlasConfig(atlasName);
                callback?.Invoke(config);
            }
            else
#endif
            {
                AtlasConfigCacher.GetAtlasConfig(atlasName, config =>
                {
                    callback?.Invoke(config);
                });
            }
        }
    }
}
