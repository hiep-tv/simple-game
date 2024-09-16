using System;
using UnityEditor;

namespace Gametamin.Core
{
    public class BaseEditorWindow : EditorWindow
    {
        protected virtual bool _ShowSearchGUI { get; } = true;
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
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = _LabelWidth;
            callback?.Invoke();
            EditorGUIUtility.labelWidth = labelWidth;
        }
        public void OnGUI()
        {
            OnCustomGUI();
        }
        protected virtual void OnCustomGUI()
        {

        }
    }
}
