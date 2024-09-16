#if UNITY_EDITOR
namespace Gametamin.Core
{
    public class PoolNameReference : ReferenceNameFactory<PoolNameReference>
    {
        protected override string _referenceName => "PoolReferenceID";
        protected override void GenerateInternal(string[] names, string[] values, string group = default)
        {
            ClassGenerator.GenerateEnumClass(ReferenceName, names, values, PathFolder);
        }
    }
}
#endif