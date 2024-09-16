#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class ButtonHelper
    {
        public static void SetButtonAndChildInteractive(this GameObject buttonObject, bool interactable)
        {
            var childButton = buttonObject.GetGameObjectReference(GameObjectReferenceID.Button);
            buttonObject.SetButtonInteractive(interactable);
            childButton.SetButtonInteractive(interactable);
        }
        public static void SetButtonInteractive(this GameObject buttonObject, bool interactable)
        {
            var isetButton = buttonObject.GetComponentSafe<IInteractable>();
            if (!isetButton.IsNullObjectSafe())
            {
                isetButton.Interactable = interactable;
            }
        }
        public static void SetBlockOtherButtons(this GameObject buttonObject)
        {
            var isetButton = buttonObject.GetComponentSafe<IBlockButton>();
            if (!isetButton.IsNullObjectSafe())
            {
                isetButton.OnBlockOthers();
            }
        }
        public static void SetUnblockOtherButtons(this GameObject buttonObject)
        {
            var isetButton = buttonObject.GetComponentSafe<IBlockButton>();
            if (!isetButton.IsNullObjectSafe())
            {
                isetButton.OnUnblockOthers();
            }
        }
        public static GameObjectReference SetButtonInteractiveReference(this GameObjectReference @ref, string buttonId, bool interactable)
        {
            var ibutton = @ref.GetComponentReference<IInteractable>(buttonId);
            ibutton.SetButtonInteractiveSafe(interactable);
            return @ref;
        }
        public static GameObjectReference SetButtonInteractive(this GameObjectReference @ref, bool interactable)
        {
            var ibutton = @ref.GetComponentSafe<IInteractable>();
            ibutton.SetButtonInteractiveSafe(interactable);
            return @ref;
        }
        public static IInteractable SetButtonInteractiveSafe(this IInteractable ibutton, bool interactable)
        {
            if (!ibutton.IsNullObjectSafe())
            {
                ibutton.Interactable = interactable;
            }
            return ibutton;
        }

        public static void SetButtonSFXReference(this GameObjectReference iref, AudioClip clip, string id = GameObjectReferenceID.Button)
        {
            var ibutton = iref.GetComponentReference<ISetButtonSFX>(id);
            ibutton.SetButtonSFXSafe(clip);
        }
        public static GameObjectReference SetButtonSFXSafe(this GameObjectReference irefButton, AudioClip clip)
        {
            var ibutton = irefButton.GetComponentSafe<ISetButtonSFX>();
            ibutton.SetButtonSFXSafe(clip);
            return irefButton;
        }
        public static void SetButtonSFXSafe(this ISetButtonSFX ibutton, AudioClip clip)
        {
            if (!ibutton.IsNullObjectSafe())
            {
                ibutton.OnSetSFX(clip);
            }
        }
        public static void SetButtonScaleAnimationTarget(this GameObject button, GameObject target)
        {
            if (!target.IsNullSafe())
            {
                var animation = button.GetComponentSafe<ScaleButton>();
                if (!animation.IsNullSafe())
                {
                    animation.SetTarget(target);
                }
            }
        }
    }
}
