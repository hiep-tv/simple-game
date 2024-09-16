#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace Gametamin.Core
{
    [CustomEditor(typeof(AtlasConfig), true)]
    public class AtlasConfigEditor : TextureNameReferenceFactoryEditor
    {
        protected override bool _ShowSearchGUI => false;
        protected override bool _ShowEditableGUI => false;
        protected override bool _Editable => true;
        protected override bool IsRoot => false;
        protected AtlasConfig _atlasConfig;
        protected virtual AtlasConfig _AtlasConfig => _atlasConfig ??= (AtlasConfig)target;
        List<Sprite> _sprites = new();
        List<bool> _clickeds = new();
        static string _atlasNameProperty = "_atlasAddressableLabel";
        protected virtual string _AtlasNameProperty => _atlasNameProperty;
        string _copyAtlas = string.Empty;
        static int _column = 4;
        protected bool _refresh = true, _clickedCopy;
        protected virtual bool _Expand { get; set; } = true;
        int _removeIndex = -1;
        protected SpriteAtlas _atlas;
        SpriteAtlas _Atlas
        {
            get
            {
                _atlas = _AtlasConfig.Atlas;
                if (_atlas == null)
                {
                    var atlasName = serializedObject.FindProperty(_AtlasNameProperty);
                    _atlas = AtlasEditorSettings.GetSpriteAtlas(atlasName.stringValue);
                }
                return _atlas;
            }
        }
        protected override void OnCustomGUI()
        {
            GUIAtlas();
            GUICopyAtlas();
            base.OnCustomGUI();
        }
        void GUICopyAtlas()
        {
            EditorGUIHelper.HorizontalLayout(() =>
            {
                _clickedCopy = EditorGUIHelper.GUIStringWithSearch("Atlas Label", _copyAtlas, AddressableLabels.Values, _clickedCopy
                    , (value, index) => _copyAtlas = value, 80f);
                EditorGUIHelper.GUIButton("Copy", () =>
                {
                    var packables = _copyAtlas.GetPackables();
                    _Atlas.AddTexturesToAtlas(packables, () => _refresh = true);
                });
                GUILayout.FlexibleSpace();
            });
        }
        void GUIAtlas()
        {
            if (_AtlasConfig == null) return;
            GUIAddPackableToAtlas(serializedObject);
            GUIButtons();
            EditorGUIHelper.GUIFoldout("Config sprite ID", _Expand, value => _Expand = value);
            if (!_Expand)
            {
                return;
            }
            if (_refresh)
            {
                _sprites.Clear();
                _clickeds.Clear();
                _Atlas.GetTextures(_sprites);
                for (int i = 0, length = _sprites.GetCountSafe(); i < length; i++)
                {
                    _clickeds.Add(false);
                }
                _refresh = false;
            }
            GUITextures(_sprites);
        }
        void GUITextures(List<Sprite> sprites)
        {
            var count = sprites.GetCountSafe();
            var itemPerColumn = count / _column;
            var remaining = count % _column;
            var startIndex = 0;
            var endIndex = itemPerColumn;
            EditorGUIHelper.HorizontalLayout(() =>
            {
                for (int i = 0; i < _column; i++)
                {
                    var end = endIndex;
                    if (remaining > 0)
                    {
                        remaining -= 1;
                        end += 1;
                    }
                    if (end <= count)
                    {
                        GUITextures(sprites, _clickeds, startIndex, end);
                        startIndex = end;
                        endIndex = Mathf.Min(startIndex + itemPerColumn, count);
                    }
                }
            });
            if (_removeIndex >= 0)
            {
                var sprite = sprites[_removeIndex].LoadAssetAtPath<UnityEngine.Object>();
                _Atlas.RemoveTexturesFromAtlas(sprite);
                _refresh = true;
                _removeIndex = -1;
            }
        }
        static string _NULL = "Null";
        protected virtual void GUITextures(List<Sprite> sprites, List<bool> clickeds, int startIndex, int endIndex, Action<Sprite> onSelect = null)
        {
            EditorGUIHelper.VerticleLayout(() =>
            {
                for (int i = startIndex; i < endIndex; i++)
                {
                    var sprite = sprites[i];
                    var spriteName = _NULL;
                    if (sprite != null)
                    {
                        spriteName = sprite.name;
                    }
                    EditorGUIHelper.VerticleLayout(() =>
                    {
                        EditorGUIHelper.HorizontalLayout(() =>
                        {
                            EditorGUIHelper.GUIButton(sprite != null ? sprite.texture : default
                                , () =>
                                 {
                                     onSelect?.Invoke(sprites[i]);
                                 }, GUILayout.Width(80f), GUILayout.Height(80f));
                            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
                            EditorGUIHelper.GUIButton("x",
                                () =>
                                {
                                    _removeIndex = i;
                                }, GUILayout.Width(EditorGUIUtility.singleLineHeight), GUILayout.Height(EditorGUIUtility.singleLineHeight));
                        });
                        GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
                        EditorGUIHelper.DisabledGUI(() => EditorGUILayout.ObjectField(sprites[i], typeof(Sprite), false));
                        var id = _AtlasConfig.GetTextureID(spriteName);
                        GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
                        clickeds[i] = EditorGUIHelper.GUIStringWithSearch("TextureName ReferenceID", id, TextureNameReferenceID.Values, clickeds[i]
                            , (result, index) =>
                             {
                                 _AtlasConfig.AddTextureID(result, spriteName);
                             }, 80f);
                        GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
                    }, true);
                }
            });
        }
        protected virtual void GUIAddPackableToAtlas(SerializedObject serialized, bool canEditFolder = true)
        {
            GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
            var property = serialized.FindProperty("_textureFolder");
            EditorGUIHelper.HorizontalLayout(() =>
            {
                EditorGUIHelper.ChangeGUILabelColor(() =>
                {
                    EditorGUIHelper.HelpBox("Drag and drop packables here!");
                    EditorGUIHelper.GUIDragAndDrops(values =>
                    {
                        _Atlas.AddTexturesToAtlas(values, () =>
                        {
                            ForcePackAtlas();
                            _refresh = true;
                        });
                    });
                }, Color.yellow);
                GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
                if (property != null)
                {
                    GUIFitLabel(() =>
                    {
                        EditorGUIHelper.DisabledGUI(() =>
                        {
                            _LabelWidth = EditorGUIHelper.GUIObjectField(property.displayName, property.objectReferenceValue, typeof(UnityEngine.Object), result =>
                            {
                                property.objectReferenceValue = result;
                                serialized.ApplyModifiedProperties();
                            });
                        }, canEditFolder);
                    });
                    GUIPackFolder(property.objectReferenceValue.GetAssetPath());
                    GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
                }
            });
        }
        void GUIButtons()
        {
            EditorGUIHelper.HorizontalLayout(() =>
            {
                EditorGUIHelper.GUIButton("Clean Atlas", () =>
                {
                    _Atlas.CleanAtlas(() =>
                    {
                        ForcePackAtlas();
                        _refresh = true;
                    });
                });
                EditorGUIHelper.GUIButton("Clear Atlas", () =>
                {
                    _Atlas.ClearAtlas(() =>
                    {
                        ForcePackAtlas();
                        _refresh = true;
                    });
                });
            }, true);
        }
        void GUIPackFolder(string path)
        {
            EditorGUIHelper.GUIButton("Pack", () =>
            {
                _Atlas.AddTexturesToAtlas(path, () =>
                {
                    ForcePackAtlas();
                    _refresh = true;
                });
            });
        }
        void ForcePackAtlas()
        {
            SpriteAtlas[] arr = new SpriteAtlas[1];
            arr[0] = _Atlas;
            SpriteAtlasUtility.PackAtlases(arr, EditorUserBuildSettings.activeBuildTarget);
        }
    }
}
#endif