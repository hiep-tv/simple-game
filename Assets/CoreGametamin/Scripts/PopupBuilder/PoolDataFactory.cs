#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public class PoolDataFactory : BaseDataFactory
    {
        [SerializeField] List<PoolConfigData> _poolConfigs;
        public List<PoolConfigData> PoolConfigs => _poolConfigs;
        public override bool AddName(string newName, string value)
        {
            return true;
        }
        public override bool IsNameExist(string newName)
        {
            var exist = false;
            _poolConfigs.ForBreakable(popup =>
            {
                if (newName.EqualsSafe(popup.PopupName))
                {
                    exist = true;
                }
                return exist;
            });
            return exist;
        }
        public void AddPopup(PoolConfigData configData)
        {
            PoolConfigs.Add(configData);
            this.MakeObjectDirty(true);
        }
        [ContextMenu("LoadPopup")]
        public void LoadPopup()
        {
            _poolConfigs.For(item =>
            {
                item.AddressableLabel.GetObjectHasLabel<PopupData>((result, group) =>
                {
                    item.SetPopup(result, group);
                });
            });
            this.MakeObjectDirty(true);
        }
        [ContextMenu("Mark Addressable")]
        public void MarkAddressable()
        {
            _poolConfigs.For(item =>
            {
                item.PopupData.CreateAssetEntry(item.AddressableGroup, item.AddressableLabel);
            });
        }
        public void AddPopup(PopupData popupData, string label)
        {
            _poolConfigs.ForBreakable(item =>
            {
                var exist = item.AddressableLabel.EqualsSafe(label);
                if (exist)
                {
                    item.PopupData = popupData;
                }
                return exist;
            });
        }
    }
}
#endif