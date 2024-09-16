using System;
using UnityEngine;

namespace Gametamin.Core
{
    [ExecuteInEditMode]
    public partial class SpriteLoader : BaseMonoBehavior
    {
        [SerializeField] string _textureName;
        [SerializeField][AtlasAddressableLabel] string _atlasName;
        [SerializeField] bool _loadOnAwake = true;
        [SerializeField] bool _loadDirect;
        ISetSprite _setSprite;
        ISetSprite _SetSprite => this.GetComponentSafe(ref _setSprite);
        bool _loaded;
        public bool LoadOnAwake => _loadOnAwake;
        Action<Sprite> _onSpriteLoaded;
        public string TextureName
        {
            get => _textureName;

#if UNITY_EDITOR
            set => _textureName = value;
#endif
        }
        public string AtlasName
        {
            get => _atlasName;
#if UNITY_EDITOR
            set
            {
                _atlasName = value;
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }
        private void Awake()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                LoadSpriteInEditorMode();
            }
            else
#endif
            if (_atlasName.IsNullOrEmptySafe() || _atlasName.IsNullOrEmptySafe())
            {
                _loadOnAwake = false;
            }
            _onSpriteLoaded = OnSpriteLoaded;
            if (!_loadDirect && _loadOnAwake)
            {
                LoadSprite();
            }
        }
        public void LoadSprite()
        {
            if (!_loaded)
            {
                LoadTextureHelper.GetSpriteByName(AtlasName, TextureName, sprite =>
                {
                    _onSpriteLoaded?.Invoke(sprite);
                });
            }
        }
        void OnSpriteLoaded(Sprite sprite)
        {
            if (!_loaded)
            {
                _loaded = true;
                SetSpriteInternal(sprite);
            }
        }
        public void SetSprite(Sprite sprite)
        {
            _loaded = true;
            SetSpriteInternal(sprite);
        }
        void SetSpriteInternal(Sprite sprite)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                RecordChange();
                if (!sprite.IsNullSafe())
                {
                    TextureName = sprite.name;
                }
            }
            if (gameObject.IsPrefabMode()) return;
#endif
            _SetSprite.OnSetSprite(sprite);
        }
        private void OnDestroy()
        {
            _onSpriteLoaded = null;
        }
    }
}
