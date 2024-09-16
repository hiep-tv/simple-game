#if UNITY_EDITOR
using System;
using UnityEditor;

namespace Gametamin.Core
{
    public partial class BaseCustomEditor : Editor
    {
        protected virtual bool _Editable { get; set; }
        protected virtual bool _ShowBaseInspectorGUI => true;
        protected virtual bool _ShowEditableGUI => true;
        protected virtual bool _ShowSearchGUI => true;

        SearchArrayData _searchArrayData;
        SearchArrayData _SearchArrayData => _searchArrayData ??= new(serializedObject);

        protected static readonly string _saveFolderErrorMessage = "Save folder is invalid!";

        protected static readonly string _popupNameLabel = "Popup Name";
        protected static readonly string _addressableLabelLabel = "Addressable Label";
        protected static readonly string _addressableGroupLabel = "Addressable Group";
        protected static readonly string _saveFolderLabel = "Save Folder";
        protected static readonly string _addButtonLabel = "Add";
        protected static readonly string _createButtonLabel = "Create";
        protected static readonly string _clearButtonLabel = "Clear";

        float _labelWidth;
        protected float _LabelWidth
        {
            set
            {
                if (_labelWidth < value)
                {
                    _labelWidth = value;
                }
            }
            get => _labelWidth;
        }
        protected void GUIFitLabel(Action callback)
        {
            GUIFitLabel(_LabelWidth, callback);
        }
        protected void GUIFitLabel(float labelWidth, Action callback)
        {
            var current = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = labelWidth;
            callback?.Invoke();
            EditorGUIUtility.labelWidth = current;
        }
        public override void OnInspectorGUI()
        {
            if (_ShowBaseInspectorGUI)
            {
                if (_ShowEditableGUI)
                {
                    "Editable".GUIToggle(_Editable, value => _Editable = value);
                }
                EditorGUIHelper.DisabledGUI(() => base.OnInspectorGUI(), _Editable);
            }
            OnCustomGUI();
            if (_ShowSearchGUI)
            {
                _SearchArrayData.GUISearchArray();
            }
        }
        protected virtual void OnCustomGUI()
        {

        }
    }
}
#endif