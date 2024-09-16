#if UNITY_EDITOR
using System;
using UnityEngine;
namespace Gametamin.Core
{
    [Serializable]
    public partial class PopupCopyData : ScriptableObject
    {
        [SerializeField] GameObject _popup;
        [SerializeField] PopupPartCopyData[] _popupPartDatas;
        [SerializeField] PopupPartPoolCopyData[] _pools;
        public GameObject Popup => _popup;
        public PopupPartCopyData[] PopupParts => _popupPartDatas;
        public PopupPartPoolCopyData[] PoolDatas => _pools;
        public void SetData(PopupData popupData)
        {
            _popup = popupData.Popup;
            CopyPart(popupData.PopupParts);
            CopyPartPool(popupData.PoolDatas);
        }
        void CopyPart(PopupPartReferenceData[] popupParts)
        {
            _popupPartDatas = new PopupPartCopyData[popupParts.GetCountSafe()];
            popupParts.For((item, index) =>
            {
                _popupPartDatas[index] = new PopupPartCopyData(item);
            });
        }
        void CopyPartPool(PopupPartPoolReferenceData[] popupParts)
        {
            _pools = new PopupPartPoolCopyData[popupParts.GetCountSafe()];
            popupParts.For((item, index) =>
            {
                _pools[index] = new PopupPartPoolCopyData(item);
            });
        }
        public GameObject GetGameObject(string id)
        {
            GameObject result = default;
            _popupPartDatas.ForBreakable(data =>
            {
                var exist = data.Id.EqualsSafe(id);
                if (exist)
                {
                    result = data.Target;
                }
                return exist;
            });
            return result;
        }
    }
}
#endif