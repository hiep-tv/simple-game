
#if UNITY_EDITOR
using System;
using System.Buffers;
using UnityEditor;
using UnityEditorInternal.VR;
using UnityEngine;
namespace Gametamin.Core
{
    public class SearchWindow2 : EditorWindow
    {
        static SerializedProperty _SerializedProperty;
        public static void Create(SerializedProperty serializedProperty)
        {
            _SerializedProperty = serializedProperty;
            var window = GetWindow(typeof(SearchWindow2));
            window.titleContent = new GUIContent(serializedProperty.name);
        }
        Vector2 _scrollPosition;
        string _search;
        int _searchResult;
        public void OnGUI()
        {
            if (_SerializedProperty != null)
            {
                EditorGUIHelper.HorizontalLayout(() =>
                {
                    var startSize = _SerializedProperty.arraySize;
                    EditorGUIHelper.GUIChangeCheck(() =>
                    {
                        EditorGUILayout.PropertyField(_SerializedProperty.FindPropertyRelative("Array.size"));
                    }, () =>
                    {
                        if (startSize != _SerializedProperty.arraySize)
                        {
                            _SerializedProperty.serializedObject.ApplyModifiedProperties();
                        }
                    });
                    EditorGUIHelper.GUISearchBar(_search, result =>
                    {
                        _search = result;
                    });
                    if (!_search.IsNullOrEmptySafe())
                    {
                        EditorGUIHelper.GUILabel($"Result ({_searchResult})");
                    }
                });
                var removedIndex = -1;
                var duplicateIndex = -1;
                var searchResult = 0;
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
                var size = _SerializedProperty.arraySize;
                for (int i = 0; i < size; i++)
                {
                    var element = _SerializedProperty.GetArrayElementAtIndex(i);
                    if (_search.IsNullOrEmptySafe())
                    {
                        //EditorGUILayout.PropertyField(element);
                        GUIItem(element, i, index =>
                        {
                            duplicateIndex = index;
                        }, index =>
                        {
                            removedIndex = index;
                        });
                    }
                    else
                    {
                        if (Contains(element, _search))
                        {
                            searchResult++;
                            //EditorGUILayout.PropertyField(element);
                            GUIItem(element, i, index =>
                            {
                                duplicateIndex = index;
                            }, index =>
                            {
                                removedIndex = index;
                            });
                        }
                    }
                }
                GUILayout.EndScrollView();
                _searchResult = searchResult;
                if (removedIndex >= 0)
                {
                    _SerializedProperty.DeleteArrayElementAtIndex(removedIndex);
                    _SerializedProperty.serializedObject.ApplyModifiedProperties();
                    removedIndex = -1;
                }
                if (duplicateIndex >= 0)
                {
                    var property = _SerializedProperty.GetArrayElementAtIndex(duplicateIndex);
                    property.DuplicateCommand();
                    _SerializedProperty.serializedObject.ApplyModifiedProperties();
                    removedIndex = -1;
                }
                if (GUI.changed)
                {
                    _SerializedProperty.serializedObject.ApplyModifiedProperties();
                }
            }
            else
            {
                Close();
            }
        }
        void GUIItem(SerializedProperty element, int index, Action<int> onDuplicate, Action<int> onRemove)
        {
            EditorGUIHelper.HorizontalLayout(() =>
            {
                EditorGUILayout.PropertyField(element);
                EditorGUIHelper.VerticleLayout(() =>
                {
                    GUILayout.Space(10);
                    EditorGUIHelper.GUIButton("+", () =>
                    {
                        onDuplicate?.Invoke(index);
                    }, GUILayout.Width(20));
                    EditorGUIHelper.GUIButton("-", () =>
                    {
                        onRemove?.Invoke(index);
                    }, GUILayout.Width(20));
                }, false, GUILayout.Width(20));
            });
        }
        bool Contains(SerializedProperty property, string search)
        {
            property = property.Copy();
            if (!property.hasVisibleChildren)
            {
                var stringValue = property.GetPropertyValueString();
                if (stringValue.Contains(search, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                return false;
            }
            var nextElement = property.Copy();
            if (!nextElement.NextVisible(false))
            {
                nextElement = null;
            }
            if (property.NextVisible(true))
            {
                var stringValue = property.GetPropertyValueString();
                if (stringValue.Contains(search, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            while (property.NextVisible(true))
            {
                if (SerializedProperty.EqualContents(property, nextElement))
                {
                    return false;
                }
                var stringValue = property.GetPropertyValueString();
                if (stringValue.Contains(search, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
#endif