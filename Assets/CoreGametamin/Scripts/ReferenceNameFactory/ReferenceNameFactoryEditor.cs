#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    public abstract partial class ReferenceNameFactoryEditor<T> : ReferenceNameFactoryEditor<ReferenceNameFactory<T>, T, ReferenceNameDataFactory> where T : ScriptableObject
    {
        protected override void GUIAddedName(ReferenceNameDataFactory item)
        {
            GUIAddedName(item.AddedNames);
        }
        void GUIAddedName(List<ReferenceNameData> _addedNames)
        {
            var indexRemoved = -1;
            var total = _addedNames.GetCountSafe();
            for (int i = 0; i < total; i++)
            {
                var index = i;
                EditorGUIHelper.HorizontalLayout(() =>
                {
                    GUIEditValue(_addedNames[index].Name
                        , edited =>
                        {
                            if (!InspectedObject.CheckNameExist(edited))
                            {
                                edited = edited.ToCamelCase();
                                _addedNames[index].Name = edited;
                            }
                            else
                            {
                                _textExist = edited;
                            }
                        }, "Name");
                    GUIEditValue(_addedNames[index].Value, edited =>
                    {
                        _addedNames[index].Value = edited;
                    }, "Value");
                    EditorGUIHelper.GUIButtonCancelSkin(() =>
                    {
                        indexRemoved = index;
                    });
                    GUILayout.FlexibleSpace();
                }, true, GUILayout.ExpandWidth(false));
            }
            if (indexRemoved >= 0)
            {
                _addedNames.RemoveAt(indexRemoved);
            }
            if (total > 0)
            {
                EditorGUIHelper.GUIButton(_clearButtonLabel, () =>
                {
                    _addedNames.SafeClear();
                });
            }
        }
    }
    public abstract partial class ReferenceNameFactoryEditor<T, U, V> : BaseCustomEditor where T : ReferenceNameFactory<U, V> where U : ScriptableObject where V : BaseDataFactory
    {
        protected string _text, _textExist;
        protected abstract T InspectedObject { get; }
        protected virtual bool IsRoot { get; } = true;
        protected virtual bool _ShowguiAddRefID { get; set; } = true;
        SerializedProperty _generateFolder;
        SerializedProperty _GenerateFolder
        {
            get
            {
                if (_generateFolder == null)
                {
                    _generateFolder = serializedObject.FindProperty("_generateFolder");
                }
                return _generateFolder;
            }
        }
        string _addIdLable;
        private void Awake()
        {
            LoadFactories();
            _addIdLable = $"Add {InspectedObject.ReferenceName}";
        }
        protected override void OnCustomGUI()
        {
            if (IsRoot)
            {
                _LabelWidth = EditorGUIHelper.GUILayoutPropertyField(_GenerateFolder, () => serializedObject.ApplyModifiedProperties());
            }
            AddByEnter();
            OnMiddleGUI();
            GUITextField();
        }
        public void AddByEnter()
        {
            Event e = Event.current;
            if (e.type == EventType.KeyUp && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
            {
                AddDefaultName(InspectedObject);
            }
        }
        protected virtual void OnMiddleGUI() { }
        public void GUITextField()
        {
            GUILayout.Space(EditorGUIHelper.GroupVerticalSpacing);
            EditorGUIHelper.ChangeGUIFoldoutLabelColor(() =>
            {
                _addIdLable.GUIFoldout(_ShowguiAddRefID, result =>
                {
                    _ShowguiAddRefID = result;
                });
            }, Color.yellow);
            if (!_ShowguiAddRefID) return;
            EditorGUIHelper.HelpBox("Add space to auto Pascal Case (Ex: 'pascal case' => 'PascalCase').\nUse  comma(,) to add multiple elements.", Color.yellow, () =>
            {
                GUIFitLabel(() =>
                {
                    GUIFactories();
                    EditorGUIHelper.HorizontalLayout(() =>
                    {
                        _LabelWidth = EditorGUIHelper.GUITextField(_text, "Reference ID", value =>
                        {
                            if (!string.IsNullOrEmpty(_textExist))
                            {
                                _textExist = null;
                            }
                            _text = value;
                        });
                        EditorGUIHelper.GUIButton("Add", () =>
                        {
                            AddDefaultName(InspectedObject);
                        });
                        EditorGUIHelper.GUIButton("Generate", () =>
                        {
                            InspectedObject.Generate();
                        });
                    });
                });
                if (!string.IsNullOrEmpty(_textExist))
                {
                    EditorGUIHelper.ChangeGUILabelColor(() =>
                    {
                        EditorGUIHelper.GUILabel($"ID \"{_textExist}\" is exist!");
                    }, Color.yellow);
                }
                var factories = InspectedObject.Factories;
                factories.For(item =>
                {
                    GUIAddedName(item);
                });
            });
        }
        void AddDefaultName(ReferenceNameFactory<U, V> InspectedObject)
        {
            if (!string.IsNullOrEmpty(_text))
            {
                GUI.FocusControl(null);
                var splits = _text.Split(',');
                splits.For(text =>
                {
                    var nameExist = InspectedObject.AddName(text);
                    if (nameExist)
                    {
                        _textExist += $"{text}; ";
                    }
                });
                _text = string.Empty;
            }
        }
        protected void CheckAndAddName(string text)
        {
            var nameExist = InspectedObject.CheckAndAddName(text);
            if (nameExist)
            {
                _textExist += $"{text}; ";
            }
        }
        protected abstract void GUIAddedName(V item);
        protected void GUIEditValue(string value, Action<string> onChanged, string label = default)
        {
            if (!label.IsNullOrEmptySafe())
            {
                label.GUILabel();
            }
            value.GUITextField(result =>
            {
                onChanged?.Invoke(result);
            });
        }

        protected virtual void InsertArrayElementAtIndex(SerializedProperty objectProperty, int index)
        {
            objectProperty.InsertArrayElementAtIndex(index);
        }
        protected virtual SerializedProperty GetArrayElementAtIndex(SerializedProperty objectProperty, int index)
        {
            //Assert.IsNotNull(objectProperty, $"{index}");
            var result = objectProperty.GetArrayElementAtIndex(index);
            //Assert.IsNotNull(result, $"{objectProperty.name} {index}");
            return result;
        }
        protected void Apply()
        {
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }
    }

}
#endif