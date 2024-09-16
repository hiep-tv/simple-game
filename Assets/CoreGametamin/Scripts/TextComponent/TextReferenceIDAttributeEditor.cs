#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
namespace Gametamin.Core
{
    public class TextReferenceIDAttribute : PropertyAttribute { }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TextReferenceIDAttribute))]
    public class TextReferenceIDAttributeEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, label, (contentPosition) =>
            {
                EditorGUIHelper.GUIStringWithSearch(contentPosition, "Text ID", property.stringValue
                , TextReferenceID.Values
                , (value, index) =>
                {
                    property.stringValue = value;
                    property.serializedObject.ApplyModifiedProperties();
                });
            });
        }
    }
#endif
}