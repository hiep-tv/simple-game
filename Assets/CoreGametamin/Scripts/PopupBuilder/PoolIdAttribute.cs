#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
namespace Gametamin.Core
{
    public class PoolIdAttribute : PropertyAttribute { }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(PoolIdAttribute))]
    public class PoolIdAttributeEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, label, (contentPosition) =>
            {
                EditorGUIHelper.GUIEnumWithSearch<PoolReferenceID>(contentPosition, property);
            });
        }
    }
#endif
}