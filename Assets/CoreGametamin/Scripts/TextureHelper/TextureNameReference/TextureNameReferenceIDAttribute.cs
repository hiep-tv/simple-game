#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Gametamin.Core
{
    public class TextureNameReferenceIDAttribute : PropertyAttribute { }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TextureNameReferenceIDAttribute))]
    public class TextureNameReferenceIDEditor : BaseGenerateAttributeEditor
    {
        static readonly string Lable = "TextureNameReferenceID";
        protected override string GUILable => Lable;
        protected override string[] Values => TextureNameReferenceID.Values;
        protected override AttributeType AttributeType => AttributeType.TextureNameReferrentID;
    }
#endif
}