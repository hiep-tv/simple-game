#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    [CustomPropertyDrawer(typeof(PopupPartCopyData), true)]
    public class PopupPartCopyDataEditor : PopupPartReferenceDataEditor
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, GUIContent.none, (contentPosition) =>
            {
                var width = contentPosition.width / 5f;
                contentPosition.width = width * 4f;
                OnCustomGUI(contentPosition, property);
                var ignoreCopy = property.FindPropertyRelative("_copyType");
                var space = EditorGUIUtility.standardVerticalSpacing * 4;
                contentPosition.x += contentPosition.width + space;
                contentPosition.width = width - space;
                EditorGUI.PropertyField(contentPosition, ignoreCopy, GUIContent.none);
            });
        }
    }
}
#endif
