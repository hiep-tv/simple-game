
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    [CustomPropertyDrawer(typeof(TextureNameReferenceData), true)]
    public class TextureNameReferenceDataEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, GUIContent.none, (contentPosition) =>
            {
                var space = EditorGUIUtility.standardVerticalSpacing;
                var widthID = contentPosition.width / 4f;
                var widthText = contentPosition.width - widthID - space;
                contentPosition.width = widthID;
                var id = property.FindPropertyRelative("_id");
                EditorGUI.PropertyField(contentPosition, id, GUIContent.none);
                contentPosition.x += contentPosition.width + space;
                contentPosition.width = widthText;
                var text = property.FindPropertyRelative("_textureName");
                EditorGUI.PropertyField(contentPosition, text, GUIContent.none);
            });
        }
    }
}
#endif