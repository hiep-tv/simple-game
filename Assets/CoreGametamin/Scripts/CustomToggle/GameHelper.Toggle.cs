#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gametamin.Core
{
    public static partial class ToggleHelper
    {
        public static void SetToggleToggleGroup(this GameObject toggleObject, ToggleGroup group)
        {
            var toggle = toggleObject.GetComponentSafe<CustomToggle>();
            toggle.SetToggleToggleGroup(group);
        }
        public static void SetToggleToggleGroup(this CustomToggle toggle, ToggleGroup group)
        {
            if (!toggle.IsNullSafe())
            {
                toggle.group = group;
            }
        }
        public static CustomToggle SetToggle(this GameObject toggleObject, bool isOn, Action<bool> onChangeValue = null, bool interactableWhenOn = true)
        {
            var toggle = toggleObject.GetComponentSafe<CustomToggle>();
            return toggle.SetToggle(isOn, onChangeValue, interactableWhenOn);
        }
        public static CustomToggle SetToggle(this CustomToggle toggle, bool isOn, Action<bool> onChangeValue = null, bool interactableWhenOn = true)
        {
            if (toggle != null)
            {
                toggle.OnInit(isOn);
                toggle.OnAddListener(value =>
                {
                    //PlaySoundButton(ClickButtonSFX);
                    onChangeValue?.Invoke(value);
                });
            }
            var iinteractable = toggle.GetComponentSafe<IToggleInteractable>();
            if (iinteractable != null)
            {
                iinteractable.InteractableWhenOn = interactableWhenOn;
            }
            return toggle;
        }
        public static CustomToggle SetToggleStateSafe(this GameObject toggleObject, bool isOn)
        {
            var toggle = toggleObject.GetComponentSafe<CustomToggle>();
            return toggle.SetToggleStateSafe(isOn);
        }
        public static CustomToggle SetToggleStateSafe(this CustomToggle toggle, bool isOn)
        {
            if (toggle != null)
            {
                toggle.SetToggleState(isOn);
            }
            return toggle;
        }
        public static CustomToggle SetToggleBlockable(this GameObject toggleObject, bool blocked)
        {
            var toggle = toggleObject.GetComponentSafe<CustomToggle>();
            return toggle.SetToggleBlockable(blocked);
        }
        public static CustomToggle SetToggleBlockable(this CustomToggle toggle, bool blocked)
        {
            if (toggle != null)
            {
                toggle.Blocked = blocked;
            }
            return toggle;
        }
        public static void SetTabChangedSafe(this GameObject tabGroup, int index)
        {
            var tabBehaviour = tabGroup.GetComponentSafe<MenuTabBehaviour>();
            tabBehaviour.SetTabChangedSafe(index);
        }
        public static void SetTabChangedSafe(this MenuTabBehaviour tabBehaviour, int index)
        {
            if (!tabBehaviour.IsNullSafe())
            {
                tabBehaviour.SetTabChanged(index);
            }
        }
    }
}
