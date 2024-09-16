using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class UIEditorSettings
    {
        static string _commonPopupDataName = "CommonPopupData", _nonPopupData = "Empty";
        static Object _saveFolder;
        static string _rawInputPopupName, _popupDataName, _popupName, _popupFolderName, _popupPoolIdString, _popupPoolName, _errorMessage;
        static bool _validated;
        static bool _createSubFolder;
        static string _popupPoolNameLabel = "Pool Name";
        static string _popupPoolIdLabel = "Pool ID";
        static PoolReferenceID _popupPoolId;
        static string[] _spriteFolders = new string[0];
        static string _currentOriginPopup;
        static int _indexOriginPopupSelected = 0, _indexCommonPopupData = 0;
        bool _popupOriginClicked, _popupPoolIdClicked;
        static string _addressableGroup, _addressableLabel;
        bool _addressabelLabelClicked, _addressabelGroupClicked;
        static PopupPartCopyType _popupCopyType = PopupPartCopyType.Use;
        static List<string> _groupNames;
        static List<string> _popupDataNames = new();
        static List<SerializedObject> _serializedPopupCopyData;
        static List<SerializedObject> _SerializedPopupCopyData => _serializedPopupCopyData ??= new();
        static List<PopupData> _popupDatas = new();
        void LoadPopupData()
        {
            _groupNames = new();
            _groupNames.SafeAddRange(AddressabelAssetHelper.GetGroupNames());
            _addressableGroup = _groupNames.GetSafe(0);
            var guidSaveFolder = GuidSaveFolder;
            if (!guidSaveFolder.IsNullOrEmptySafe())
            {
                _saveFolder = guidSaveFolder.LoadAssetByGUID<Object>();
            }
            _SerializedPopupCopyData.Clear();
            _popupDataNames.Clear();
            _popupDatas.Clear();
            _popupDataNames.Add(_nonPopupData);
            var instance = CreateInstance<PopupCopyData>();
            var serializedObject = new SerializedObject(instance);
            _SerializedPopupCopyData.Add(new SerializedObject(instance));
            _spriteFolders.FindAssetDatabase<PopupData>(
                result =>
                {
                    _popupDatas.Add(result);
                    if (result.name.EqualsSafe(_commonPopupDataName))
                    {
                        _indexCommonPopupData = _popupDataNames.GetCountSafe();
                        _indexOriginPopupSelected = _indexCommonPopupData;
                        _currentOriginPopup = _commonPopupDataName;
                    }
                    _popupDataNames.Add(result.name);
                    var instance = CreateInstance<PopupCopyData>();
                    instance.SetData(result);
                    var serializedObject = new SerializedObject(instance);
                    _SerializedPopupCopyData.Add(serializedObject);
                });
        }
        void GUICreatePopupData()
        {
            GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
            GUIAddNewPopupData();
            GUIError();
            GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
            GUIOriginPopup();
            GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
            EditorGUIHelper.HorizontalLayout(() =>
            {
                EditorGUIHelper.GUIButton(_addButtonLabel, () =>
                {
                    var namevalid = ValidateFileName();
                    if (namevalid)
                    {
                        CreatePopupData();
                    }
                });

                EditorGUIHelper.GUIButton(_clearButtonLabel, () =>
                {
                    ResetValues();
                });
            });
        }
        void GUIAddNewPopupData()
        {
            GUIFitLabel(() =>
            {
                GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                EditorGUIHelper.HorizontalLayout(() =>
                {
                    _LabelWidth = EditorGUIHelper.DelayedTextField(_popupDataName, _popupNameLabel
                    , result =>
                    {
                        _errorMessage = StringHelper.DefaultValue;
                        _popupDataName = result;
                        ValidateFileName();
                    });
                    EditorGUIHelper.GUIButton(_addButtonLabel, () =>
                    {
                        GUI.FocusControl(string.Empty);
                    });
                });
                GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                _LabelWidth = EditorGUIHelper.GUITextField(_popupPoolName, _popupPoolNameLabel, result =>
                {
                    _errorMessage = StringHelper.DefaultValue;
                    _popupPoolName = result;
                });
                GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                EditorGUIHelper.HorizontalLayout(() =>
                {
                    _LabelWidth = EditorGUIHelper.GUITextField(_popupPoolIdString, _popupPoolIdLabel, result =>
                    {
                        _errorMessage = StringHelper.DefaultValue;
                        _popupPoolIdString = result;
                        _popupPoolId = PoolReferenceID.Non;
                    });
                    _popupPoolIdClicked = EditorGUIHelper.GUIEnumWithSearch(_popupPoolId, _popupPoolIdClicked
                    , (result) =>
                    {
                        _popupPoolId = result;
                        _popupPoolIdString = result.ToString();
                    });
                });
                GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                EditorGUIHelper.HorizontalLayout(() =>
                {
                    _LabelWidth = EditorGUIHelper.GUITextField(_addressableLabel, _addressableLabelLabel, result =>
                    {
                        _errorMessage = StringHelper.DefaultValue;
                        _addressableLabel = result;
                    });
                    _addressabelLabelClicked = EditorGUIHelper.GUIStringWithSearch(_addressableLabelLabel, _addressableLabel, AddressableLabels.PopupValues, _addressabelLabelClicked
                    , (result, index) =>
                    {
                        _addressableLabel = result;
                    });
                });
                GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                EditorGUIHelper.HorizontalLayout(() =>
                {
                    _LabelWidth = EditorGUIHelper.GUITextField(_addressableGroup, _addressableGroupLabel, result =>
                    {
                        _errorMessage = StringHelper.DefaultValue;
                        _addressableGroup = result;
                    });
                    _addressabelGroupClicked = EditorGUIHelper.GUIStringWithSearch(_addressableGroupLabel, _addressableGroup, _groupNames, _addressabelGroupClicked
                    , (result, index) =>
                    {
                        _addressableGroup = result;
                    });
                });
                GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                GUISelectPopupData();
                GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                GUISaveFolder();
            });
        }
        void GUISelectPopupData()
        {
            EditorGUIHelper.HorizontalLayout(() =>
            {
                EditorGUIHelper.GUILabel("Origin Popup", GUILayout.Width(_LabelWidth));
                _popupOriginClicked = EditorGUIHelper.GUIStringWithSearch("Popup Data", _currentOriginPopup, _popupDataNames, _popupOriginClicked
                    , (result, index) =>
                    {
                        _indexOriginPopupSelected = index;
                        _currentOriginPopup = result;
                    });
                if (_indexOriginPopupSelected >= 0)
                {
                    EditorGUIHelper.GUIButton("Open", () =>
                    {
                        _popupDatas.GetSafe(_indexOriginPopupSelected).CreatePopupEditorMode();
                    });
                }
            });
        }

        void CreatePopupData()
        {
            if (!_saveFolder.IsNullSafe())
            {
                var path = _saveFolder.GetAssetPath();
                if (_createSubFolder)
                {
                    path = AssetDatabaseHelper.CreateFolderAndGetPath(path, $"{_popupFolderName}Popup");
                }
                PopupPoolFactory.AddPopup(new PoolConfigData(_popupPoolName, _popupPoolIdString, _addressableLabel, _addressableGroup));
                PoolNameReference.Instance.AddName(_popupPoolIdString);
                AddressableNameFactory.Instance.AddName(_addressableLabel);
                AssetDatabaseHelper.CreateScriptableObject<PopupData>(path, _popupDataName
                    , popupData =>
                    {
                        if (_indexOriginPopupSelected >= 0)
                        {
                            var serializedPopupDataObject = _SerializedPopupCopyData.GetSafe(_indexOriginPopupSelected);
                            serializedPopupDataObject.ApplyModifiedProperties();
                            var data = (PopupCopyData)serializedPopupDataObject.targetObject;
                            popupData.SetPopupData(data, _popupName, path, _popupCopyType);
                            PopupPoolFactory.Instance.AddPopup(popupData, _addressableLabel);
                        }
                        popupData.CreateAssetEntry(_addressableGroup, _addressableLabel);
                        AddressableNameFactory.Instance.Generate();
                        PoolNameReference.Instance.Generate();
                        PopupPoolFactory.Instance.Generate();
                        LoadPopupData();
                    });
            }
            else
            {
                _errorMessage = _saveFolderErrorMessage;
            }
        }
        void GUIOriginPopup()
        {
            if (_indexOriginPopupSelected >= 0)
            {
                var item = _SerializedPopupCopyData.GetSafe(_indexOriginPopupSelected);
                if (!item.IsNullObjectSafe() && !item.targetObject.IsNullObjectSafe())
                {
                    EditorGUIHelper.HorizontalLayout(() =>
                    {
                        GUIFitLabel(() =>
                        {
                            var popupProperty = item.FindProperty("_popup");
                            EditorGUIHelper.GUIObjectField(popupProperty.displayName, popupProperty.objectReferenceValue, typeof(GameObject)
                                , result =>
                                {
                                    popupProperty.objectReferenceValue = result;
                                    if (item.hasModifiedProperties)
                                    {
                                        item.ApplyModifiedProperties();
                                    }
                                }, false);
                        });
                        EditorGUIHelper.GUIEnumPopup(_popupCopyType, result =>
                        {
                            _popupCopyType = (PopupPartCopyType)result;
                        });
                    });
                    EditorGUILayout.PropertyField(item.FindProperty("_popupPartDatas"));
                    EditorGUILayout.PropertyField(item.FindProperty("_pools"));
                }
                else
                {
                    Debug.Log("null item");
                }
            }
        }
        void GUISaveFolder()
        {
            EditorGUIHelper.HorizontalLayout(() =>
            {
                _LabelWidth = EditorGUIHelper.GUIObjectField(_saveFolderLabel, _saveFolder, typeof(Object), result =>
                {
                    _errorMessage = StringHelper.DefaultValue;
                    _saveFolder = result;
                    GuidSaveFolder = _saveFolder.GetGUID();
                });
                EditorGUIHelper.GUIToggle("Create Subfolder", _createSubFolder, value => _createSubFolder = value);
            });
        }
        void GUIError()
        {
            if (!_errorMessage.IsNullOrEmptySafe())
            {
                EditorGUIHelper.ChangeGUILabelColor(() =>
                {
                    EditorGUIHelper.GUILabel(_errorMessage);
                }, Color.yellow);
            }
        }
        bool ValidateFileName()
        {
            if (_popupDataName.IsNullOrEmptySafe())
            {
                return false;
            }
            if (_validated)
            {
                return true;
            }
            var namevalid = IsValidFileName(_popupDataName);
            if (namevalid)
            {
                _validated = true;
                if (_popupFolderName.IsNullOrEmptySafe())
                {
                    _popupFolderName = _popupDataName.ToCamelCase().RemoveSpaceSafe();
                }
                if (_popupPoolIdString.IsNullOrEmptySafe())
                {
                    _popupPoolIdString = _popupDataName.ToCamelCase().RemoveSpaceSafe();
                    _popupPoolName = _popupPoolIdString;
                }
                _popupDataName = _popupDataName.ToLowerSafe().RemoveSpaceSafe();
                _popupName = _popupDataName;
                var builder = StringHelper.GetBuilder(_popupDataName);
                builder.Replace("_popupdata", string.Empty);
                builder.Replace("popupdata", string.Empty);
                builder.Append("_popupdata");
                _popupDataName = builder.GetValueInBuilder();
                if (_addressableLabel.IsNullOrEmptySafe())
                {
                    _addressableLabel = _popupDataName;
                }
            }
            else if (!namevalid)
            {
                _errorMessage = "Popup name is invalid!";
            }
            Repaint();
            return namevalid;
        }
        bool IsValidFileName(string fileName)
        {
            var invalidCharacters = Path.GetInvalidFileNameChars();
            return !fileName.IsNullOrEmptySafe() && fileName.IndexOfAny(invalidCharacters) < 0;
        }
        void ResetValues()
        {
            _validated = false;
            _popupDataName = string.Empty;
            _popupPoolName = string.Empty;
            _popupPoolIdString = string.Empty;
            _popupFolderName = string.Empty;
            _addressableLabel = string.Empty;
            _addressableGroup = _groupNames.GetSafe(0);
            _indexOriginPopupSelected = _indexCommonPopupData;
            _currentOriginPopup = _popupDataNames.GetSafe(_indexOriginPopupSelected);
        }
    }
}
