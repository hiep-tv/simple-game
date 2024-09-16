#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Gametamin.Core
{
    public class GameObjectReferenceIDAttribute : PropertyAttribute { }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(GameObjectReferenceIDAttribute))]
    public class GameObjectReferenceIDAttributeEditor : BaseGenerateAttributeEditor
    {
        static readonly string Lable = "GameObjectReferenceID";
        protected override string GUILable => Lable;
        protected override string[] Values => GameObjectReferenceID.Values;
        protected override AttributeType AttributeType => AttributeType.GameObjectReferrentID;
    }
#endif
}