using System;
using UnityEngine;

namespace Gametamin.Core
{
    [Serializable]
    public class PopupPartPoolReferenceData : GameObjectReferenceData
    {
        public PopupPartPoolReferenceData() : base()
        {

        }
        public PopupPartPoolReferenceData(string id, GameObject target) : base(id, target)
        {
        }
    }
}
