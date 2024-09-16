#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class PopupDataEditor
    {
        class AddPoolData : ScriptableObject
        {
            public List<PopupPartPoolCopyData> PendingPools = new();
            public static string ArrayName = "PendingPools";
        }
        AddPoolData _addPoolData;
        AddPoolData _AddPoolData
        {
            get
            {
                if (_addPoolData == null)
                {
                    _addPoolData = CreateInstance<AddPoolData>();
                }
                return _addPoolData;
            }
        }
        SerializedObject _addPoolDataSerializedObject;
        SerializedObject _AddPoolDataSerializedObject
        {
            get
            {
                if (_addPoolDataSerializedObject == null)
                {
                    _addPoolDataSerializedObject = new SerializedObject(_AddPoolData);
                }
                return _addPoolDataSerializedObject;
            }
        }
        bool HasPendingPool => _AddPoolData.PendingPools.GetCountSafe() > 0;
        void GUIPopupPool()
        {
            var prop = serializedObject.FindProperty("_pools");
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
        float GUIAddAndEditPool()
        {
            var labelWidth = EditorGUIHelper.HelpBox("Drag and drop here to edit and add Pools!", Color.yellow, _AddLabelWidth);
            EditorGUIHelper.GUIDragAndDrop(null, value =>
            {
                if (value is GameObject obj)
                {
                    var newData = new PopupPartPoolCopyData(obj);
                    var _popupPools = _AddPoolDataSerializedObject.FindProperty(AddPoolData.ArrayName);
                    var arraySize = _popupPools.arraySize;
                    InsertArrayElementAtIndex(_popupPools, arraySize);
                    var objectReference = GetArrayElementAtIndex(_popupPools, arraySize);
                    objectReference.FindPropertyRelative("_target").objectReferenceValue = value;
                }
            }, () =>
            {
                if (_AddPoolDataSerializedObject.hasModifiedProperties)
                {
                    _AddPoolDataSerializedObject.ApplyModifiedProperties();
                }
            });
            return labelWidth;
        }
        void GUIAddNewPool(SerializedObject PoolData)
        {
            var datas = PoolData.FindProperty(AddPoolData.ArrayName);
            EditorGUILayout.PropertyField(datas);
            if (PoolData.hasModifiedProperties)
            {
                PoolData.ApplyModifiedProperties();
            }
            GUIAddPartButtons(() =>
            {
                _PopupData.AddPools(_AddPoolData.PendingPools);
                _AddPoolDataSerializedObject.ClearArrayElements(AddPoolData.ArrayName);
            }, () =>
            {
                _AddPoolDataSerializedObject.ClearArrayElements(AddPoolData.ArrayName);
            });
        }
    }
}
#endif