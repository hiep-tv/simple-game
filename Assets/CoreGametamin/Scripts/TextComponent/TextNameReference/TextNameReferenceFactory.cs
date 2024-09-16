#if UNITY_EDITOR
namespace Gametamin.Core
{
    public class TextNameReferenceFactory : ReferenceNameFactory<TextNameReferenceFactory>
    {
        protected override string _referenceName => "TextReferenceID";
    }
}
#endif