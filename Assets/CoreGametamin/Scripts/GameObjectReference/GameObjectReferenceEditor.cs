namespace Gametamin.Core
{
#if UNITY_EDITOR
    using UnityEditor;
    using UnityEngine;

    namespace Gametamin.Core
    {
        [CustomEditor(typeof(GameObjectReference), true)]
        public class GameObjectReferenceEditor : GameObjectReferenceNameFactoryEditor
        {
            protected override bool _Editable => true;
            protected override bool IsRoot => false;
            protected override bool _ShowBaseInspectorGUI => false;
            SerializedProperty _datasProperty;
            GameObjectReference _instance;
            GameObjectReference _Instance => _instance ??= (GameObjectReference)target;
            void OnEnable()
            {
                _ShowguiAddRefID = false;
                _datasProperty = serializedObject.FindProperty("_datas");
            }
            protected override void OnMiddleGUI()
            {
                target.GUIMonoBehaviourScript();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_copyType"));
                GUIObject();
                EditorGUIHelper.HelpBox("Warning: make sure no dulicate reference ID when copy to top parent!", Color.yellow);
                var space = EditorGUIHelper.GroupVerticalSpacing;
                GUILayout.Space(space);
                LoadTextureHelper.GUILoadAndClearTexture(_Instance.gameObject);
            }
            void GUIObject()
            {
                EditorGUILayout.PropertyField(_datasProperty, true);
                if (Application.isPlaying) return;
                EditorGUIHelper.GUIDragAndDrop(null, value =>
                {
                    if (value is GameObject obj)
                    {
                        var arraySize = _datasProperty.arraySize;
                        InsertArrayElementAtIndex(_datasProperty, arraySize);
                        var objectReference = GetArrayElementAtIndex(_datasProperty, arraySize);
                        objectReference.FindPropertyRelative("_target").objectReferenceValue = value;
                    }
                }, () =>
                {
                    if (serializedObject.hasModifiedProperties)
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                });
                if (serializedObject.hasModifiedProperties)
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
#endif
}
