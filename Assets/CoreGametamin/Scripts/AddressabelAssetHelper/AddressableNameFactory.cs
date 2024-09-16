#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public class AddressableNameFactory : ReferenceNameFactory<AddressableNameFactory>
    {
        protected override string _referenceName => "AddressableLabels";
        protected override string ValidateName(string name)
        {
            return name.ToSnakeCase().UpperFirstCase();
        }
        protected override string ValidateValue(string value)
        {
            return value.ToSnakeCase().ToLower();
        }
        protected override void GenerateInternal()
        {
            base.GenerateInternal();
            GenerateAtlasValues();
            GeneratePopupDataValues();
        }
        void GenerateAtlasValues()
        {
            var suffix = "atlas";
            var values = new List<string>();
            _referenceNameDatas.For((item, index) =>
            {
                if (item.Value.ContainsSafe(suffix))
                {
                    values.Add(item.Value);
                }
            });
            ClassGenerator.GenerateStaticClassValues(ReferenceName, values.ToArray(), PathFolder, "Atlas");
        }
        void GeneratePopupDataValues()
        {
            var suffix = "popupdata";
            var values = new List<string>();
            _referenceNameDatas.For((item, index) =>
            {
                if (item.Value.ContainsSafe(suffix))
                {
                    values.Add(item.Value);
                }
            });
            ClassGenerator.GenerateStaticClassValues(ReferenceName, values.ToArray(), PathFolder, "Popup");
        }

        [ContextMenu("Add to system")]
        public void AddToSystem()
        {
            var list = GetUniques();
            list.For(item =>
            {
                item.Value.AddAddressabelLabel();
            });
        }
    }
}
#endif
