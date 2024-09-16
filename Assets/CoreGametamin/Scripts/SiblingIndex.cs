#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
namespace Gametamin.Core
{
    [ExecuteInEditMode()]
    public class SiblingIndex : MonoBehaviour
    {
        [SerializeField] int _siblingIndex;
        [SerializeField] bool _asFirst, _asLast;
        private void Awake()
        {
            SetSiblingIndex();
        }
        [ContextMenu("Set Sibling Index")]
        public void SetSiblingIndex()
        {
            if (_asFirst)
            {
                transform.SetAsFirstSibling();
                _siblingIndex = transform.GetSiblingIndex();
            }
            else if (_asLast)
            {
                transform.SetAsLastSibling();
                _siblingIndex = transform.GetSiblingIndex();
            }
            else
            {
                transform.SetSiblingIndex(_siblingIndex);
            }
        }
    }
#if UNITY_EDITOR

    [CustomEditor(typeof(SiblingIndex))]
    public class SiblingIndexCustomEditor : Editor
    {
        SerializedProperty _asFirst, _asLast, _siblingIndex;
        SiblingIndex _siblingObject;
        void Awake()
        {
            _asFirst = serializedObject.FindProperty("_asFirst");
            _asLast = serializedObject.FindProperty("_asLast");
            _siblingIndex = serializedObject.FindProperty("_siblingIndex");
            _siblingObject = (SiblingIndex)target;
            _siblingObject.SetSiblingIndex();
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_asFirst);
            if (_asFirst.boolValue)
            {
                _asLast.boolValue = false;
            }
            EditorGUILayout.PropertyField(_asLast);
            if (_asLast.boolValue)
            {
                _asFirst.boolValue = false;
            }
            bool isFirstOrLast = _asFirst.boolValue || _asLast.boolValue;
            if (!isFirstOrLast)
            {
                EditorGUILayout.PropertyField(_siblingIndex);
            }
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                _siblingObject.SetSiblingIndex();
            }
        }
    }
#endif
}
