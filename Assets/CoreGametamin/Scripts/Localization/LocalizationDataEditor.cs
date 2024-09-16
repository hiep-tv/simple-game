#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core.Localization
{
    [CustomPropertyDrawer(typeof(TextAssetData), true)]
    public class TextAssetDataEditor : PropertyDrawer
    {
        static GUIStyle style = new(EditorStyles.textArea)
        {
            wordWrap = true,
            stretchHeight = true
        };
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, GUIContent.none, (contentPosition) =>
            {
                var space = EditorGUIUtility.standardVerticalSpacing;
                var widthID = contentPosition.width / 4f;
                var widthText = contentPosition.width - widthID - space;
                contentPosition.width = widthID;
                var textID = property.FindPropertyRelative("_textID");
                EditorGUIHelper.GUIStringWithSearch(contentPosition, "TextID", textID.stringValue, TextReferenceID.Values, (value, index) =>
                {
                    textID.stringValue = value;
                    textID.serializedObject.ApplyModifiedProperties();
                });
                contentPosition.x += contentPosition.width + space;
                contentPosition.width = widthText;
                var text = property.FindPropertyRelative("_text");
                var height = style.CalcHeight(new GUIContent(text.stringValue), contentPosition.width);
                contentPosition.height = height;
                text.stringValue = EditorGUI.TextArea(contentPosition, text.stringValue, style);
            });
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var text = property.FindPropertyRelative("_text");
            return style.CalcSize(new GUIContent(text.stringValue)).y;
        }
    }
}
#endif