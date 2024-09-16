using System;
using UnityEngine;

namespace Gametamin.Core
{
    [ExecuteAlways]
    public partial class SpriteDirectLoader : MonoBehaviour
    {
        SpriteLoader _loader;
        SpriteLoader _Loader => gameObject.GetComponentSafe(ref _loader);
        bool _loaded;
        private void Awake()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (!gameObject.IsPrefabMode())
                {
                    //ForceLoadSprite();
                }
            }
            else
#endif
            if (_Loader.LoadOnAwake)
            {
                LoadSprite();
            }
        }
        public void LoadSprite(Action onLoaded = null)
        {
            if (!_loaded)
            {
                _Loader.AtlasName.LoadAddressableAssetAsync<Sprite>(handler =>
                {
                    if (!_loaded)
                    {
                        _loaded = true;
                        if (handler.IsDone)
                        {
                            SetSpriteInternal(handler.Result);
                        }
                    }
                    onLoaded?.Invoke();
                });
            }
            else
            {
                onLoaded?.Invoke();
            }
        }
        public void SetSprite(Sprite sprite)
        {
            _loaded = true;
            SetSpriteInternal(sprite);
        }
        void SetSpriteInternal(Sprite sprite)
        {
            _Loader.SetSprite(sprite);
        }
    }
}