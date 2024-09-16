#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Object = UnityEngine.Object;
namespace Gametamin.Core
{
    public static partial class EditorGUIHelper
    {
        static GUIContent _guiContent;
        static GUIContent _GuiContent => _guiContent ??= new();
        static GUIContent _guiContentText;
        static GUIContent _GuiContentText => _guiContentText ??= new();
        static GUIStyle _warningLabelStyle;
        public static GUIStyle WarningLabelStyle => _warningLabelStyle ??= new(GUI.skin.label);
        const string _helpBox = "HelpBox";
        public static void HelpBox(this string label, bool expand = true)
        {
            VerticleLayout(() =>
            {
                GUILabel(label, expand);
            }, true);
        }
        public static void HelpBox(Texture texture, Action onDraw = null, params GUILayoutOption[] layoutOptions)
        {
            VerticleLayout(() =>
            {
                GUILabel(texture, layoutOptions);
                onDraw?.Invoke();
            }, true);
        }
        public static float HelpBox(this string label, Color labelColor, Action onDraw = null, bool center = true)
        {
            VerticleLayout(() =>
            {
                ChangeGUILabelColor(() =>
                {
                    if (center)
                    {
                        ChangeGUILabelAlignment(() => GUILabel(label), TextAnchor.MiddleCenter);
                    }
                    else
                    {
                        //GUILabel(label);
                        GUILayout.Label(label);
                    }
                }, labelColor);
                onDraw?.Invoke();
            }, true);
            return label.GetLabelWidth();
        }
        public static float HelpBox(this string label, Color labelColor, float width)
        {
            VerticleLayout(() =>
            {
                ChangeGUILabelColor(() =>
                {
                    if (width > 0)
                    {
                        GUILayout.Label(label, GUILayout.Width(width));
                    }
                    else
                    {
                        GUILayout.Label(label);
                    }
                }, labelColor);
            }, true);
            return label.GetLabelWidth();
        }
        public static void ChangeGUILabelColor(Action callback, Color color)
        {
            var current = GUI.skin.label.normal.textColor;
            GUI.skin.label.normal.textColor = color;
            callback?.Invoke();
            GUI.skin.label.normal.textColor = current;
        }
        public static void ChangeGUIFoldoutLabelColor(Action callback, Color color)
        {
            var current = EditorStyles.foldout.normal.textColor;
            EditorStyles.foldout.normal.textColor = color;
            callback?.Invoke();
            EditorStyles.foldout.normal.textColor = current;
        }
        public static void ChangeGUILabelAlignment(Action callback, TextAnchor textAnchor)
        {
            var current = GUI.skin.label.alignment;
            GUI.skin.label.alignment = textAnchor;
            callback?.Invoke();
            GUI.skin.label.alignment = current;
        }
        public static void DisabledGUI(Action callback, bool enabled = false)
        {
            var current = GUI.enabled;
            GUI.enabled = enabled;
            callback?.Invoke();
            GUI.enabled = current;
        }
        public static void ChangeGUIColor(Action callback, Color color)
        {
            var current = GUI.color;
            GUI.color = color;
            callback?.Invoke();
            GUI.color = current;
        }
        public static void ChangeGUILabelWidth(float labelWidth, Action callback)
        {
            var current = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = labelWidth;
            callback?.Invoke();
            EditorGUIUtility.labelWidth = current;
        }
        public static void ShowGUIConfirm(this string title, string message, Action<bool> callback, string ok = "OK", string cancel = "Cancel")
        {
            bool accepted = EditorUtility.DisplayDialog(title, message, ok, cancel);
            callback?.Invoke(accepted);
        }
        public static void GUILabel(this string label, bool expand = true)
        {
            _GuiContentText.text = label;
            _GuiContentText.tooltip = label;
            if (expand)
            {
                GUILayout.Label(_GuiContentText);
            }
            else
            {
                var width = GetLabelWidth(label);
                GUILayout.Label(_GuiContentText, GUILayout.Width(width));
            }
        }
        public static void GUILabel(this string label, params GUILayoutOption[] layoutOptions)
        {
            _GuiContentText.text = label;
            _GuiContentText.tooltip = label;
            GUILayout.Label(_GuiContentText, layoutOptions);
        }
        public static void GUILabel(this string label, ref float width)
        {
            var currentWidth = GetLabelWidth(label);
            if (currentWidth < width)
            {
                currentWidth = width;
            }
            else
            {
                width = currentWidth;
            }
            GUILayout.Label(label, GUILayout.Width(currentWidth));
        }
        public static void GUILabel(this Texture texture, params GUILayoutOption[] layoutOptions)
        {
            GUILayout.Label(texture, layoutOptions);
        }

        static GUIStyle _labelStyle;
        public static GUIStyle LabelStyle => _labelStyle ??= new GUIStyle(EditorStyles.label);
        public static void GUIToggle(this string label, bool value, Action<bool> callback)
        {
            EditorGUI.BeginChangeCheck();
            var labelWidth = GetLabelWidth(label);
            float originalValue = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = labelWidth;
            var result = EditorGUILayout.ToggleLeft(label, value, GUILayout.Width(labelWidth + 15f));
            EditorGUIUtility.labelWidth = originalValue;
            if (EditorGUI.EndChangeCheck())
            {
                if (result != value)
                {
                    callback?.Invoke(result);
                }
            }
        }
        public static void GUIToggle(this string label, bool value, Action<bool> callback, bool helpBox = true, params GUILayoutOption[] layouts)
        {
            EditorGUI.BeginChangeCheck();
            HorizontalLayout(() =>
            {
                var result = EditorGUILayout.ToggleLeft(label, value, layouts);
                if (EditorGUI.EndChangeCheck())
                {
                    if (result != value)
                    {
                        callback?.Invoke(result);
                    }
                }
            }, helpBox);
        }
        public static float GUIObjectField(this string label, Object valueObject, Type objectType, Action<Object> callback, bool allowSceneObject = false, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            var value = EditorGUILayout.ObjectField(label, valueObject, objectType, allowSceneObject, options);
            if (EditorGUI.EndChangeCheck())
            {
                callback?.Invoke(value);
            }
            return label.GetLabelWidth();
        }
        public static void GUIObjectField(Object valueObject, Type objectType, Action<Object> callback, bool allowSceneObject = false, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            var value = EditorGUILayout.ObjectField(valueObject, objectType, allowSceneObject, options);
            if (EditorGUI.EndChangeCheck())
            {
                if (value != null)
                {
                    callback?.Invoke(value);
                }
            }
        }
        public static void GUIObjectFieldFixed(Object valueObject, Type objectType, Action<Object> callback, float width = 100f, bool allowSceneObject = false)
        {
            EditorGUI.BeginChangeCheck();
            var value = EditorGUILayout.ObjectField(valueObject, objectType, allowSceneObject, GUILayout.Width(width));
            if (EditorGUI.EndChangeCheck())
            {
                if (value != null)
                {
                    callback?.Invoke(value);
                }
            }
        }
        public static void GUIObjectFieldSingleLine(this string label, Object valueObject, Type objectType, Action<Object> callback, bool allowSceneObject = false, float minWidth = 50f)
        {
            EditorGUI.BeginChangeCheck();
            var value = EditorGUILayout.ObjectField(label, valueObject, objectType, allowSceneObject, GUILayout.MinWidth(minWidth), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            if (EditorGUI.EndChangeCheck())
            {
                if (value != null)
                {
                    callback?.Invoke(value);
                }
            }
        }
        public static void GUIFoldout(this string label, bool foldout, Action<bool> callback)
        {
            EditorGUI.BeginChangeCheck();
            var value = EditorGUILayout.Foldout(foldout, label, true);
            if (EditorGUI.EndChangeCheck())
            {
                if (value != foldout)
                {
                    callback?.Invoke(value);
                }
            }
        }
        public static void HorizontalLayout(Action callback, bool flexibleSpace = false)
        {
            GUILayout.BeginHorizontal();
            callback?.Invoke();
            if (flexibleSpace)
            {
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
        public static void HorizontalLayout(Action callback, GUIStyle style)
        {
            GUILayout.BeginHorizontal(style);
            callback?.Invoke();
            GUILayout.EndHorizontal();
        }
        public static void HorizontalLayout(Action callback, params GUILayoutOption[] layoutOptions)
        {
            GUILayout.BeginHorizontal(layoutOptions);
            callback?.Invoke();
            GUILayout.EndHorizontal();
        }
        public static void HorizontalLayout(Action callback, bool helpBox = false, params GUILayoutOption[] layoutOptions)
        {
            if (helpBox)
            {
                GUILayout.BeginHorizontal(_helpBox, layoutOptions);
            }
            else
            {
                GUILayout.BeginHorizontal(layoutOptions);
            }
            callback?.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void VerticleLayout(Action callback, bool helpBox = false)
        {
            if (helpBox)
            {
                GUILayout.BeginVertical(_helpBox);
            }
            else
            {
                GUILayout.BeginVertical();
            }
            callback?.Invoke();
            GUILayout.EndVertical();
        }
        public static void VerticleLayout(Action callback, bool helpBox = false, params GUILayoutOption[] layouts)
        {
            if (helpBox)
            {
                GUILayout.BeginVertical(_helpBox, layouts);
            }
            else
            {
                GUILayout.BeginVertical(layouts);
            }
            callback?.Invoke();
            GUILayout.EndVertical();
        }
        public static float GUIIntField(this string label, int value, Action<int> callback, float labelWidth = 0f)
        {
            labelWidth = labelWidth > 0 ? labelWidth : GetLabelWidth(label);
            float originalValue = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.BeginChangeCheck();
            int result = EditorGUILayout.IntField(label, value);
            EditorGUIUtility.labelWidth = originalValue;
            if (EditorGUI.EndChangeCheck())
            {
                if (result != value)
                {
                    callback?.Invoke(result);
                }
            }
            return labelWidth;
        }
        public static float GUIFloatField(this string label, float value, Action<float> callback, float labelWidth = 0f)
        {
            labelWidth = labelWidth > 0 ? labelWidth : GetLabelWidth(label);
            float originalValue = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.BeginChangeCheck();
            float result = EditorGUILayout.FloatField(label, value);
            EditorGUIUtility.labelWidth = originalValue;
            if (EditorGUI.EndChangeCheck())
            {
                if (result != value)
                {
                    callback?.Invoke(result);
                }
            }
            return labelWidth;
        }
        public static float GUITextField(this string value, string label, Action<string> callback, float width = 0f)
        {
            HorizontalLayout(() =>
            {
                EditorGUI.BeginChangeCheck();
                string result;
                result = EditorGUILayout.TextField(label, value, GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    if (result != value)
                    {
                        callback?.Invoke(result);
                    }
                }
            });
            return label.GetLabelWidth();
        }
        public static float DelayedTextField(this string value, string label, Action<string> callback, float width = 0f)
        {
            HorizontalLayout(() =>
            {
                EditorGUI.BeginChangeCheck();
                string result;
                result = EditorGUILayout.DelayedTextField(label, value, GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    if (result != value)
                    {
                        callback?.Invoke(result);
                    }
                }
            });
            return label.GetLabelWidth();
        }
        public static void GUITextField(this string value, Action<string> callback, float width = 0f)
        {
            EditorGUI.BeginChangeCheck();
            string result;
            if (width > 0f)
            {
                result = EditorGUILayout.TextField(value, GUILayout.Width(width));
            }
            else
            {
                result = EditorGUILayout.TextField(value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (result != value)
                {
                    callback?.Invoke(result);
                }
            }
        }
        public static void GUITextField(this string value, GUIStyle style, Action<string> callback, float width = 0f)
        {
            EditorGUI.BeginChangeCheck();
            string result = EditorGUILayout.TextField(value, style);
            if (EditorGUI.EndChangeCheck())
            {
                if (result != value)
                {
                    callback?.Invoke(result);
                }
            }
        }
        static GUIStyle _style;
        static GUIStyle style => _style ??= new(EditorStyles.textArea)
        {
            wordWrap = true,
        };
        public static void GUITextArea(this string value, Action<string> callback, float minWidth = 100f)
        {
            EditorGUI.BeginChangeCheck();
            string result;
            var valueWidth = GetLabelWidth(value);
            if (valueWidth < minWidth)
            {
                result = EditorGUILayout.TextArea(value, GUILayout.MinWidth(minWidth));
            }
            else
            {
                result = EditorGUILayout.TextArea(value, style);
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (!result.EqualsSafe(value))
                {
                    callback?.Invoke(result);
                }
            }
        }
        public static void GUIButtonFixedLabel(this string buttonName, Action callback, float minWidth = 50f)
        {
            var labelWidth = GetLabelWidth(buttonName);
            if (GUILayout.Button(buttonName, GUILayout.Width(labelWidth + 10), GUILayout.MinWidth(minWidth)))
            {
                callback?.Invoke();
            }
        }
        public static void GUIButton(this string buttonName, Action callback)
        {
            if (GUILayout.Button(buttonName))
            {
                callback?.Invoke();
            }
        }
        public static void GUIButton(this string buttonName, Action callback, params GUILayoutOption[] layouts)
        {
            if (GUILayout.Button(buttonName, layouts))
            {
                callback?.Invoke();
            }
        }
        public static void GUIButton(Texture buttonTexture, Action callback, params GUILayoutOption[] layouts)
        {
            _GuiContent.image = buttonTexture;
            if (buttonTexture != null)
            {
                _GuiContent.tooltip = $"{buttonTexture?.name} ({buttonTexture.width}x{buttonTexture.height})";
            }
            if (GUILayout.Button(_GuiContent, layouts))
            {
                callback?.Invoke();
            }
        }
        public static void GUIButton(GUIContent buttonTexture, Action callback, params GUILayoutOption[] layouts)
        {
            if (GUILayout.Button(buttonTexture, layouts))
            {
                callback?.Invoke();
            }
        }
        public static void GUIButtonFixedWidth(this string buttonName, Action callback, float width = 50f)
        {
            if (GUILayout.Button(buttonName, GUILayout.Width(width)))
            {
                callback?.Invoke();
            }
        }
        public static void GUIButtonCancelSkin(Action callback)
        {
            if (GUILayout.Button(string.Empty, GUI.skin.FindStyle(TOOLBARSEACHCANCELBUTTON), GUILayout.Width(20f)))
            {
                callback?.Invoke();
            }
        }
        /// <summary>
        /// using GUILayoutUtility.GetLastRect. Useful in inspector.
        /// </summary>
        /// <returns></returns>
        public static bool GUIEnumWithSearch<T>(T current, bool clicked, Action<T> callback, float dropDownButtonMinWidth = 200f) where T : struct
        {
            GUIButtonDropDown(current.ToString(), () =>
            {
                clicked = true;
            }, GUILayout.MinWidth(dropDownButtonMinWidth));
            Event e = Event.current;
            if (e.type == EventType.Repaint)
            {
                if (clicked)
                {
                    clicked = false;
                    GUIEnumWithSearch<T>(GUILayoutUtility.GetLastRect(), callback);
                }
            }
            return clicked;
        }
        /// <summary>
        /// using GUILayoutUtility.GetLastRect. Useful in inspector.
        /// </summary>
        /// <returns></returns>
        public static bool GUIStringWithSearch(this string label, string current, string[] values, bool clicked, Action<string, int> callback, float dropDownButtonMinWidth = 200f)
        {
            if (string.IsNullOrEmpty(current))
            {
                current = string.Empty;
            }
            GUIButtonDropDown(current.ToString(), () =>
            {
                clicked = true;
            }, GUILayout.MinWidth(dropDownButtonMinWidth));
            Event e = Event.current;
            if (e.type == EventType.Repaint)
            {
                if (clicked)
                {
                    clicked = false;
                    GUIStringWithSearch(GUILayoutUtility.GetLastRect(), label, values, callback);
                }
            }
            return clicked;
        }/// <summary>
         /// using GUILayoutUtility.GetLastRect. Useful in inspector.
         /// </summary>
         /// <returns></returns>
        public static bool GUIStringWithSearch(this string label, string current, List<string> values, bool clicked, Action<string, int> callback, float dropDownButtonMinWidth = 200f)
        {
            if (string.IsNullOrEmpty(current))
            {
                current = string.Empty;
            }
            GUIButtonDropDown(current.ToString(), () =>
            {
                clicked = true;
            }, GUILayout.MinWidth(dropDownButtonMinWidth));
            Event e = Event.current;
            if (e.type == EventType.Repaint)
            {
                if (clicked)
                {
                    clicked = false;
                    GUIStringWithSearch(GUILayoutUtility.GetLastRect(), label, values, callback);
                }
            }
            return clicked;
        }
        static GUIContent _buttonDropDown = new GUIContent();
        public static void GUIButtonDropDown(this string buttonName, Action callback, params GUILayoutOption[] layouts)
        {
            _buttonDropDown.text = buttonName;
            if (EditorGUILayout.DropdownButton(_buttonDropDown, FocusType.Keyboard, layouts))
            {
                callback?.Invoke();
            }
        }
        public static float GUIIntPopup(this string label, int value, string[] names, int[] values, Action<int> callback)
        {
            EditorGUI.BeginChangeCheck();
            var result = EditorGUILayout.IntPopup(label, value, names, values);
            Rect rect = GUILayoutUtility.GetLastRect();
            if (EditorGUI.EndChangeCheck())
            {
                if (result != value)
                {
                    callback?.Invoke(result);
                }
            }
            return rect.width;
        }
        public static void GUIEnumPopup(this Enum value, Action<Enum> callback, float maxWidth = 200f)
        {
            EditorGUI.BeginChangeCheck();
            var result = EditorGUILayout.EnumPopup(value, GUILayout.MaxWidth(maxWidth));
            if (EditorGUI.EndChangeCheck())
            {
                if (result != value)
                {
                    callback?.Invoke(result);
                }
            }
        }
        public static float GUIEnumPopup(this string label, Enum value, Action<Enum> callback, float maxWidth = 200f)
        {
            var labelWidth = GetLabelWidth(label);
            float originalValue = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.BeginChangeCheck();
            var result = EditorGUILayout.EnumPopup(label, value, GUILayout.MaxWidth(maxWidth));
            EditorGUIUtility.labelWidth = originalValue;
            if (EditorGUI.EndChangeCheck())
            {
                if (result != value)
                {
                    callback?.Invoke(result);
                }
            }
            return labelWidth;
        }
        public static void GUIPropertyField(Rect position, SerializedProperty property, GUIContent label, Action onChanged = null)
        {
            GUIChangeCheck(() => EditorGUI.PropertyField(position, property, label), onChanged);
        }
        public static float GUILayoutPropertyField(this SerializedProperty property, Action onChanged = null)
        {
            GUIChangeCheck(() => EditorGUILayout.PropertyField(property), onChanged);
            return property.displayName.GetLabelWidth();
        }
        public static void GUIChangeCheck(Action beginChangeCheck, Action endChangeCheck)
        {
            EditorGUI.BeginChangeCheck();
            beginChangeCheck?.Invoke();
            if (EditorGUI.EndChangeCheck())
            {
                endChangeCheck?.Invoke();
            }
        }

        public static Vector2 GUIScrollView(Vector2 scrollViewVector, Action callback)
        {
            GUILayout.Space(10f);
            scrollViewVector = GUILayout.BeginScrollView(scrollViewVector);
            callback?.Invoke();
            GUILayout.EndScrollView();
            return scrollViewVector;
        }
        public static Vector2 GUIScrollView(Vector2 scrollViewVector, Action callback, float height)
        {
            GUILayout.Space(10f);
            scrollViewVector = GUILayout.BeginScrollView(scrollViewVector, GUILayout.Height(height));
            callback?.Invoke();
            GUILayout.EndScrollView();
            return scrollViewVector;
        }
        public static void GUIDragAndDrop(Action<int> onDrop, Action<Object> callback, Action onEndDrag = null)
        {
            Rect myRect = GUILayoutUtility.GetLastRect();
            if (myRect.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.DragUpdated)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.DragPerform)
                {
                    int length = DragAndDrop.objectReferences.GetCountSafe();
                    if (length > 0)
                    {
                        onDrop?.Invoke(length);
                        DragAndDrop.objectReferences.For((item, index) =>
                        {
                            callback?.Invoke(item);
                        });
                        DragAndDrop.visualMode = DragAndDropVisualMode.None;
                        onEndDrag?.Invoke();
                    }
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.DragExited)
                {
                    DragAndDrop.PrepareStartDrag();
                }
            }
        }
        public static void GUIDragAndDrops(Action<Object[]> callback = null)
        {
            Rect myRect = GUILayoutUtility.GetLastRect();
            if (myRect.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.DragUpdated)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.DragPerform)
                {
                    int length = DragAndDrop.objectReferences.GetCountSafe();
                    if (length > 0)
                    {
                        callback?.Invoke(DragAndDrop.objectReferences);
                    }
                    DragAndDrop.visualMode = DragAndDropVisualMode.None;
                    Event.current.Use();
                }
            }
        }
        public static void GUIDragAndDrop(Rect effectRect, Action<int> onDrop, Action<Object> callback, Action onEndDrag = null)
        {
            if (effectRect.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.DragUpdated)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                    Event.current.Use();
                }
                else if (Event.current.type == EventType.DragPerform)
                {
                    int length = DragAndDrop.objectReferences.GetCountSafe();
                    if (length > 0)
                    {
                        onDrop?.Invoke(length);
                        DragAndDrop.objectReferences.For((item, index) =>
                        {
                            callback?.Invoke(item);
                        });
                        onEndDrag?.Invoke();
                    }
                    Event.current.Use();
                }
            }
        }
        public static T[] AddRange<T>(T[] source, T[] destination)
        {
            int sourceLength = source.GetCountSafe();
            int destinationLength = destination.GetCountSafe();
            int length = sourceLength + destinationLength;
            T[] tempArray = new T[length];
            for (int i = 0; i < length; i++)
            {
                if (i < sourceLength)
                {
                    tempArray[i] = source[i];
                }
                else
                {
                    tempArray[i] = destination[i - sourceLength];
                }
            }
            return tempArray;
        }

        public static T[] AddElements<T>(T[] source, int length)
        {
            int sourceLength = source.GetCountSafe();
            T[] tempArray = new T[length];
            sourceLength = sourceLength > length ? length : sourceLength;
            for (int i = 0; i < sourceLength; i++)
            {
                tempArray[i] = source[i];
            }
            return tempArray;
        }

        public static float GetHighestLabelSize(params string[] labels)
        {
            float maxWidth = 0f;
            for (int i = 0, length = labels.Length; i < length; i++)
            {
                var width = GetLabelWidth(labels[i]);
                if (maxWidth < width)
                {
                    maxWidth = width;
                }
            }
            return maxWidth;
        }

        public static float GetLabelWidth(this string content)
        {
            return GetLabelSize(content).x;
        }
        public static float GetLabelWidth(this GUIContent content)
        {
            return GetLabelSize(content).x;
        }
        public static float GetLabelHeight(this string content)
        {
            return GetLabelSize(content).y;
        }
        public static float GetLabelHeight(this GUIContent content)
        {
            return GetLabelSize(content).y;
        }
        public static Vector2 GetLabelSize(this string content)
        {
            return GetLabelSize(new GUIContent(content));
        }
        public static Vector2 GetLabelSize(this GUIContent content)
        {
            return GUI.skin.label.CalcSize(content);
        }
#if UNITY_2022_1_OR_NEWER || UNITY_2021_3_33 || UNITY_2021_3_35
        const string TOOLBAR = "Toolbar";
        const string TOOLBARSEACHTEXTFIELD = "ToolbarSearchTextField";
        const string TOOLBARSEACHCANCELBUTTON = "ToolbarSearchCancelButton";
        const string SEARCH = "Search";
#else
        const string TOOLBAR = "Toolbar";
        const string TOOLBARSEACHTEXTFIELD = "ToolbarSeachTextField";
        const string TOOLBARSEACHCANCELBUTTON = "ToolbarSeachCancelButton";
        const string SEARCH = "Search";
#endif
        public static void GUISearchBar(this string searchString, Action<string> OnInput)
        {
            GUILayout.BeginHorizontal(GUI.skin.FindStyle(TOOLBAR));
            GUILayout.FlexibleSpace();
            EditorGUI.BeginChangeCheck();
            GUITextField(searchString, GUI.skin.FindStyle(TOOLBARSEACHTEXTFIELD), OnInput);
            if (GUILayout.Button(SEARCH, GUI.skin.FindStyle(TOOLBARSEACHCANCELBUTTON)))
            {
                // Remove focus if cleared
                OnInput?.Invoke(string.Empty);
                GUI.FocusControl(null);
            }
            GUILayout.EndHorizontal();
        }
        public static void GUISearchBar(Rect position, string searchString, Action<string> OnInput)
        {
            EditorGUI.BeginChangeCheck();
            var buttonSize = 20f;
            position.width -= buttonSize;
            GUITextField(position, searchString, GUI.skin.FindStyle(TOOLBARSEACHTEXTFIELD), OnInput);
            position.x += position.width;
            position.width = buttonSize;
            if (GUI.Button(position, SEARCH, GUI.skin.FindStyle(TOOLBARSEACHCANCELBUTTON)))
            {
                // Remove focus if cleared
                OnInput?.Invoke(string.Empty);
                GUI.FocusControl(null);
            }
        }
        public static void OnDrawHorizontal(this SerializedProperty property, Rect position, GUIContent label, GUIContent[] propertyRelatives, string[] propertyName, Func<SerializedProperty, int, bool> callback = null)
        {
            property.OnDraw(position, label, (contentPosition) =>
            {
                float width = contentPosition.width / propertyRelatives.GetCountSafe();
                contentPosition.width = width;
                propertyRelatives.ForBreakable((guiContent, index) =>
                {
                    EditorGUIUtility.labelWidth = GetLabelWidth(guiContent);
                    var relativeProperty = property.FindPropertyRelative(propertyName.GetSafe(index));
                    EditorGUI.PropertyField(contentPosition, relativeProperty, guiContent);
                    var isBreak = callback?.Invoke(relativeProperty, index);
                    contentPosition.x += width + 5f;
                    return isBreak.HasValue && isBreak.Value;
                });
            });
        }
        public static Rect OnDrawHorizontal(this SerializedProperty property, Rect position, GUIContent label, GUIContent[] propertyRelatives, string[] propertyName, Action<Rect, SerializedProperty, int> callback = null)
        {
            var lastRect = position;
            property.OnDraw(position, label, (contentPosition) =>
            {
                float width = contentPosition.width / propertyRelatives.GetCountSafe();
                contentPosition.width = width;
                propertyRelatives.For((guiContent, index) =>
                {
                    EditorGUIUtility.labelWidth = GetLabelWidth(guiContent);
                    var relativeProperty = property.FindPropertyRelative(propertyName.GetSafe(index));
                    callback?.Invoke(contentPosition, relativeProperty, index);
                    contentPosition.x += width + 5f;
                    lastRect = contentPosition;
                });
            });
            return lastRect;
        }
        public static void OnDraw(this SerializedProperty property, Rect position, GUIContent label, Action<Rect> callback)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            {
                Rect rect = EditorGUI.PrefixLabel(position, label);

                int indentLevel = EditorGUI.indentLevel;
                float labelWidth = EditorGUIUtility.labelWidth;

                EditorGUI.indentLevel = 0;
                callback(rect);
                EditorGUIUtility.labelWidth = labelWidth;
                EditorGUI.indentLevel = indentLevel;
            }
            EditorGUI.EndProperty();
        }
        public static void OnInspectorGUI(this SerializedObject serializedObject, Action<SerializedProperty> callback)
        {
            serializedObject.Update();

            SerializedProperty property = serializedObject.GetIterator();

            // Script
            if (property.NextVisible(true))
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(property);
                GUI.enabled = true;
            }

            callback?.Invoke(property);

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
        public static void GUIMonoBehaviourScript(this Object target)
        {
            DisabledGUI(() =>
            {
                var mono = (MonoBehaviour)target;
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mono), target.GetType(), false);
            });
        }
    }
}
#endif