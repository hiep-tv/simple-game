#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
namespace Gametamin.Core
{
    public partial class SpriteLoader : IForceLoadEditorMode
    {
        event Action<string> _onAtlasChanged;
        void LoadSpriteInEditorMode()
        {
            if (!gameObject.IsPrefabMode())
            {
                if (!_atlasName.IsNullOrEmptySafe())
                {
                    ForceLoadSprite();
                }
                else
                {
                    var sprite = _SetSprite.OnGetSprite();
                    if (!sprite.IsNullSafe())
                    {
                        _textureName = sprite.name;
                        this.MakeObjectDirty();
                    }
                }
            }
        }
        public void AddAtlasChangeListener(Action<string> callback)
        {
            _onAtlasChanged += callback;
        }
        public void RemoveAtlasChangeListener(Action<string> callback)
        {
            _onAtlasChanged -= callback;
        }
        public override void OnValueChanged(AttributeType attributeType)
        {
            if (attributeType == AttributeType.AtlasName)
            {
                _textureName = string.Empty;
                ForceLoadSprite();
                _onAtlasChanged?.Invoke(AtlasName);
            }
        }
        [ContextMenu("Load")]
        public void ForceLoadSprite()
        {
            if (_loadDirect)
            {
                ForceLoadSpriteDirect();
            }
            else
            {
                LoadTextureHelper.GetSpriteByName(AtlasName, TextureName, sprite =>
                {
                    SetSpriteInternal(sprite);
                });
            }
        }
        public void ForceLoadSpriteDirect()
        {
            AtlasName.GetObjectHasLabel<Sprite>(sprite =>
            {
                if (!sprite.IsNullSafe())
                {
                    if (TextureName.IsNullOrEmptySafe() || sprite.name.EqualsSafe(TextureName))
                    {
                        SetSpriteInternal(sprite);
                    }
                }
            });
        }
        protected void RecordChange()
        {
            var components = gameObject.GetComponentsSafe<MonoBehaviour>();
            Undo.RecordObjects(components, "Update Texture");
            if (PrefabUtility.IsPartOfAnyPrefab(this))
            {
                components.For(component =>
                {
                    PrefabUtility.RecordPrefabInstancePropertyModifications(component);
                });
            }
        }
        public void SetLoadDirect(bool direct)
        {
            _loadDirect = direct;
            this.MakeObjectDirty();
        }
    }
}
#endif