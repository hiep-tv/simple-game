#if UNITY_EDITOR

using System.Collections.Generic;

namespace Gametamin.Core
{
    public abstract partial class ReferenceNameFactory<T>
    {
        protected List<ReferenceNameData> _referenceNameDatas = new();
        public void Generate(string[] names, string[] values, string group = default)
        {
            GenerateInternal(names, values, group);
        }
        public override void Generate()
        {
            LoadFactories();
            _referenceNameDatas.SafeClear();
            Factories.For(item =>
            {
                var datas = item.GetNameDatas();
                GetUniqueName(datas);
            });
            GenerateInternal();
            Save();
        }
        protected List<ReferenceNameData> GetUniques()
        {
            LoadFactories();
            _referenceNameDatas.SafeClear();
            Factories.For(item =>
            {
                var datas = item.GetNameDatas();
                GetUniqueName(datas);
            });
            return _referenceNameDatas;
        }
        protected void GetUniqueName(List<ReferenceNameData> names)
        {
            names.For(item =>
            {
                var exits = false;
                _referenceNameDatas.ForBreakable((item2, index) =>
                {
                    exits = item2.Name.EqualsSafe(item.Name);
                    return exits;
                });
                if (!exits)
                {
                    _referenceNameDatas.Add(item);
                }
            });
        }
        protected virtual void GenerateInternal()
        {
            var count = _referenceNameDatas.GetCountSafe() + 1;
            var names = new string[count];
            var values = new string[count];
            names[0] = "Empty";
            values[0] = string.Empty;
            _referenceNameDatas.For((item, index) =>
            {
                names[index + 1] = item.Name;
                values[index + 1] = item.Value;
            });
            GenerateInternal(names, values);
        }
        protected virtual void GenerateInternal(string[] names, string[] values, string group = default)
        {
            ClassGenerator.GenerateStaticClass(ReferenceName, names, values, PathFolder, group);
            ClassGenerator.GenerateStaticClassValues(ReferenceName, values, PathFolder, group);
        }
    }
}
#endif