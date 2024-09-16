using System;
using UnityEngine;
namespace Gametamin.Core
{
    [Serializable]
    public partial class PopupData : ScriptableObject
    {
        [SerializeField] GameObject _popup;
        [SerializeField][HideInInspector] PopupPartReferenceData[] _popupPartDatas;
        [SerializeField][HideInInspector] PopupPartPoolReferenceData[] _pools;
        public GameObject Popup => _popup;
        public PopupPartReferenceData[] PopupParts => _popupPartDatas;
        public PopupPartPoolReferenceData[] PoolDatas => _pools;
        public int PartCount => _popupPartDatas.GetCountSafe();
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
