#if UNITY_EDITOR
using UnityEditor;
namespace Gametamin.Core
{
    [CustomEditor(typeof(PoolNameReference), true)]
    public class PoolNameReferenceEditor : ReferenceNameFactoryEditor<PoolNameReference>
    {
        protected override ReferenceNameFactory<PoolNameReference> InspectedObject => PoolNameReference.Instance;
    }
}
#endif