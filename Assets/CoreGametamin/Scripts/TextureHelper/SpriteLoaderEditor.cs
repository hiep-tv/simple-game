#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace Gametamin.Core
{
    [CustomEditor(typeof(SpriteLoader), true)]
    public class TextureLoaderEditor : AtlasConfigEditor
    {
        string _currentAtlasName;
        SpriteLoader _instance;
        SpriteLoader _Instance => _instance ??= (SpriteLoader)target;
        SerializedObject _configSerializedObject;
        protected override AtlasConfig _AtlasConfig
        {
            get
            {
                if (_atlasConfig == null)
                {
                    _atlasConfig = _Instance.AtlasName.GetAtlasConfigInAssetDatabase();
                    if (_atlasConfig != null)
                    {
                        _configSerializedObject = new SerializedObject(_atlasConfig);
                    }
                }
                return _atlasConfig;
            }
        }
        protected override string _AtlasNameProperty => _atlasNameProperty;
        static string _atlasNameProperty = "_atlasName";
        bool _disabledByOther = true;
        private void Awake()
        {
            _disabledByOther = serializedObject.FindProperty("_loadDirect").boolValue;
            _ShowguiAddRefID = false;
            var atlasName = serializedObject.FindProperty(_AtlasNameProperty);
            _currentAtlasName = atlasName.stringValue;
        }
        protected override void OnCustomGUI()
        {
            if (_disabledByOther)
            {
                "Load by other".HelpBox(Color.yellow);
                return;
            }
            if (_currentAtlasName.IsNullOrEmptySafe()) return;
            GUICurrentAtlas();
            base.OnCustomGUI();
            var atlasName = serializedObject.FindProperty(_AtlasNameProperty);
            if (!_currentAtlasName.EqualsSafe(atlasName.stringValue))
            {
                _currentAtlasName = atlasName.stringValue;
                _atlas = _currentAtlasName.GetSpriteAtlasInAssetDatabase();
                _refresh = true;
            }
        }
        protected override void GUITextures(List<Sprite> sprites, List<bool> clickeds, int startIndex, int endIndex, Action<Sprite> onSelect = null)
        {
            base.GUITextures(sprites, clickeds, startIndex, endIndex, selected =>
            {
                _Instance.SetSprite(selected);
            });
        }
        protected override void GUIAddPackableToAtlas(SerializedObject serialized, bool canEditFolder)
        {
            base.GUIAddPackableToAtlas(_configSerializedObject, false);
        }
        void GUICurrentAtlas()
        {
            if (!_configSerializedObject.IsNullObjectSafe())
            {
                EditorGUIHelper.DisabledGUI(() =>
                {
                    var atlas = _configSerializedObject.FindProperty("_atlas");
                    EditorGUIHelper.HorizontalLayout(() =>
                    {
                        EditorGUILayout.PropertyField(atlas);
                        EditorGUILayout.ObjectField(_configSerializedObject.targetObject, typeof(AtlasConfig), false);
                    });
                });
            }
        }
    }
}
#endif