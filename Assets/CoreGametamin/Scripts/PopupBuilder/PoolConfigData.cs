using System;
using UnityEngine;

namespace Gametamin.Core
{
    [Serializable]
    public class PoolConfigData
    {
        [SerializeField] string _popupName;
#if UNITY_EDITOR
        [SerializeField] string _poolIdString;
        [SerializeField] PopupData _popupData;
        [SerializeField] string _addressableGroup;
        public string PoolIdString => _poolIdString;
        public string AddressableGroup => _addressableGroup;
        public PopupData PopupData
        {
            get => _popupData;
            set => _popupData = value;
        }
        public void SetPopup(PopupData popup, string group)
        {
            _popupData = popup;
            _addressableGroup = group;
        }
#endif
        PoolReferenceID _poolId;
        [SerializeField, PopupDataAddressableLabel] string _addressableLabel;
        public PoolReferenceID PoolId => _poolId;
        public string AddressableLabel => _addressableLabel;
        public string PopupName => _popupName;
        public PoolConfigData(PoolReferenceID poolId, string addressableLabel)
        {
            _poolId = poolId;
            _addressableLabel = addressableLabel;
            _popupName = $"{poolId}Popup";
        }
#if UNITY_EDITOR
        public PoolConfigData(string popupName, string poolId, string addressableLabel, string group)
        {
            _poolIdString = poolId;
            _addressableLabel = addressableLabel;
            _popupName = $"{popupName}Popup";
            _addressableGroup = group;
        }
#endif
    }
}
