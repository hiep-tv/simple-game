#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public abstract class BaseDataFactory : ScriptableObject
    {
        public abstract bool AddName(string newName, string value);
        public abstract bool IsNameExist(string newName);
    }
    public partial class ReferenceNameDataFactory : BaseDataFactory
    {
        [SerializeField] protected List<ReferenceNameData> _referenceNameDatas;
        protected List<ReferenceNameData> _ReferenceNameDatas => _referenceNameDatas ??= new();
        protected List<ReferenceNameData> _addedNames;
        public List<ReferenceNameData> AddedNames => _addedNames ??= new();
        public override bool AddName(string newName, string value)
        {
            var isExist = IsNameExist(newName);
            if (!isExist)
            {
                AddedNames.Add(new ReferenceNameData(newName, value));
                this.MakeObjectDirty(true);
            }
            return isExist;
        }
        public override bool IsNameExist(string newName)
        {
            var isExist = _ReferenceNameDatas.IsItemExist(newName);
            if (!isExist)
            {
                isExist = AddedNames.IsItemExist(newName);
            }
            return isExist;
        }
        public void AddNames(List<ReferenceNameData> referenceNameDatas)
        {
            _ReferenceNameDatas.SafeAddRange(referenceNameDatas);
            this.MakeObjectDirty(true);
        }
        public List<ReferenceNameData> GetNameDatas()
        {
            if (AddedNames.GetCountSafe() > 0)
            {
                _ReferenceNameDatas.SafeAddRange(AddedNames);
                AddedNames.SafeClear();
                this.MakeObjectDirty(true);
            }
            return _ReferenceNameDatas;
        }
    }
}
#endif