using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class UIEditorSettings
    {
        Editor _currentPopupData;
        int _indexPopupDataSelected = 0;
        string _searchPopupData = string.Empty;
        bool _searchingPopupData;
        void GUIPopupDatas()
        {
            GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
            EditorGUIHelper.GUISearchBar(_searchPopupData, result =>
            {
                _searchPopupData = result;
                _searchingPopupData = !_searchPopupData.IsNullOrEmptySafe();
            });
            GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
            _popupDatas.For((item, index) =>
            {

                if (_searchingPopupData)
                {
                    if (item.name.ContainsSafe(_searchPopupData, System.StringComparison.OrdinalIgnoreCase))
                    {
                        GUIPopupData(item, index);
                        GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                    }
                }
                else
                {
                    GUIPopupData(item, index);
                    GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                }
            });
            if (_currentPopupData == null)
            {
                var firstItem = _popupDatas.GetSafe(0);
                if (firstItem != null)
                {
                    _currentPopupData = Editor.CreateEditor(firstItem);
                }
            }
            InspectorGUIPopupData();
        }
        void GUIPopupData(PopupData item, int index)
        {
            EditorGUIHelper.HorizontalLayout(() =>
            {
                EditorGUILayout.ObjectField(item, typeof(PopupData), false);
                GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                EditorGUIHelper.DisabledGUI(() =>
                {
                    EditorGUIHelper.GUIButton("View", () =>
                    {
                        _currentPopupData = Editor.CreateEditor(item);
                        _indexPopupDataSelected = index;
                    });
                }, _indexPopupDataSelected != index);
                EditorGUIHelper.GUIButton("Create Popup", () =>
                {
                    _currentPopupData = Editor.CreateEditor(item);
                    _indexPopupDataSelected = index;
                    item.CreatePopupEditorMode();
                });
            });
        }
        void InspectorGUIPopupData()
        {
            if (_currentPopupData != null)
            {
                GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
                _currentPopupData.OnInspectorGUI();
                if (_currentPopupData.serializedObject.hasModifiedProperties)
                {
                    _currentPopupData.serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
