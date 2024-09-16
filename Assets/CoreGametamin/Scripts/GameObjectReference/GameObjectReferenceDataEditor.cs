#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    [CustomPropertyDrawer(typeof(GameObjectReferenceData), true)]
    public class GameObjectReferenceDataEditor : PropertyDrawer
    {
        protected virtual bool ShowIgnoreProperty => true;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, GUIContent.none, (contentPosition) =>
            {
                contentPosition.x += 5;
                contentPosition.width -= 5;
                OnCustomGUI(contentPosition, property);
            });
        }
        static GUIContent _ignore;
        static GUIContent _Ignore
        {
            get
            {
                if (_ignore == null)
                {
                    _ignore = new GUIContent("Ignore")
                    {
                        tooltip = "Ignore copy to parent"
                    };
                }
                return _ignore;
            }
        }

        protected virtual void OnCustomGUI(Rect contentPosition, SerializedProperty property)
        {
            var space = EditorGUIUtility.standardVerticalSpacing;
            float width = contentPosition.width / 2;
            if (ShowIgnoreProperty)
            {
                width -= EditorGUIHelper.GetLabelWidth(_Ignore);
            }
            contentPosition.width = width - space;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("_target"), GUIContent.none);
            contentPosition.x += width + space;
            var id = property.FindPropertyRelative("_id");

            var labelWidth = EditorGUIHelper.GetLabelWidth("ID");
            contentPosition.width = labelWidth;
            EditorGUIHelper.GUILabel(contentPosition, "ID");
            contentPosition.x += contentPosition.width + space;
            contentPosition.width = width - contentPosition.width - space / 2f;
            //var currentWidth = contentPosition.width;
            var labelId = typeof(GameObjectReferenceID).Name;
            //contentPosition.width = Mathf.Max(currentWidth, labelId.GetLabelWidth());
            EditorGUIHelper.GUIStringWithSearch(contentPosition, labelId, id, GameObjectReferenceID.Values);

            if (ShowIgnoreProperty)
            {
                contentPosition.x += contentPosition.width + space;
                var ignoreCopy = property.FindPropertyRelative("_ignoreCopy");
                labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = EditorGUIHelper.GetLabelWidth(_Ignore);
                EditorGUI.PropertyField(contentPosition, ignoreCopy, _Ignore);
                EditorGUIUtility.labelWidth = labelWidth;
            }
        }
    }
}
#endif
