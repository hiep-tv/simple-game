#if UNITY_EDITOR
using UnityEditor;

namespace Gametamin.Core
{
    [CustomEditor(typeof(PopupPoolFactory), true)]
    public class PopupPoolFactoryEditor : ReferenceNameFactoryEditor<PopupPoolFactory, PopupPoolFactory, PoolDataFactory>
    {
        protected override PopupPoolFactory InspectedObject => PopupPoolFactory.Instance;
        protected override void GUIAddedName(PoolDataFactory item)
        {
            //TODO
        }
    }
}
#endif