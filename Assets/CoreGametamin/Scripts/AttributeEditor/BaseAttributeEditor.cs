#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
namespace Gametamin.Core
{
#if UNITY_EDITOR
    public abstract class BaseGenerateAttributeEditor : PropertyDrawer
    {
        protected abstract AttributeType AttributeType { get; }
        protected abstract string GUILable { get; }
        protected abstract string[] Values { get; }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, label, (contentPosition) =>
            {
                EditorGUIHelper.GUIStringWithSearch(contentPosition, GUILable, property.stringValue
                , Values
                , (value, index) =>
                {
                    property.stringValue = value;
                    property.serializedObject.ApplyModifiedProperties();
                    var target = property.serializedObject.targetObject;
                    if (target is IAttributeValueChanged iattribute)
                    {
                        iattribute.OnValueChanged(AttributeType);
                    }
                    else
                    {
                        Debug.Log("error");
                    }
                });
            });
        }
    }
    public abstract class BaseEnumAttributeEditor<T> : PropertyDrawer where T : struct
    {
        protected abstract AttributeType AttributeType { get; }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, label, (contentPosition) =>
            {
                EditorGUIHelper.GUIEnumWithSearch<T>(contentPosition, property, () =>
                {
                    var target = property.serializedObject.targetObject;
                    if (target is IAttributeValueChanged iattribute)
                    {
                        iattribute.OnValueChanged(AttributeType);
                    }
                    else
                    {
                        Debug.Log("error");
                    }
                });
            });
        }
    }
#endif
}