#if UNITY_EDITOR
using UnityEditor;
namespace Gametamin.Core
{
    [CustomEditor(typeof(BaseTextComponent), true)]
    public class BaseTextComponentEditor : TextReferenceNameFactoryEditor
    {
        protected override bool _Editable => true;
        protected override bool IsRoot => false;
    }
}
#endif
