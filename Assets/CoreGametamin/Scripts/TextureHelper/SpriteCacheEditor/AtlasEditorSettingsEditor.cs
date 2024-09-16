#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gametamin.Core
{
    [CustomEditor(typeof(AtlasEditorSettings), true)]
    public partial class AtlasEditorSettingsEditor : BaseCustomEditor
    {
        protected static string _atlasNameErrorMessage = "Atlas name is invalid!";
        static bool _createSubFolder;
        AtlasEditorSettings _uieditorSettings;
        static string _atlasSuffix = "atlas";
        static List<string> _groupNames;
        static List<string> _GroupNames
        {
            get
            {
                if (_groupNames.GetCountSafe() <= 0)
                {
                    _groupNames = new();
                    _groupNames.SafeAddRange(AddressabelAssetHelper.GetGroupNames());
                    _addressableGroup = _groupNames.GetSafe(0);
                }
                return _groupNames;
            }
        }
        static string _addressableGroup, _addressableLabel;
        bool _addressabelLabelClicked, _addressabelGroupClicked;
        static string _atlasName, _errorMessage;
        Object _saveFolder, _textureFolder;
        string GuidSaveFolder
        {
            get => EditorPrefs.GetString("_save_folder_atlas_");
            set => EditorPrefs.SetString("_save_folder_atlas_", value);
        }
        string GuidTextureFolder
        {
            get => EditorPrefs.GetString("_texture_folder_atlas_");
            set => EditorPrefs.SetString("_texture_folder_atlas_", value);
        }
        private void Awake()
        {
            var guidSaveFolder = GuidSaveFolder;
            if (!guidSaveFolder.IsNullOrEmptySafe())
            {
                _saveFolder = guidSaveFolder.LoadAssetByGUID<Object>();
            }
            var guidTextureFolder = GuidTextureFolder;
            if (!guidTextureFolder.IsNullOrEmptySafe())
            {
                _textureFolder = guidTextureFolder.LoadAssetByGUID<Object>();
            }
        }
        protected override void OnCustomGUI()
        {
            GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
            GUICreateAtlas();
        }
        void GUICreateAtlas()
        {
            EditorGUIHelper.VerticleLayout(() =>
            {
                GUIFitLabel(() =>
                {
                    EditorGUIHelper.HorizontalLayout(() =>
                    {
                        _LabelWidth = EditorGUIHelper.DelayedTextField(_atlasName, "New Atlas"
                        , result =>
                        {
                            _errorMessage = StringHelper.DefaultValue;
                            _atlasName = result;
                            ValidateFileName();
                        });
                        EditorGUIHelper.GUIButton(_addButtonLabel, () =>
                        {
                            GUI.FocusControl(string.Empty);
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
                        _addressabelLabelClicked = EditorGUIHelper.GUIStringWithSearch(_addressableLabelLabel, _addressableLabel, AddressableLabels.AtlasValues, _addressabelLabelClicked
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
                        _addressabelGroupClicked = EditorGUIHelper.GUIStringWithSearch(_addressableGroupLabel, _addressableGroup, _GroupNames, _addressabelGroupClicked
                        , (result, index) =>
                        {
                            _addressableGroup = result;
                        });
                    });
                    GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                    _LabelWidth = EditorGUIHelper.GUIObjectField("Texture Folder", _textureFolder, typeof(Object)
                        , result =>
                        {
                            _textureFolder = result;
                            GuidTextureFolder = _textureFolder.GetGUID();
                        });
                    GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                    GUISaveFolder();
                });
                GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                EditorGUIHelper.HorizontalLayout(() =>
                {
                    EditorGUIHelper.GUIButton(_createButtonLabel, () =>
                    {
                        var namevalid = IsValidFileName(_atlasName);
                        if (namevalid && !_saveFolder.IsNullSafe())
                        {
                            var path = _saveFolder.GetAssetPath();
                            AtlasHelper.CreateNewAtlas(path, _atlasName, (config) =>
                            {
                                config.SetTextureFolder(_textureFolder);
                                config.AtlasName = _atlasName;
                                AtlasEditorSettings.Reload();
                                config.CreateAssetEntry(_addressableGroup, _addressableLabel);
                                config.Atlas.CreateAssetEntry(_addressableGroup, _addressableLabel);
                                AddressableNameFactory.Instance.AddName(_addressableLabel);
                            }, _textureFolder.GetAssetPath());
                        }
                        else if (!namevalid)
                        {
                            _errorMessage = _atlasNameErrorMessage;
                        }
                        else
                        {
                            _errorMessage = _saveFolderErrorMessage;
                        }
                    });
                    EditorGUIHelper.GUIButton(_clearButtonLabel, () =>
                    {
                        ResetValues();
                    });
                });
            }, true);
            if (!_errorMessage.IsNullOrEmptySafe())
            {
                EditorGUIHelper.ChangeGUILabelColor(() =>
                {
                    EditorGUIHelper.GUILabel(_errorMessage);
                }, Color.yellow);
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
        void ValidateFileName()
        {
            _atlasName = _atlasName.ToLowerSafe().RemoveSpaceSafe();
            if (_atlasName.IsNullOrEmptySafe()) return;

            if (!_atlasName.EqualsSafe(_atlasSuffix))
            {
                if (!_atlasName.ContainsSafe("atlas"))
                {
                    _atlasName += "_atlas";
                }
                else
                {
                    _atlasName = _atlasName.Replace("atlas", "_atlas");
                }
                if (_addressableLabel.IsNullOrEmptySafe())
                {
                    _addressableLabel = _atlasName;
                }
            }
            else
            {
                _atlasName = string.Empty;
                _errorMessage = _atlasNameErrorMessage;
            }
            Repaint();
        }
        bool IsValidFileName(string fileName)
        {
            var invalidCharacters = Path.GetInvalidFileNameChars();
            return !fileName.IsNullOrEmptySafe() && fileName.IndexOfAny(invalidCharacters) < 0;
        }
        void ResetValues()
        {
            _atlasName = string.Empty;
            _addressableLabel = string.Empty;
            _addressableGroup = _GroupNames.GetSafe(0);
        }
    }
}
#endif
