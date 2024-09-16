using System;
using UnityEngine;
using UnityEngine.U2D;

namespace Gametamin.Core
{
    public static partial class AtlasHelper
    {
        public static Sprite GetSpriteSafe(this SpriteAtlas atlas, string spriteName)
        {
            //Debug.Log($"atlas={atlas}, spriteName={spriteName}");
            if (atlas != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    return atlas.GetSpritePackable(spriteName);
                }
#endif
                return atlas.GetSprite(spriteName);
            }
            return default;
        }
    }
}
