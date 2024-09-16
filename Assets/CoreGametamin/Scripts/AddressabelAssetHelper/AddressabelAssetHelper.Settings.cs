#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.AddressableAssets;

namespace Gametamin.Core
{
    public static partial class AddressabelAssetHelper
    {
        public static void AddAddressabelLabel(this string label)
        {
            AddressableAssetSettingsDefaultObject.Settings.AddLabel(label);
        }
        public static List<string> GetLabels()
        {
            return AddressableAssetSettingsDefaultObject.Settings.GetLabels();
        }
        static List<string> _groupNames;
        static List<string> _GroupNames => _groupNames ??= new();
        public static List<string> GetGroupNames()
        {
            _GroupNames.Clear();
            var groups = AddressableAssetSettingsDefaultObject.Settings.groups;
            groups.For(group =>
            {
                if (!group.ReadOnly)
                {
                    _GroupNames.Add(group.Name);
                }
            });
            return _GroupNames;
        }
    }
}
#endif