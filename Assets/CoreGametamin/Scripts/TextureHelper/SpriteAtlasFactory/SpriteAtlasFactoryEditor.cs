#if UNITY_EDITOR
using UnityEditor;

namespace Gametamin.Core
{
    [CustomEditor(typeof(SpriteAtlasFactory), true)]
    public class SpriteAtlasFactoryEditor : ReferenceNameFactoryEditor<SpriteAtlasFactory, SpriteAtlasFactory, SpriteAtlasDataFactory>
    {
        protected override SpriteAtlasFactory InspectedObject => SpriteAtlasFactory.Instance;

        protected override void GUIAddedName(SpriteAtlasDataFactory item)
        {

        }
    }
}
#endif