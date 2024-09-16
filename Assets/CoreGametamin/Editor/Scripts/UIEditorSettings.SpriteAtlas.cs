using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class UIEditorSettings
    {
        Editor _currentAtlas;
        int _indexAtlasSelected = 0;
        string _searchAtlas = string.Empty;
        bool _searchingAtlas;
        List<AtlasConfig> _Atlases = new();
        void LoadAtlasConfig()
        {
            _Atlases.SafeClear();
            _spriteFolders.FindAssetDatabase<AtlasConfig>(
                result =>
                {
                    _Atlases.AddSafe(result);
                });
        }
        void GUIAtlases()
        {
            GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
            EditorGUIHelper.GUISearchBar(_searchAtlas, result =>
            {
                _searchAtlas = result;
                _searchingAtlas = !_searchAtlas.IsNullOrEmptySafe();
            });
            GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
            _Atlases.For((item, index) =>
            {
                if (_searchingAtlas)
                {
                    if (item.name.ContainsSafe(_searchAtlas, System.StringComparison.OrdinalIgnoreCase))
                    {
                        GUIAtlas(item, index);
                        GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                    }
                }
                else
                {
                    GUIAtlas(item, index);
                    GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                }
            });
            if (_currentAtlas == null)
            {
                var firstItem = _Atlases.GetSafe(0);
                if (firstItem != null)
                {
                    _currentAtlas = Editor.CreateEditor(firstItem);
                }
            }
            InspectorGUIAtlas();
        }
        void GUIAtlas(AtlasConfig item, int index)
        {
            EditorGUIHelper.HorizontalLayout(() =>
            {
                EditorGUILayout.ObjectField(item, typeof(AtlasConfig), false);
                GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                EditorGUIHelper.DisabledGUI(() =>
                {
                    EditorGUIHelper.GUIButton("View", () =>
                    {
                        _currentAtlas = Editor.CreateEditor(item);
                        _indexAtlasSelected = index;
                    });
                }, _indexAtlasSelected != index);
            });
        }
        void InspectorGUIAtlas()
        {
            if (_currentAtlas != null)
            {
                GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
                _currentAtlas.OnInspectorGUI();
                if (_currentAtlas.serializedObject.hasModifiedProperties)
                {
                    _currentAtlas.serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
