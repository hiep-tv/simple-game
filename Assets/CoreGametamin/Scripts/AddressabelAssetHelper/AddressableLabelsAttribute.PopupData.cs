#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Gametamin.Core
{
    public class PopupDataAddressableLabelAttribute : PropertyAttribute { }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(PopupDataAddressableLabelAttribute))]
    public class PopupDataAddressableLabelAttributeEditor : BaseGenerateAttributeEditor
    {
        static readonly string Lable = "Addressable Label";
        protected override string GUILable => Lable;
        protected override string[] Values => AddressableLabels.PopupValues;
        protected override AttributeType AttributeType => AttributeType.PopupData;
    }
#endif
}