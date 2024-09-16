#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    [CustomPropertyDrawer(typeof(ReferenceNameData))]
    public class ReferenceNameDataEditor : PropertyDrawer
    {
        protected virtual int PropertyCount => 2;
        protected virtual bool IsHorizontal => true;
        float _space = EditorGUIUtility.standardVerticalSpacing;
        protected virtual float Space => _space;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, GUIContent.none, (contentPosition) =>
            {
                OnCustomGUI(position, property);
            });
        }
        protected virtual void OnCustomGUI(Rect contentPosition, SerializedProperty property)
        {
            float width = contentPosition.width / PropertyCount;

            contentPosition.width = width - Space;
            var x = GUIPropertyField(contentPosition, property.FindPropertyRelative("_name"));

            contentPosition.x = x + Space;
            x = GUIPropertyField(contentPosition, property.FindPropertyRelative("_value"));

        }
        protected virtual float GUIPropertyField(Rect contentPosition, SerializedProperty property, bool showLabel = true)
        {
            var labelWidth = 0f;
            var width = contentPosition.width;
            if (showLabel)
            {
                var label = property.displayName;
                labelWidth = label.GetLabelWidth();
                contentPosition.width = labelWidth;
                GUI.Label(contentPosition, label);
            }
            contentPosition.x += contentPosition.width + Space;
            contentPosition.width = width - labelWidth - Space;
            EditorGUI.PropertyField(contentPosition, property, GUIContent.none);
            return contentPosition.x + contentPosition.width;
        }
    }
}
#endif