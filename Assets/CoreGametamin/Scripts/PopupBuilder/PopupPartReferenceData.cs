using System;
using UnityEngine;

namespace Gametamin.Core
{
    [Serializable]
    public class PopupPartReferenceData : GameObjectReferenceData
    {
        [SerializeField] string _parentID;
        public string ParentID => _parentID;
        public PopupPartReferenceData()
        {

        }
        public PopupPartReferenceData(string id, GameObject target, string parentID)
            : base(id, target)
        {
            _parentID = parentID;
        }
        public PopupPartReferenceData(PopupPartReferenceData other) : base(other)
        {
            _parentID = other.ParentID;
        }
        public void SetData(string parentID, string id, GameObject target)
        {
            base.SetData(id, target);
            _parentID = parentID;
        }
        public bool Equals(PopupPartReferenceData other)
        {
            return ParentID.EqualsSafe(other.ParentID) && Id.EqualsSafe(other.Id);
        }
    }

    [Serializable]
    public class PopupPartDirectReferenceData
    {
        [AtlasAddressableLabel]
        [SerializeField] string _addressableLabel;
        [SerializeField] string _parentID;
        public string ParentID => _parentID;
        public string AddressableLabel => _addressableLabel;
    }
}
