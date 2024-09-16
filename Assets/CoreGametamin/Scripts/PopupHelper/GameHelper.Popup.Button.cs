using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PopupHelper
    {
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetButtonCloseEvent(this GameObjectReference rootRef, Action onClick = null, bool canClickOutside = true)
        {
            var clickSFX = ClosePopupSFX;
            if (canClickOutside)
            {
                rootRef.SetButtonEventReference(onClick, GameObjectReferenceID.ButtonCloseOutside);
                rootRef.SetButtonSFXReference(clickSFX, GameObjectReferenceID.ButtonCloseOutside);
            }
            rootRef.SetButtonInteractiveReference(GameObjectReferenceID.ButtonCloseOutside, canClickOutside);
            rootRef.SetButtonSFXReference(clickSFX, GameObjectReferenceID.ButtonClose);
            return rootRef.SetButtonEventReference(onClick, GameObjectReferenceID.ButtonClose);
        }
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetButtonCloseEvent2(this GameObjectReference rootRef,
                                                                    Action onClick = null,
                                                                    bool canClickOutside = true,
                                                                    bool canHidePopup = true,
                                                                    AudioClip clickSFX=null)
        {
            if(clickSFX == null)
            {
                clickSFX = ClosePopupSFX;
            }

            var outsideButton = rootRef.GetGameObjectReference(GameObjectReferenceID.ButtonCloseOutside);
            if (canClickOutside)
            {
                rootRef.SetButtonSFXReference(clickSFX, GameObjectReferenceID.ButtonCloseOutside);
                outsideButton.SetButtonEventSafe(onClick);
                if (canHidePopup)
                {
                    outsideButton.HidePopupOnClick(rootRef);
                }
            }
            outsideButton.SetButtonInteractive(canClickOutside);
            rootRef.SetButtonSFXReference(clickSFX, GameObjectReferenceID.ButtonClose);
            var ibutton = rootRef.SetButtonEventReference(onClick, GameObjectReferenceID.ButtonClose);
            if (canHidePopup)
            {
                ibutton.HidePopupOnClick(rootRef);
            }
            return ibutton;
        }
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetButtonCloseEvent(this Component component, Action onClick = null)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.SetButtonCloseEvent(onClick);
            }
            return default;
        }
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetButtonCloseEvent(this GameObject root, Action onClick = null)
        {
            return root.SetButtonEventReference(onClick, GameObjectReferenceID.ButtonClose)
                .SetButtonSFXSafe(ClosePopupSFX);
        }

        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetMainButtonEvent(this GameObjectReference rootRef, Action onClick = null)
        {
            //rootRef.AddKeyListener(GameObjectReferenceID.MainButton, KeyCode.Return);
            return rootRef.SetButtonEventReference(onClick, GameObjectReferenceID.MainButton)
                .SetButtonSFXSafe(MainButtonSFX);
        }
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetMainButtonEvent(this Component component, Action onClick = null)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.SetMainButtonEvent(onClick);
            }
            return default;
        }
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetMainButtonEvent(this GameObject root, Action onClick = null)
        {
            //rootRef.AddKeyListener(GameObjectReferenceID.MainButton, KeyCode.Return);
            return root.SetButtonEventReference(onClick, GameObjectReferenceID.MainButton)
                .SetButtonSFXSafe(MainButtonSFX);
        }

        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetLeftButtonEvent(this GameObjectReference rootRef, Action onClick = null)
        {
            return rootRef.SetButtonEventReference(onClick, GameObjectReferenceID.LeftButton)
                .SetButtonSFXSafe(LeftButtonSFX);
        }
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetLeftButtonEvent(this Component component, Action onClick = null)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.SetLeftButtonEvent(onClick);
            }
            return default;
        }
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetLeftButtonEvent(this GameObject root, Action onClick = null)
        {
            return root.SetButtonEventReference(onClick, GameObjectReferenceID.LeftButton)
                .SetButtonSFXSafe(LeftButtonSFX);
        }

        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetRightButtonEvent(this GameObjectReference iref, Action onClick = null)
        {
            return iref.SetButtonEventReference(onClick, GameObjectReferenceID.RightButton)
                .SetButtonSFXSafe(RightButtonSFX);
        }
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetRightButtonEvent(this Component component, Action onClick = null)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.SetRightButtonEvent(onClick);
            }
            return default;
        }
        /// <summary>
        /// return GameObjectReference of button
        /// </summary>
        public static GameObjectReference SetRightButtonEvent(this GameObject root, Action onClick = null)
        {
            return root.SetButtonEventReference(onClick, GameObjectReferenceID.RightButton)
                .SetButtonSFXSafe(RightButtonSFX);
        }
        public static GameObjectReference SetActiveButtonClose(this GameObjectReference rootRef, bool active)
        {
            var outsideButton = rootRef.GetGameObjectReference(GameObjectReferenceID.ButtonCloseOutside);
            if (!outsideButton.IsNullSafe())
            {
                outsideButton.SetButtonInteractive(active);
            }
            rootRef.SetActiveGameObjectReference(GameObjectReferenceID.ButtonClose, active);
            return rootRef;
        }
    }
}
