#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public abstract partial class ReferenceNameFactoryEditor<T, U, V>
    {
        List<string> _factoryNames = new();
        List<string> _FactoryNames
        {
            get
            {
                if (_factoryNames.GetCountSafe() <= 0)
                {
                    LoadFactories();
                }
                return _factoryNames;
            }
        }
        float _maxWidthFactoryName;
        float _MaxWidthFactoryName
        {
            get
            {
                if (_maxWidthFactoryName <= 0)
                {
                    _FactoryNames.For(item =>
                    {
                        var width = item.GetLabelWidth();
                        if (_maxWidthFactoryName < width)
                        {
                            _maxWidthFactoryName = width;
                        }
                    });
                }
                return _maxWidthFactoryName;
            }
        }
        int _factorySelectedIndex
        {
            get => InspectedObject.FactorySelectedIndex;
            set => InspectedObject.FactorySelectedIndex = value;
        }
        bool _factoryNameClicked;
        string _FactorySuffixName
        {
            get => InspectedObject.FactorySuffixName;
            set => InspectedObject.FactorySuffixName = value;
        }
        string _factoryName;
        static string _factoryNameLabel = "Factory Suffix";
        //List<ReferenceNameDataFactory> _Factories => InspectedObject.Factories;
        void LoadFactories()
        {
            _factoryNames.Clear();
            _factorySelectedIndex = 0;
            var suffix = _FactorySuffixName;
            var hasSuffix = !suffix.IsNullOrEmptySafe();
            var path = InspectedObject.GetAssetPath();
            path = path[..(path.LastIndexOf("/"))];
            var subFactoryExist = false;
            InspectedObject.LoadFactories((resut) =>
                {
                    var factoryName = resut.name;
                    _factoryNames.Add(factoryName);
                    var splits = factoryName.Split(".");
                    if (factoryName.EqualsSafe(InspectedObject.DefaultFactory))
                    {
                        if (!hasSuffix)
                        {
                            _factoryName = factoryName;
                            _factorySelectedIndex = _factoryNames.GetCountSafe() - 1;
                        }
                    }
                    else
                    {
                        if (hasSuffix)
                        {
                            if (splits.GetSafe(1).EqualsSafe(suffix))
                            {
                                subFactoryExist = true;
                                _factoryName = factoryName;
                                _factorySelectedIndex = _factoryNames.GetCountSafe() - 1;
                            }
                        }
                    }
                });
            if (!subFactoryExist && hasSuffix)
            {
                var subFacetoryName = $"{InspectedObject.DefaultFactory}.{suffix}";
                _factoryNames.Add(subFacetoryName);
                _factorySelectedIndex = _factoryNames.GetCountSafe() - 1;
                _factoryName = subFacetoryName;
            }
        }
        void GUIFactories()
        {
            GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
            EditorGUIHelper.HorizontalLayout(() =>
            {
                _LabelWidth = EditorGUIHelper.GUITextField(_FactorySuffixName, _factoryNameLabel, result =>
                 {
                     _FactorySuffixName = result.ToCamelCase();
                     AddNewSubFactroryName(_FactorySuffixName);

                 });
                _factoryNameClicked = EditorGUIHelper.GUIStringWithSearch("Factory", _factoryName, _FactoryNames, _factoryNameClicked
                    , (result, index) =>
                    {
                        _factoryName = result;
                        _factorySelectedIndex = index;
                    }, Mathf.Min(_MaxWidthFactoryName, 200));
                EditorGUIHelper.GUIButton("Load Factories", () =>
                {
                    LoadFactories();
                });
            });
            GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
        }
        void AddNewSubFactroryName(string suffix)
        {
            var subFacetoryName = $"{InspectedObject.DefaultFactory}.{suffix}";
            var exist = _factoryNames.IsItemExist(subFacetoryName);
            if (!exist)
            {
                LoadFactories();
            }
        }
    }
}
#endif