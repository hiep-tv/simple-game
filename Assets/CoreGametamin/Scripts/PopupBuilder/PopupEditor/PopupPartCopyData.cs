#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Gametamin.Core
{
    public enum PopupPartCopyType
    {
        [InspectorName("Not Use")]
        Non = -1,
        Use,
        [InspectorName("Make Variant")]
        Variant,
        Duplicate
    }
    [Serializable]
    public class PopupPartCopyData : PopupPartReferenceData
    {
        [SerializeField] PopupPartCopyType _copyType = PopupPartCopyType.Use;
        public PopupPartCopyType CopyType => _copyType;
        public PopupPartCopyData(PopupPartReferenceData partReferenceData)
            : base(partReferenceData.Id, partReferenceData.Target, partReferenceData.ParentID)
        {
        }
        public PopupPartCopyData(GameObject target)
            : base(GameObjectReferenceID.Empty, target, GameObjectReferenceID.Empty)
        {
        }
    }
}
#endif
