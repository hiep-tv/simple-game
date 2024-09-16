#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
namespace Gametamin.Core
{
    public static partial class EditorGUIHelper
    {
        public static float StandardVerticalSpacing => EditorGUIUtility.standardVerticalSpacing;
        public static float GroupVerticalSpacing => EditorGUIUtility.standardVerticalSpacing * 5;

        public static void DelayEditor(float time, System.Action callback = null)
        {
            var nextTime = EditorApplication.timeSinceStartup + time;
            EditorUtility.DisplayProgressBar("Waiting...", "Processing...", .2f);
            EditorApplication.update += OnScenGUI;
            void OnScenGUI()
            {
                if (EditorApplication.timeSinceStartup >= nextTime)
                {
                    EditorApplication.update -= OnScenGUI;
                    EditorUtility.ClearProgressBar();
                    callback?.Invoke();
                }
            }
        }
        public static void DelayEditorSilent(float time, System.Action callback = null)
        {
            var nextTime = EditorApplication.timeSinceStartup + time;
            EditorApplication.update += OnScenGUI;
            void OnScenGUI()
            {
                if (EditorApplication.timeSinceStartup >= nextTime)
                {
                    EditorApplication.update -= OnScenGUI;
                    callback?.Invoke();
                }
            }
        }
        public static void GUIPropertyField(this SerializedProperty property, Rect position, string label, ref float labelWidth)
        {
            var widthOfLabel = GetLabelWidth(label) + EditorGUIUtility.standardVerticalSpacing;
            if (widthOfLabel > labelWidth)
            {
                labelWidth = widthOfLabel;
            }
            else
            {
                widthOfLabel = labelWidth;
            }
            var width = position.width;
            position.width = widthOfLabel;
            GUI.Label(position, label);
            position.x += widthOfLabel;
            position.width = width - widthOfLabel;
            EditorGUI.PropertyField(position, property, GUIContent.none);
        }
        public static void GUIPropertyField(this SerializedProperty property, Rect position, string label)
        {
            var widthOfLabel = GetLabelWidth(label) + EditorGUIUtility.standardVerticalSpacing;
            var width = position.width;
            position.width = widthOfLabel;
            GUI.Label(position, label);
            position.x += widthOfLabel;
            position.width = width - widthOfLabel;
            EditorGUI.PropertyField(position, property, GUIContent.none);
        }
        public static void GUIPropertyField(this SerializedProperty property, Rect position, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
        }
        public static Vector2 GUIVector2Field(Rect position, Vector2 value, string label, ref float labelWidth)
        {
            var widthOfLabel = GetLabelWidth(label) + EditorGUIUtility.standardVerticalSpacing;
            if (widthOfLabel > labelWidth)
            {
                labelWidth = widthOfLabel;
            }
            else
            {
                widthOfLabel = labelWidth;
            }
            var width = position.width;
            position.width = widthOfLabel;
            GUI.Label(position, label);
            position.x += widthOfLabel;
            var contentWidth = (width - widthOfLabel) / 2f;
            position.width = contentWidth - EditorGUIUtility.standardVerticalSpacing / 2;
            value.x = GUIFloatField(position, value.x, "X");
            position.x += contentWidth + EditorGUIUtility.standardVerticalSpacing;
            value.y = GUIFloatField(position, value.y, "Y");
            return value;
        }
        public static Vector2 GUIVector2Field(Rect position, Vector2 value, string label)
        {
            var widthOfLabel = GetLabelWidth(label) + EditorGUIUtility.standardVerticalSpacing;
            var width = position.width;
            position.width = widthOfLabel;
            GUI.Label(position, label);
            position.x += widthOfLabel;
            var contentWidth = (width - widthOfLabel) / 2f;
            position.width = contentWidth - EditorGUIUtility.standardVerticalSpacing / 2;
            value.x = GUIFloatField(position, value.x, "X");
            position.x += contentWidth + EditorGUIUtility.standardVerticalSpacing;
            value.y = GUIFloatField(position, value.y, "Y");
            return value;
        }
        public static float GUIIntField(Rect position, int value)
        {
            var result = EditorGUI.IntField(position, value);
            return result;
        }
        public static float GUIIntField(Rect position, int value, string label, ref float labelWidth)
        {
            var width = position.width;
            var widthOfLabel = GetLabelWidth(label) + EditorGUIUtility.standardVerticalSpacing;
            if (widthOfLabel > labelWidth)
            {
                labelWidth = widthOfLabel;
            }
            else
            {
                widthOfLabel = labelWidth;
            }
            position.width = widthOfLabel;
            GUI.Label(position, label);
            position.x += widthOfLabel;
            position.width = width - widthOfLabel;
            var result = EditorGUI.IntField(position, value);
            return result;
        }
        public static float GUIIntField(Rect position, int value, string label)
        {
            var width = position.width;
            var labelWidth = GetLabelWidth(label) + EditorGUIUtility.standardVerticalSpacing;
            position.width = labelWidth;
            GUI.Label(position, label);
            position.x += labelWidth;
            position.width = width - labelWidth;
            var result = EditorGUI.IntField(position, value);
            return result;
        }
        public static float GUIFloatField(Rect position, float value, string label, ref float labelWidth)
        {
            var width = position.width;
            var widthOfLabel = GetLabelWidth(label) + EditorGUIUtility.standardVerticalSpacing;
            if (widthOfLabel > labelWidth)
            {
                labelWidth = widthOfLabel;
            }
            else
            {
                widthOfLabel = labelWidth;
            }
            position.width = widthOfLabel;
            GUI.Label(position, label);
            position.x += widthOfLabel;
            position.width = width - widthOfLabel;
            var result = EditorGUI.FloatField(position, value);
            return result;
        }
        public static void GUIFloatField(Rect position, float value, string label, Action<float> callback = null)
        {
            var width = position.width;
            var widthOfLabel = GetLabelWidth(label) + EditorGUIUtility.standardVerticalSpacing;
            position.width = widthOfLabel;
            GUI.Label(position, label);
            position.x += widthOfLabel;
            position.width = width - widthOfLabel;
            var result = value;
            GUIChangeCheck(() => result = EditorGUI.FloatField(position, value), () => callback?.Invoke(result));
        }
        public static float GUIFloatField(Rect position, float value, string label)
        {
            var width = position.width;
            var labelWidth = GetLabelWidth(label) + EditorGUIUtility.standardVerticalSpacing;
            position.width = labelWidth;
            GUI.Label(position, label);
            position.x += labelWidth;
            position.width = width - labelWidth;
            var result = EditorGUI.FloatField(position, value);
            return result;
        }
        public static void GUIEnumWithSearch<T>(Rect position, SerializedProperty property, Action callback = null) where T : struct
        {
            var id = (T)(object)property.intValue;
            GUIEnumWithSearch(position, id, (result) =>
            {
                property.intValue = (int)(object)result;
                property.serializedObject.ApplyModifiedProperties();
                callback?.Invoke();
            });
        }
        public static void GUIEnumWithSearch<T>(Rect position, T current, Action<T> callback) where T : struct
        {
            GUIButtonDropDown(position, current.ToString(), () =>
            {
                GUIEnumWithSearch<T>(position, callback);
            });
        }
        public static void GUIEnumWithSearch<T>(Rect position, Action<T> callback) where T : struct
        {
            var dropdown = new DropdownEnumWindowEditor<T>(typeof(T).Name, new AdvancedDropdownState(), callback);
            dropdown.Show(position);
        }
        public static void GUIStringWithSearch(Rect position, string label, SerializedProperty property, string[] values)
        {
            GUIStringWithSearch(position, label, property.stringValue, values, (value, Index) =>
            {
                property.stringValue = value;
                property.serializedObject.ApplyModifiedProperties();
            });
        }
        public static void GUIStringWithSearch(Rect position, string label, string current, string[] values, Action<string, int> callback)
        {
            GUIButtonDropDown(position, current.ToString(), () =>
            {
                var dropdown = new DropdownStringWindowEditor(label, values, new AdvancedDropdownState(), callback);
                position.width = Mathf.Max(position.width, label.GetLabelWidth() + EditorGUIUtility.singleLineHeight);
                dropdown.Show(position);
            });
        }
        public static void GUIStringWithSearch(Rect position, string label, string[] values, Action<string, int> callback)
        {
            var dropdown = new DropdownStringWindowEditor(label, values, new AdvancedDropdownState(), callback);
            dropdown.Show(position);
        }
        public static void GUIStringWithSearch(Rect position, string label, List<string> values, Action<string, int> callback)
        {
            var dropdown = new DropdownStringWindowEditor(label, values, new AdvancedDropdownState(), callback);
            dropdown.Show(position);
        }
        public static void GUIStringWithSearch(DropdownStringWindowEditor instance, Rect position, string label, string current, string[] values, Action<string, int> callback)
        {
            GUIButtonDropDown(position, current.ToString(), () =>
            {
                instance ??= new DropdownStringWindowEditor(label, values, new AdvancedDropdownState(), callback);
                instance.Show(position);
            });
        }
        public static float GUILabel(Rect rect, string label)
        {
            rect.width = GetLabelWidth(label);
            GUI.Label(rect, label);
            return rect.width;
        }
        public static float GUIButton(Rect rect, string label, Action callback = null)
        {
            rect.width = GetLabelWidth(label) + 20f;
            if (GUI.Button(rect, label))
            {
                callback?.Invoke();
            }
            return rect.width;
        }
        public static void GUIButtonDropDown(Rect position, string buttonName, Action callback)
        {
            _buttonDropDown.text = buttonName;
            if (EditorGUI.DropdownButton(position, _buttonDropDown, FocusType.Keyboard))
            {
                callback?.Invoke();
            }
        }
        public static void GUITextField(Rect position, string value, Action<string> callback)
        {
            EditorGUI.BeginChangeCheck();
            string result = EditorGUI.TextField(position, value);
            if (EditorGUI.EndChangeCheck())
            {
                if (result != value)
                {
                    callback?.Invoke(result);
                }
            }
        }
        public static void GUITextField(Rect position, string value, GUIStyle style, Action<string> callback)
        {
            EditorGUI.BeginChangeCheck();
            string result = EditorGUI.TextField(position, value, style);
            if (EditorGUI.EndChangeCheck())
            {
                if (result != value)
                {
                    callback?.Invoke(result);
                }
            }
        }
        public static void GUIObjectField(Rect position, string label, UnityEngine.Object valueObject, Type objectType, Action<UnityEngine.Object> callback = null, bool allowSceneObject = false)
        {
            EditorGUI.BeginChangeCheck();
            float originalValue = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = GetLabelWidth(label);
            var value = EditorGUI.ObjectField(position, valueObject, objectType, allowSceneObject);
            EditorGUIUtility.labelWidth = originalValue;
            if (EditorGUI.EndChangeCheck())
            {
                if (value != null)
                {
                    callback?.Invoke(value);
                }
            }
        }
        public static void GUIFoldout(Rect position, string label, bool foldout, Action<bool> callback)
        {
            EditorGUI.BeginChangeCheck();
            var value = EditorGUI.Foldout(position, foldout, label, true);
            if (EditorGUI.EndChangeCheck())
            {
                if (value != foldout)
                {
                    callback?.Invoke(value);
                }
            }
        }
        public static void ClearArrayElements(this SerializedObject serialized, string arrayName)
        {
            var array = serialized.FindProperty(arrayName);
            array.ClearArray();
            serialized.ApplyModifiedProperties();
        }
    }
}
#endif