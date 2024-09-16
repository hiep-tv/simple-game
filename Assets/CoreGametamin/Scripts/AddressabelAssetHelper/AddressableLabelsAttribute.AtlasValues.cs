#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Gametamin.Core
{
    public class AtlasAddressableLabelAttribute : PropertyAttribute { }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AtlasAddressableLabelAttribute))]
    public class AtlasAddressableLabelAttributeEditor : BaseGenerateAttributeEditor
    {
        static readonly string Lable = "Addressable Label";
        protected override string GUILable => Lable;
        protected override string[] Values => AddressableLabels.AtlasValues;
        protected override AttributeType AttributeType => AttributeType.AtlasName;
    }
#endif
}