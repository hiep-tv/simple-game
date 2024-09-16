#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    public class CustomSearchArrayElementInspector : Editor
    {
        SearchArrayData _searchArrayData;
        SearchArrayData _SearchArrayData => _searchArrayData ??= new(serializedObject);
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _SearchArrayData.GUISearchArray();
        }
    }
    public class SearchArrayData
    {
        SerializedObject serializedObject;
        public SearchArrayData(SerializedObject serializedObject)
        {
            this.serializedObject = serializedObject;
        }
        string[] _arrayNames;
        string[] _arrayDisplayNames;
        bool loaded;
        string arrayName = "";
        int arrayNameIndex;
        bool clicked;
        bool includeChildren;
        public void LoadData()
        {
            if (loaded) return;
            loaded = true;
            SerializedProperty property = serializedObject.GetIterator();
            property.LoadSerializedArrayProperty(ref _arrayNames, ref _arrayDisplayNames, includeChildren);
            arrayNameIndex = 0;
            if (_arrayDisplayNames.GetCountSafe() > 0)
            {
                arrayName = _arrayDisplayNames.GetSafe(0);
            }
        }
        public void GUISearchArray()
        {
            GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
            LoadData();
            EditorGUIHelper.HorizontalLayout(() =>
            {
                clicked = string.Empty.GUIStringWithSearch(arrayName, _arrayDisplayNames, clicked, (result, index) =>
                {
                    arrayNameIndex = index;
                    arrayName = result;
                    var arr = serializedObject.FindProperty(_arrayNames[arrayNameIndex]);
                    SearchWindow.Create(arr);
                });
                EditorGUIHelper.GUIToggle("Include Children", includeChildren, value =>
                {
                    includeChildren = value;
                    loaded = false;
                });
                if (GUILayout.Button("Search"))
                {
                    var arr = serializedObject.FindProperty(_arrayNames[arrayNameIndex]);
                    SearchWindow.Create(arr);
                }
            }, true);
        }
    }
    public static partial class SearchArrayHelper
    {
        public static void LoadSerializedArrayProperty(this SerializedProperty property, ref string[] arrayNames, ref string[] arrayDisplayNames, bool enterChild = false)
        {
            List<string> tempList = new();
            List<string> tempList2 = new();
            property.NextVisible(true);
            var index = 0;
            while (property.NextVisible(enterChild))
            {
                if (property.isArray && property.propertyType != SerializedPropertyType.String)
                {
                    if (enterChild)
                    {
                        tempList.Add($"{property.displayName} ({index})");
                    }
                    else
                    {
                        tempList.Add($"{property.displayName}");
                    }
                    tempList2.Add(property.propertyPath);
                    index++;
                }
            }
            arrayDisplayNames = tempList.ToArray();
            arrayNames = tempList2.ToArray();
            property.Reset();
        }
        public static string GetPropertyValueString(this SerializedProperty prop)
        {
            if (prop == null) throw new System.ArgumentNullException("prop");
            object value = default;
            switch (prop.propertyType)
            {
                case SerializedPropertyType.Integer:
                    value = prop.intValue;
                    break;
                case SerializedPropertyType.Boolean:
                    value = prop.boolValue;
                    break;
                case SerializedPropertyType.Float:
                    value = prop.floatValue;
                    break;
                case SerializedPropertyType.String:
                    value = prop.stringValue;
                    break;
                case SerializedPropertyType.Color:
                    value = prop.colorValue;
                    break;
                case SerializedPropertyType.ObjectReference:
                    value = prop.objectReferenceValue;
                    break;
                case SerializedPropertyType.LayerMask:
                    value = (LayerMask)prop.intValue;
                    break;
                case SerializedPropertyType.Enum:
                    value = prop.enumNames.GetSafe(prop.enumValueIndex);
                    break;
                case SerializedPropertyType.Vector2:
                    value = prop.vector2Value;
                    break;
                case SerializedPropertyType.Vector3:
                    value = prop.vector3Value;
                    break;
                case SerializedPropertyType.Vector4:
                    value = prop.vector4Value;
                    break;
                case SerializedPropertyType.Rect:
                    value = prop.rectValue;
                    break;
                case SerializedPropertyType.ArraySize:
                    value = prop.intValue;
                    break;
                case SerializedPropertyType.Character:
                    value = (char)prop.intValue;
                    break;
                case SerializedPropertyType.AnimationCurve:
                    value = prop.animationCurveValue;
                    break;
                case SerializedPropertyType.Bounds:
                    value = prop.boundsValue;
                    break;
                case SerializedPropertyType.Gradient:
                    throw new System.InvalidOperationException("Can not handle Gradient types.");
            }
            if (value != null)
            {
                return value.ToString();
            }
            return string.Empty;
        }
    }
}
#endif
