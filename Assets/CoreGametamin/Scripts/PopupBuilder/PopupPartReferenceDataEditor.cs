#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace Gametamin.Core
{
    [CustomPropertyDrawer(typeof(PopupPartReferenceData), true)]
    public class PopupPartReferenceDataEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, GUIContent.none, (contentPosition) =>
            {
                contentPosition.x += 5;
                contentPosition.width -= 5;
                OnCustomGUI(contentPosition, property);
            });
        }
        protected virtual void OnCustomGUI(Rect contentPosition, SerializedProperty property)
        {
            var space = EditorGUIUtility.standardVerticalSpacing;
            var width = contentPosition.width / 3;
            contentPosition.width = width - space;
            contentPosition.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("_target"), GUIContent.none);
            //contentPosition.y += EditorGUIUtility.singleLineHeight + space;
            contentPosition.x += contentPosition.width + space;
            var id = property.FindPropertyRelative("_id");

            contentPosition.width = EditorGUIHelper.GetLabelWidth("ID");
            EditorGUIHelper.GUILabel(contentPosition, "ID");
            contentPosition.x += contentPosition.width + space;
            contentPosition.width = width - contentPosition.width - space;
            EditorGUIHelper.GUIStringWithSearch(contentPosition, "GameObjectReferenceID", id, GameObjectReferenceID.Values);

            contentPosition.x += contentPosition.width + space;
            var parentID = property.FindPropertyRelative("_parentID");
            contentPosition.width = EditorGUIHelper.GetLabelWidth("Parent");
            EditorGUIHelper.GUILabel(contentPosition, "Parent");
            contentPosition.x += contentPosition.width + space;
            contentPosition.width = width - contentPosition.width - space;
            EditorGUIHelper.GUIStringWithSearch(contentPosition, "GameObjectReferenceID", parentID, GameObjectReferenceID.Values);
        }
        //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        //{
        //    if (property == null) return EditorGUIUtility.singleLineHeight;
        //    return EditorGUIUtility.singleLineHeight * 2f + EditorGUIUtility.standardVerticalSpacing * 2f;
        //}
    }
}
#endif