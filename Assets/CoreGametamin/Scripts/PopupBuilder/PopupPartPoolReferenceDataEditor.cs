#if UNITY_EDITOR
using UnityEditor;

namespace Gametamin.Core
{
    [CustomPropertyDrawer(typeof(PopupPartPoolReferenceData), true)]
    public class PopupPartPoolReferenceDataEditor : GameObjectReferenceDataEditor
    {
        protected override bool ShowIgnoreProperty => false;
    }
}
#endif
