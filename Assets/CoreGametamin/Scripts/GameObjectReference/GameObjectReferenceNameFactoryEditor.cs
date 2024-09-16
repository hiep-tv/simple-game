#if UNITY_EDITOR
using UnityEditor;
namespace Gametamin.Core
{
    [CustomEditor(typeof(GameObjectReferenceNameFactory), true)]
    public class GameObjectReferenceNameFactoryEditor : ReferenceNameFactoryEditor<GameObjectReferenceNameFactory>
    {
        protected override ReferenceNameFactory<GameObjectReferenceNameFactory> InspectedObject => GameObjectReferenceNameFactory.Instance;
    }
}
#endif