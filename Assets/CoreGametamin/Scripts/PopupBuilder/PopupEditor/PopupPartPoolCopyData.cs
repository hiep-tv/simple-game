#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Gametamin.Core
{
    [Serializable]
    public class PopupPartPoolCopyData : PopupPartPoolReferenceData
    {
        [SerializeField] PopupPartCopyType _copyType = PopupPartCopyType.Use;
        public PopupPartCopyType CopyType => _copyType;
        public PopupPartPoolCopyData() : base()
        {
        }
        public PopupPartPoolCopyData(PopupPartPoolReferenceData data) : base(data.Id, data.Target)
        {
        }
        public PopupPartPoolCopyData(GameObject target) : base(GameObjectReferenceID.Empty, target)
        {
        }
    }
}
#endif