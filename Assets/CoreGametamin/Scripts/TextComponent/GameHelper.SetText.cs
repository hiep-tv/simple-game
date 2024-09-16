#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using UnityEngine;
namespace Gametamin.Core
{
    public static partial class TextHelper
    {
        public static void SetTextReference(this GameObject root, object value, string parentId, string textId = GameObjectReferenceID.Text)
        {
            if (!root.IsNullSafe())
            {
                var textObject = root.GetGameObjectReference(parentId, textId);
                textObject.SetTextSafe(value);
            }
        }
        public static GameObjectReference SetTextReferenceLayer3(this GameObjectReference @ref, object value, string parentId, string subParentId, string textId = GameObjectReferenceID.Text)
        {
            if (!@ref.IsNullSafe())
            {
                var textObject = @ref.GetGameObjectReference(parentId, subParentId, textId);
                textObject.SetTextSafe(value);
                return @ref;
            }
            return default;
        }
        public static GameObjectReference SetTextReference(this GameObjectReference @ref, object value, string parentId, string subParentId, string textId = GameObjectReferenceID.Text)
        {
            if (!@ref.IsNullSafe())
            {
                var textObject = @ref.GetGameObjectReference(parentId, subParentId, textId);
                textObject.SetTextSafe(value);
                return @ref;
            }
            return default;
        }
        public static GameObjectReference SetTextReferenceLayer2(this GameObjectReference @ref, object value, string parentId, string textId = GameObjectReferenceID.Text)
        {
            if (!@ref.IsNullSafe())
            {
                var textObject = @ref.GetGameObjectReference(parentId, textId);
                textObject.SetTextSafe(value);
                return @ref;
            }
            return default;
        }
        public static void SetTextReferenceLayer2(this GameObject root, object value, string parentId, string textId = GameObjectReferenceID.Text)
        {
            if (!root.IsNullSafe())
            {
                var textObject = root.GetGameObjectReference(parentId, textId);
                textObject.SetTextSafe(value);
            }
        }
        public static GameObjectReference SetTextReference(this GameObjectReference @ref, object value, string parentId, string textId = GameObjectReferenceID.Text)
        {
            if (!@ref.IsNullSafe())
            {
                var textObject = @ref.GetGameObjectReference(parentId, textId);
                textObject.SetTextSafe(value);
                return @ref;
            }
            return default;
        }
        public static GameObjectReference SetTextReference(this Component component, object value, string textId = GameObjectReferenceID.Text)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.SetTextReference(value, textId);
            }
            return default;
        }
        public static GameObjectReference SetTextReference(this Component component, object value, TextStyle textStyle, string textId = GameObjectReferenceID.Text)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.SetTextReference(value, textId);
            }
            return default;
        }
        public static GameObjectReference SetTextReference(this GameObject root, object value, TextStyle textStyle, string textId = GameObjectReferenceID.Text)
        {
            var @ref = root.GetComponentSafe<GameObjectReference>();
            @ref.SetTextReference(value, textStyle, textId);
            return @ref;
        }
        public static GameObjectReference SetTextReference(this GameObjectReference @ref, object value, TextStyle textStyle, string textId = GameObjectReferenceID.Text)
        {
            var itext = @ref.GetComponentReference<ITextComponent>(textId);
            itext.SetTextSafe(value, textStyle);
            return @ref;
        }
        public static GameObjectReference SetTextReference(this GameObject root, object value, string textId = GameObjectReferenceID.Text)
        {
            var @ref = root.GetComponentSafe<GameObjectReference>();
            @ref.SetTextReference(value, textId);
            return @ref;
        }
        public static GameObjectReference SetTextReference(this GameObjectReference @ref, object value, string textId = GameObjectReferenceID.Text)
        {
            var itext = @ref.GetComponentReference<ITextComponent>(textId);
            itext.SetTextSafe(value);
            return @ref;
        }
        public static void SetTextSafe(this Component textObject, object value)
        {
            var itext = textObject.GetComponentSafe<ITextComponent>();
            itext.SetTextSafe(value);
        }
        public static void SetTextSafe(this GameObject textObject, object value)
        {
            var itext = textObject.GetComponentSafe<ITextComponent>();
            itext.SetTextSafe(value);
        }
        public static void SetTextSafe(this ITextComponent itext, object value)
        {
            if (!itext.IsNullObjectSafe())
            {
                itext.OnSetText(value);
            }
        }
        public static void SetTextSafe(this ITextComponent itext, object value, TextStyle textStyle)
        {
            if (!itext.IsNullObjectSafe())
            {
                itext.OnSetText(value, textStyle);
            }
        }
    }
}
