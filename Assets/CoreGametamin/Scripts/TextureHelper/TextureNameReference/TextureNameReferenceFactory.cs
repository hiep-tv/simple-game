#if UNITY_EDITOR
namespace Gametamin.Core
{
    public class TextureNameReferenceFactory : ReferenceNameFactory<TextureNameReferenceFactory>
    {
        protected override string _referenceName => "TextureNameReferenceID";
    }
}
#endif