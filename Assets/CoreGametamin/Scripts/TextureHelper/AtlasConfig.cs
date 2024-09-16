using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Gametamin.Core
{
    public partial class AtlasConfig : ScriptableObject
    {
        [SerializeField] bool _immortal;
        public bool Immortal => _immortal;
        [SerializeField] SpriteAtlas _atlas;
        [SerializeField] List<TextureNameReferenceData> _textureNames;
        public SpriteAtlas Atlas
        {
            get => _atlas;
            set => _atlas = value;
        }
        public List<TextureNameReferenceData> TextureNames => _textureNames ??= new();
        public Sprite GetSpriteById(string spriteId)
        {
            var spriteName = LoadTextureHelper.GetTextureName(TextureNames, spriteId);
            if (!spriteName.IsNullOrEmptySafe())
            {
                return GetSpriteByName(spriteName);
            }
            return default;
        }
        public Sprite GetSpriteById(string spriteId, params object[] @params)
        {
            var spriteName = LoadTextureHelper.GetTextureName(TextureNames, spriteId);
            if (!spriteName.IsNullOrEmptySafe())
            {
                return GetSpriteByName(string.Format(spriteName, @params));
            }
            return default;
        }
        public Sprite GetSpriteByName(string spriteName)
        {
            return Atlas.GetSpriteSafe(spriteName);
        }
        static List<Sprite> _SpriteCacheList = new();
        public Sprite[] GetSpritesById(string id, int count, int start = 0)
        {
            _SpriteCacheList.SafeClear();
            var spriteNameFormat = LoadTextureHelper.GetTextureName(TextureNames, id);
            for (int i = start; i < count + start; i++)
            {
                var spriteName = string.Format(spriteNameFormat, i);
                var sprite = GetSpriteByName(spriteName);
                if (sprite != null)
                {
                    _SpriteCacheList.Add(sprite);
                }
            }
            return _SpriteCacheList.ToArray();
        }
    }
}
