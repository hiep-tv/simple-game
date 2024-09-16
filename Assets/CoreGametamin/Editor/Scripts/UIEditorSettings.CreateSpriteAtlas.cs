using System.IO;
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class UIEditorSettings
    {
        protected static string _atlasNameErrorMessage = "Atlas name is invalid!";
        static string _atlasSuffix = "atlas";
        static string _atlasName;
        static bool _maskAsAddressable;
        Object _saveAtlasFolder, _textureFolder;
        string GuidSaveAtlasFolder
        {
            get => EditorPrefs.GetString("_save_folder_atlas_");
            set => EditorPrefs.SetString("_save_folder_atlas_", value);
        }
        string GuidTextureFolder
        {
            get => EditorPrefs.GetString("_texture_folder_atlas_");
            set => EditorPrefs.SetString("_texture_folder_atlas_", value);
        }
        private void LoadCreateAtlasData()
        {
            var saveFolder = GuidSaveAtlasFolder;
            if (!saveFolder.IsNullOrEmptySafe())
            {
                _saveAtlasFolder = saveFolder.LoadAssetByGUID<Object>();
            }
            var guidTextureFolder = GuidTextureFolder;
            if (!guidTextureFolder.IsNullOrEmptySafe())
            {
                _textureFolder = guidTextureFolder.LoadAssetByGUID<Object>();
            }
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
                            ValidateAtlasFileName();
                        });
                        EditorGUIHelper.GUIButton(_addButtonLabel, () =>
                        {
                            GUI.FocusControl(string.Empty);
                        });
                    });
                    EditorGUIHelper.GUIToggle("Mask as Addressable", _maskAsAddressable, (value) =>
                    {
                        _maskAsAddressable = value;
                    });
                    if (_maskAsAddressable)
                    {
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
                            _addressabelGroupClicked = EditorGUIHelper.GUIStringWithSearch(_addressableGroupLabel, _addressableGroup, _groupNames, _addressabelGroupClicked
                            , (result, index) =>
                            {
                                _addressableGroup = result;
                            });
                        });
                    }
                    GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                    _LabelWidth = EditorGUIHelper.GUIObjectField("Texture Folder", _textureFolder, typeof(Object)
                        , result =>
                        {
                            _textureFolder = result;
                            GuidTextureFolder = _textureFolder.GetGUID();
                        });
                    GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                    GUISaveAtlasFolder();
                });
                GUILayout.Space(EditorGUIHelper.StandardVerticalSpacing);
                EditorGUIHelper.HorizontalLayout(() =>
                {
                    EditorGUIHelper.GUIButton(_createButtonLabel, () =>
                    {
                        var namevalid = IsValidAtlasFileName(_atlasName);
                        if (namevalid && !_saveAtlasFolder.IsNullSafe())
                        {
                            AddressableNameFactory.Instance.AddName(_addressableLabel);
                            var path = _saveAtlasFolder.GetAssetPath();
                            AtlasHelper.CreateNewAtlas(path, _atlasName, (config) =>
                            {
                                config.SetTextureFolder(_textureFolder);
                                config.AtlasName = _atlasName;
                                if (_maskAsAddressable)
                                {
                                    config.CreateAssetEntry(_addressableGroup, _addressableLabel);
                                    config.Atlas.CreateAssetEntry(_addressableGroup, _addressableLabel);
                                    AddressableNameFactory.Instance.Generate();
                                    SpriteAtlasFactory.Instance.AddAtlasConfig(config, _addressableLabel, _addressableGroup);
                                }
                                else
                                {
                                    SpriteAtlasFactory.Instance.AddAtlasConfig(config, string.Empty, string.Empty);
                                }
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
                        ResetAtlasValues();
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
        void GUISaveAtlasFolder()
        {
            EditorGUIHelper.HorizontalLayout(() =>
            {
                _LabelWidth = EditorGUIHelper.GUIObjectField(_saveFolderLabel, _saveAtlasFolder, typeof(Object), result =>
                {
                    _errorMessage = StringHelper.DefaultValue;
                    _saveAtlasFolder = result;
                    GuidSaveAtlasFolder = _saveAtlasFolder.GetGUID();
                });
                EditorGUIHelper.GUIToggle("Create Subfolder", _createSubFolder, value => _createSubFolder = value);
            });
        }
        void ValidateAtlasFileName()
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
        bool IsValidAtlasFileName(string fileName)
        {
            var invalidCharacters = Path.GetInvalidFileNameChars();
            return !fileName.IsNullOrEmptySafe() && fileName.IndexOfAny(invalidCharacters) < 0;
        }
        void ResetAtlasValues()
        {
            _atlasName = string.Empty;
            _addressableLabel = string.Empty;
        }
    }
}
