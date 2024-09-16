using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PopupHelper
    {
        public static GameObjectReference GetPopup(this PoolReferenceID popupId)
        {
            //if (popupId == PoolReferenceID.MainMenu)
            //{
            //    Debug.Log("GetPopup");
            //}
            var popup = PoolHelper.GetGameObject(popupId);
            if (!popup.IsNullSafe())
            {
                return popup.GetComponentSafe<GameObjectReference>();
            }
            return default;
        }
        public static GameObjectReference HidePopupOnClick(this GameObject buttonObject, GameObjectReference rootRef)
        {
            return buttonObject.SetButtonPopupParent(rootRef.gameObject);
        }
        public static GameObjectReference SetButtonPopupParent(this GameObject buttonObject, GameObject popup)
        {
            if (!buttonObject.IsNullSafe())
            {
                var ibutton = buttonObject.GetComponentSafe<ICommonUIButton>();
                if (!ibutton.IsNullObjectSafe())
                {
                    ibutton.OnAddPopup(popup);
                }
            }
            return buttonObject.GetComponentSafe<GameObjectReference>();
        }
        public static GameObjectReference HidePopupOnClick(this GameObjectReference buttonObject, GameObjectReference rootRef)
        {
            if (!buttonObject.IsNullSafe())
            {
                var ibutton = buttonObject.GetComponentSafe<ICommonUIButton>();
                if (!ibutton.IsNullObjectSafe())
                {
                    ibutton.OnAddPopup(rootRef.gameObject);
                }
            }
            return buttonObject.GetComponentSafe<GameObjectReference>();
        }

        public static GameObjectReference SetLeftPopupButton(this GameObjectReference @ref, Action onClick, string textButton, Sprite boardButton = default)
        {
            @ref.SetLeftButtonEvent(onClick)
                .SetTextReference(textButton)
                .SetImageReference(boardButton);
            return @ref;
        }
        public static GameObjectReference SetRightPopupButton(this GameObjectReference @ref, Action onClick, string textButton, Sprite boardButton = default)
        {
            @ref.SetRightButtonEvent(onClick)
                   .SetTextReference(textButton)
                   .SetImageReference(boardButton);
            return @ref;
        }
        public static GameObjectReference SetMainPopupButton(this GameObjectReference @ref, Action onClick, string textButton, Sprite boardButton = default)
        {
            @ref.SetMainButtonEvent(onClick)
                .SetTextReference(textButton)
                .SetImageReference(boardButton);
            return @ref;
        }
    }
}
