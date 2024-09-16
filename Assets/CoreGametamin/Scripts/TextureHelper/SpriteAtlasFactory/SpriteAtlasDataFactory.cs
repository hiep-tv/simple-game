#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public class SpriteAtlasDataFactory : BaseDataFactory
    {
        [SerializeField] List<SpriteAtlasConfigData> _spriteAtlasConfigData;
        public List<SpriteAtlasConfigData> SpriteAtlasConfigData => _spriteAtlasConfigData ??= new();
        public override bool AddName(string newName, string value)
        {
            return true;
        }

        public override bool IsNameExist(string newName)
        {
            var exist = false;
            _spriteAtlasConfigData.ForBreakable(popup =>
            {
                if (newName.EqualsSafe(popup.AddressableLabel, System.StringComparison.OrdinalIgnoreCase))
                {
                    exist = true;
                }
                return exist;
            });
            return exist;
        }
        [ContextMenu("Load")]
        void LoadFromSystem()
        {
            SpriteAtlasConfigData.Clear();
            AddressabelAssetHelper.GetObjectHasLabel<AtlasConfig>((item, label, group) =>
            {
                AddAtlasConfig(item, label, group);
            });
            this.MakeObjectDirty(true);
        }
        public void AddAtlasConfig(AtlasConfig item, string label, string group)
        {
            SpriteAtlasConfigData.Add(new SpriteAtlasConfigData(item, label, group));
        }
        [ContextMenu("Mark Addressable")]
        public void MarkAddressable()
        {
            SpriteAtlasConfigData.For(item =>
            {
                item.AtlasConfig.CreateAssetEntry(item.AddressableGroup, item.AddressableLabel);
                item.AtlasConfig.Atlas.CreateAssetEntry(item.AddressableGroup, item.AddressableLabel);
            });
        }
    }
}
#endif