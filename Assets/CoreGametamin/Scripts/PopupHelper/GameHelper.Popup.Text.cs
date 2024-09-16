using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PopupHelper
    {
        public static GameObjectReference SetTitleReference(this Component component, string textTitle, string refId = GameObjectReferenceID.Title)
        {
            if (!component.IsNullSafe())
            {
                component.gameObject.SetTitleReference(textTitle, refId);
            }
            return default;
        }
        public static GameObjectReference SetTitleReference(this GameObject target, string textTitle, string refId = GameObjectReferenceID.Title)
        {
            var title = target.GetGameObjectReference(refId);
            title.SetTextReference(textTitle);
            return target.GetComponentSafe<GameObjectReference>();
        }
        public static GameObjectReference SetTitleReference(this GameObjectReference @ref, string textTitle, string refId = GameObjectReferenceID.Title)
        {
            var title = @ref.GetGameObjectReference(refId);
            title.SetTextReference(textTitle);
            return @ref;
        }
        public static GameObjectReference SetMessageReference(this GameObjectReference @ref, string textMessage, string parentId, string refId = GameObjectReferenceID.Message)
        {
            var parent = @ref.GetGameObjectReference(parentId);
            parent.SetMessageReference(textMessage, refId);
            return @ref;
        }
        public static GameObjectReference SetMessageReference(this Component component, string textMessage, string refId = GameObjectReferenceID.Message)
        {
            if (!component.IsNullSafe())
            {
                component.gameObject.SetMessageReference(textMessage, refId);
            }
            return default;
        }
        public static GameObjectReference SetMessageReference(this GameObject target, string textMessage, string refId = GameObjectReferenceID.Message)
        {
            var Message = target.GetGameObjectReference(refId);
            Message.SetTextReference(textMessage);
            return target.GetComponentSafe<GameObjectReference>();
        }
        public static GameObjectReference SetMessageReference(this GameObjectReference @ref, string textMessage, string refId = GameObjectReferenceID.Message)
        {
            var message = @ref.GetGameObjectReference(refId);
            message.SetTextReference(textMessage);
            return @ref;
        }
    }
}
