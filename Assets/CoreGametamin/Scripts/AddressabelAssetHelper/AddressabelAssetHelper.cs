#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class AddressabelAssetHelper
    {
        public static AddressableAssetEntry CreateAssetEntry<T>(this T source, string groupName, string label) where T : Object
        {
            var entry = CreateAssetEntry(source, groupName);
            if (source != null)
            {
                source.AddAddressableAssetLabel(label);
            }

            return entry;
        }

        public static AddressableAssetEntry CreateAssetEntry<T>(T source, string groupName) where T : Object
        {
            if (source == null || string.IsNullOrEmpty(groupName) || !AssetDatabase.Contains(source))
                return null;

            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            var sourcePath = AssetDatabase.GetAssetPath(source);
            var sourceGuid = AssetDatabase.AssetPathToGUID(sourcePath);
            var group = !GroupExists(groupName) ? CreateGroup(groupName) : GetGroup(groupName);

            var entry = addressableSettings.CreateOrMoveEntry(sourceGuid, group);
            entry.address = sourcePath;

            addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

            return entry;
        }

        public static AddressableAssetEntry CreateAssetEntry<T>(T source) where T : Object
        {
            if (source == null || !AssetDatabase.Contains(source))
                return null;

            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            var sourcePath = AssetDatabase.GetAssetPath(source);
            var sourceGuid = AssetDatabase.AssetPathToGUID(sourcePath);
            var entry = addressableSettings.CreateOrMoveEntry(sourceGuid, addressableSettings.DefaultGroup);
            entry.address = sourcePath;

            addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

            return entry;
        }

        public static AddressableAssetGroup GetGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
                return null;

            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            return addressableSettings.FindGroup(groupName);
        }

        public static AddressableAssetGroup CreateGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
                return null;

            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            var group = addressableSettings.CreateGroup(groupName, false, false, false, addressableSettings.DefaultGroup.Schemas);

            addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.GroupAdded, group, true);

            return group;
        }
        //public static bool LabelExists(string groupName)
        //{
        //    var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
        //    return addressableSettings.label(groupName) != null;
        //}
        public static bool GroupExists(string groupName)
        {
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            return addressableSettings.FindGroup(groupName) != null;
        }
        public static void RemoveAddressableAssetLabel(this Object source, string label)
        {
            if (source == null || !AssetDatabase.Contains(source))
                return;

            var entry = source.GetAddressableAssetEntry();
            if (entry != null && entry.labels.Contains(label))
            {
                entry.labels.Remove(label);

                AddressableAssetSettingsDefaultObject.Settings.SetDirty(AddressableAssetSettings.ModificationEvent.LabelRemoved, entry, true);
            }
        }

        public static void AddAddressableAssetLabel(this Object source, string label)
        {
            if (source == null || !AssetDatabase.Contains(source))
                return;

            var entry = source.GetAddressableAssetEntry();
            if (entry != null && !entry.labels.Contains(label))
            {
                //entry.labels.Add(label);
                entry.SetLabel(label, true, true);
                var settings = AddressableAssetSettingsDefaultObject.Settings;
                settings.SetDirty(AddressableAssetSettings.ModificationEvent.LabelAdded, entry, true);
                AssetDatabase.SaveAssets();
            }
        }

        public static void SetAddressableAssetAddress(this Object source, string address)
        {
            if (source == null || !AssetDatabase.Contains(source))
                return;

            var entry = source.GetAddressableAssetEntry();
            if (entry != null)
            {
                entry.address = address;
            }
        }

        public static void SetAddressableAssetGroup(this Object source, string groupName)
        {
            if (source == null || !AssetDatabase.Contains(source))
                return;

            var group = !GroupExists(groupName) ? CreateGroup(groupName) : GetGroup(groupName);
            source.SetAddressableAssetGroup(group);
        }

        public static void SetAddressableAssetGroup(this Object source, AddressableAssetGroup group)
        {
            if (source == null || !AssetDatabase.Contains(source))
                return;

            var entry = source.GetAddressableAssetEntry();
            if (entry != null && !source.IsInAddressableAssetGroup(group.Name))
            {
                entry.parentGroup = group;
            }
        }

        public static HashSet<string> GetAddressableAssetLabels(this Object source)
        {
            if (source == null || !AssetDatabase.Contains(source))
                return null;

            var entry = source.GetAddressableAssetEntry();
            return entry?.labels;
        }

        public static string GetAddressableAssetPath(this Object source)
        {
            if (source == null || !AssetDatabase.Contains(source))
                return string.Empty;

            var entry = source.GetAddressableAssetEntry();
            return entry != null ? entry.address : string.Empty;
        }

        public static bool IsInAddressableAssetGroup(this Object source, string groupName)
        {
            if (source == null || !AssetDatabase.Contains(source))
                return false;

            var group = source.GetCurrentAddressableAssetGroup();
            return group != null && group.Name == groupName;
        }

        public static AddressableAssetGroup GetCurrentAddressableAssetGroup(this Object source)
        {
            if (source == null || !AssetDatabase.Contains(source))
                return null;

            var entry = source.GetAddressableAssetEntry();
            return entry?.parentGroup;
        }

        public static AddressableAssetEntry GetAddressableAssetEntry(this Object source)
        {
            if (source == null || !AssetDatabase.Contains(source))
            {
                Debug.Log("No Addressable Asset Entry found!");
                return null;
            }

            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            var sourcePath = AssetDatabase.GetAssetPath(source);
            var sourceGuid = AssetDatabase.AssetPathToGUID(sourcePath);
            return addressableSettings.FindAssetEntry(sourceGuid);
        }
        public static T GetObjectHasLabel<T>(this string lable) where T : Object
        {
            T result = default;
            var assetList = new List<AddressableAssetEntry>();
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            addressableSettings.GetAllAssets(assetList, false);
            assetList.ForBreakable(asset =>
            {
                if (asset.labels.Contains(lable))
                {
                    result = asset.AssetPath.LoadAssetAtPath<T>();
                    return true;
                }
                return false;
            });
            return result;
        }
        public static void GetObjectHasLabel<T>(this string lable, System.Action<T> callback) where T : Object
        {
            var assetList = new List<AddressableAssetEntry>();
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            addressableSettings.GetAllAssets(assetList, false);
            assetList.For(asset =>
            {
                if (asset.labels.Contains(lable))
                {
                    var result = asset.AssetPath.LoadAssetAtPath<T>();
                    if (!result.IsNullSafe())
                    {
                        callback?.Invoke(result);
                    }
                    else
                    {

                    }
                }
            });
        }
        /// <summary>
        /// Return object and parent group name
        /// </summary>
        public static void GetObjectHasLabel<T>(this string lable, System.Action<T, string> callback) where T : Object
        {
            var assetList = new List<AddressableAssetEntry>();
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            addressableSettings.GetAllAssets(assetList, false);
            assetList.For(asset =>
            {
                if (asset.labels.Contains(lable))
                {
                    var result = asset.AssetPath.LoadAssetAtPath<T>();
                    callback?.Invoke(result, asset.parentGroup.Name);
                }
            });
        }
        /// <summary>
        /// Return object and parent group name
        /// </summary>
        public static void GetObjectHasLabel<T>(System.Action<T, string, string> callback) where T : Object
        {
            var assetList = new List<AddressableAssetEntry>();
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            addressableSettings.GetAllAssets(assetList, false);
            assetList.For(asset =>
            {
                var result = asset.AssetPath.LoadAssetAtPath<T>();
                if (result != null)
                {
                    callback?.Invoke(result, asset.labels.First(), asset.parentGroup.Name);
                }
            });
        }
    }
}
#endif