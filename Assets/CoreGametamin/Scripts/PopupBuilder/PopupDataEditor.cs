#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
namespace Gametamin.Core
{
    [CustomEditor(typeof(PopupData), true)]
    public partial class PopupDataEditor : GameObjectReferenceNameFactoryEditor
    {
        float _addlabelWidth;
        float _AddLabelWidth
        {
            get => _addlabelWidth;
            set
            {
                if (value > _addlabelWidth)
                {
                    _addlabelWidth = value;
                }
            }
        }
        protected override bool _Editable => true;
        protected override bool IsRoot => false;
        PopupData _popupData;
        PopupData _PopupData
        {
            get
            {
                if (_popupData.IsNullSafe())
                {
                    _popupData = (PopupData)target;
                }
                return _popupData;
            }
        }
        protected override void OnMiddleGUI()
        {
            GUIPopupPart();
            GUIPopupPool();
            GUIDragAndDrop();
            if (HasPendingPart || HasPendingPool)
            {
                return;
            }
            EditorGUIHelper.GUIButton("Create Popup", () =>
            {
                var popupData = (PopupData)target;
                popupData.CreatePopupEditorMode();
            });
        }
        void GUIDragAndDrop()
        {
            GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);

            var hasPendingPart = HasPendingPart;
            var hasPendingPool = HasPendingPool;
            if (!hasPendingPool)
            {
                EditorGUIHelper.HorizontalLayout(() =>
                {
                    GUIFitLabel(_AddLabelWidth, () =>
                    {
                        _AddLabelWidth = GUIAddAndEditPart();
                    });
                }, true);
                GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
            }
            if (!hasPendingPart)
            {
                EditorGUIHelper.HorizontalLayout(() =>
                {
                    GUIFitLabel(_AddLabelWidth, () =>
                    {
                        _AddLabelWidth = GUIAddAndEditPool();
                    });
                }, true);
            }
            if (HasPendingPart)
            {
                GUIAddNewPart(_AddPartDataSerializedObject);
            }
            if (HasPendingPool)
            {
                GUIAddNewPool(_AddPoolDataSerializedObject);
            }
            GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
        }
        void GUIAddPartButtons(Action onAdd, Action onClear)
        {
            EditorGUIHelper.HorizontalLayout(() =>
            {
                var height = GUILayout.Height(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2);
                EditorGUIHelper.GUIButton(_addButtonLabel, onAdd, height);
                EditorGUIHelper.GUIButton(_clearButtonLabel, onClear, height);
            });
        }
    }
}
#endif