#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Gametamin.Core
{
    [Serializable]
    public class PoolConfigSaveData
    {
        [SerializeField] string _popupName;
        [SerializeField] string _poolIdString;
        PoolReferenceID _poolId;
        [SerializeField, PopupDataAddressableLabel] string _addressableLabel;
        [SerializeField] PopupData _popupData;
        public PoolReferenceID PoolId => _poolId;
        public string AddressableLabel => _addressableLabel;
        public string PopupName => _popupName;
        public string PoolIdString => _poolIdString;
        public PoolConfigSaveData(PoolReferenceID poolId, string addressableLabel)
        {
            _poolId = poolId;
            _addressableLabel = addressableLabel;
            _popupName = $"{poolId}Popup";
        }
        public PoolConfigSaveData(string popupName, string poolId, string addressableLabel)
        {
            _poolIdString = poolId;
            _addressableLabel = addressableLabel;
            _popupName = $"{popupName}Popup";
        }
    }
}
#endif