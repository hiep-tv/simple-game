#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class PopupDataEditor
    {
        class AddPartData : ScriptableObject
        {
            public List<PopupPartCopyData> PendingParts = new();
            public static string ArrayName = "PendingParts";
        }
        AddPartData _addPartData;
        AddPartData _AddPartData
        {
            get
            {
                if (_addPartData == null)
                {
                    _addPartData = CreateInstance<AddPartData>();
                }
                return _addPartData;
            }
        }
        SerializedObject _addPartDataSerializedObject;
        SerializedObject _AddPartDataSerializedObject
        {
            get
            {
                if (_addPartDataSerializedObject == null)
                {
                    _addPartDataSerializedObject = new SerializedObject(_AddPartData);
                }
                return _addPartDataSerializedObject;
            }
        }
        bool HasPendingPart => _AddPartData.PendingParts.GetCountSafe() > 0;
        void GUIPopupPart()
        {
            var prop = serializedObject.FindProperty("_popupPartDatas");
            EditorGUILayout.PropertyField(prop);
            EditorGUIHelper.GUIDragAndDrop(null, value =>
            {
                if (value is GameObject obj)
                {
                    var arraySize = prop.arraySize;
                    InsertArrayElementAtIndex(prop, arraySize);
                    var objectReference = GetArrayElementAtIndex(prop, arraySize);
                    objectReference.FindPropertyRelative("_target").objectReferenceValue = value;
                }
            }, () =>
            {
                if (serializedObject.hasModifiedProperties)
                {
                    serializedObject.ApplyModifiedProperties();
                }
            });
            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        float GUIAddAndEditPart()
        {
            var labelWidth = EditorGUIHelper.HelpBox("Drag and drop here to edit and add parts!", Color.yellow, _AddLabelWidth);
            EditorGUIHelper.GUIDragAndDrop(null, value =>
            {
                if (value is GameObject obj)
                {
                    var newData = new PopupPartCopyData(obj);
                    var _popupParts = _AddPartDataSerializedObject.FindProperty(AddPartData.ArrayName);
                    var arraySize = _popupParts.arraySize;
                    InsertArrayElementAtIndex(_popupParts, arraySize);
                    var objectReference = GetArrayElementAtIndex(_popupParts, arraySize);
                    objectReference.FindPropertyRelative("_target").objectReferenceValue = value;
                }
            }, () =>
            {
                if (_AddPartDataSerializedObject.hasModifiedProperties)
                {
                    _AddPartDataSerializedObject.ApplyModifiedProperties();
                }
            });
            return labelWidth;
        }
        void GUIAddNewPart(SerializedObject partData)
        {
            var datas = partData.FindProperty(AddPartData.ArrayName);
            EditorGUILayout.PropertyField(datas);
            if (partData.hasModifiedProperties)
            {
                partData.ApplyModifiedProperties();
            }
            GUIAddPartButtons(() =>
            {
                _PopupData.AddParts(_AddPartData.PendingParts);
                _AddPartDataSerializedObject.ClearArrayElements(AddPartData.ArrayName);
            }, () =>
            {
                _AddPartDataSerializedObject.ClearArrayElements(AddPartData.ArrayName);
            });
        }
    }
}
#endif