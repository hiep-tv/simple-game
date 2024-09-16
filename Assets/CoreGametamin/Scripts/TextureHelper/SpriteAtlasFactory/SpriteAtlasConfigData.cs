#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Gametamin.Core
{
    [Serializable]
    public class SpriteAtlasConfigData
    {
        [SerializeField] AtlasConfig _atlasConfig;
        [SerializeField, AtlasAddressableLabel] string _addressableLabel;
        [SerializeField] string _addressableGroup;

        public SpriteAtlasConfigData(AtlasConfig atlasConfig, string addressableLabel, string addressableGroup)
        {
            _atlasConfig = atlasConfig;
            _addressableLabel = addressableLabel;
            _addressableGroup = addressableGroup;
        }

        public AtlasConfig AtlasConfig => _atlasConfig;
        public string AddressableLabel => _addressableLabel;
        public string AddressableGroup => _addressableGroup;
    }
}
#endif