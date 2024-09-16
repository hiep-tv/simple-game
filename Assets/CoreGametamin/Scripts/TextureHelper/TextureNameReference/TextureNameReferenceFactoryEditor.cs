#if UNITY_EDITOR
using UnityEditor;
namespace Gametamin.Core
{
    [CustomEditor(typeof(TextureNameReferenceFactory), true)]
    public class TextureNameReferenceFactoryEditor : ReferenceNameFactoryEditor<TextureNameReferenceFactory>
    {
        protected override ReferenceNameFactory<TextureNameReferenceFactory> InspectedObject => TextureNameReferenceFactory.Instance;
    }
}
#endif