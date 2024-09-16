#if UNITY_EDITOR
using UnityEngine;

namespace Gametamin.Core
{
    public partial class BaseMonoBehavior : MonoBehaviour, IAttributeValueChanged
    {
        public virtual void OnValueChanged(AttributeType attributeType)
        {

        }
    }
}
#endif