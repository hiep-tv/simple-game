#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace Gametamin.Core.Localization
{
    [CustomEditor(typeof(LanguageData), true)]
    public class CustomLanguageDataEditor : CustomSearchArrayElementInspector
    {

    }
}
#endif
