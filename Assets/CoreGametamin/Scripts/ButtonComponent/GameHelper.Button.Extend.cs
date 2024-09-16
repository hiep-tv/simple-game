#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class ButtonHelper
    {
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetButtonEventReferenceLayer2(this GameObject root, string parentId, Action onClick = null, string referenceID = GameObjectReferenceID.Button)
        {
            var parent = root.GetGameObjectReference(parentId);
            return parent.SetButtonEventReference(onClick, referenceID);
        }
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetButtonEventReferenceLayer3(this GameObject root, string parentId, string subParentId, Action onClick = null, string referenceID = GameObjectReferenceID.Button)
        {
            var parent = root.GetGameObjectReference(parentId);
            var subParent = root.GetGameObjectReference(subParentId);
            return subParent.SetButtonEventReference(onClick, referenceID);
        }

        public static GameObjectReference SetBlockOtherButtonSafe(this GameObjectReference refButton)
        {
            return refButton.GetComponentSafe<IBlockButton>().SetBlockOtherButtonSafe();
        }
        public static GameObjectReference SetBlockOtherButtonSafe(this IBlockButton ibutton)
        {
            if (!ibutton.IsNullObjectSafe())
            {
                ibutton.OnBlockOthers();
            }
            return ibutton.GetComponentSafe<GameObjectReference>();
        }
        public static void DelayButtonEvent(this GameObject buttonObject, Action<Action> onClick = null)
        {
            var ibutton = buttonObject.GetComponentSafe<ICommonUIButton>();
            var action = ibutton.OnClickListener;
            ibutton.OnAddListener(() =>
            {
                onClick?.Invoke(action);
                ibutton.OnAddListener(action);
            });
        }
        public static void SetPreClickButtonClosePopup(this GameObjectReference @ref, Action callback = null)
        {
            var buttonClose = @ref.GetGameObjectReference(GameObjectReferenceID.ButtonClose);
            buttonClose.SetButtonPreClickEvent(OnClosePopup);
            buttonClose = @ref.GetGameObjectReference(GameObjectReferenceID.ButtonCloseOutside);
            buttonClose.SetButtonPreClickEvent(OnClosePopup);
            void OnClosePopup()
            {
                var button = @ref.GetComponentReference<ICommonButton>(GameObjectReferenceID.ButtonClose);
                button.OnRemovePreListener(OnClosePopup);
                button = @ref.GetComponentReference<ICommonButton>(GameObjectReferenceID.ButtonCloseOutside);
                button.OnRemovePreListener(OnClosePopup);
                callback?.Invoke();
            }
        }
        public static void DelayedButtonClosePopup(this GameObjectReference @ref, Action<Action> onClick = null)
        {
            var buttonClose = @ref.GetGameObjectReference(GameObjectReferenceID.ButtonClose);
            buttonClose.DelayButtonEvent(onClick);
            var buttonCloseOutSide = @ref.GetGameObjectReference(GameObjectReferenceID.ButtonCloseOutside);
            buttonCloseOutSide.DelayButtonEvent(onClick);
        }
        public static GameObjectReference TriggerButtonEventReference(this GameObjectReference @ref, string refId = GameObjectReferenceID.Button)
        {
            var ibutton = @ref.GetComponentReference<IClickButton>(refId);
            ibutton?.OnClickButton();
            return @ref;
        }
    }
}
