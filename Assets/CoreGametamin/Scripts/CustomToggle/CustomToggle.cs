using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gametamin.Core
{
    public class CustomToggle : Toggle, IToggleListener
    {
        IPlayAnimation _isPlayAnimation;
        IShowHideAnimation _isetShowHide;
        Action<bool> _onClick;
        bool _started;
        public bool Interactable { get => interactable; set => interactable = value; }
        public bool InteractableWhenOn { get; set; } = true;
        public bool Blocked { get; set; }
        public bool IsOn => isOn;
        public void OnInit(bool enable)
        {
            _isetShowHide = gameObject.GetComponentSafe<IShowHideAnimation>();
            _isPlayAnimation = gameObject.GetComponentSafe<IPlayAnimation>();
            onValueChanged.AddListener(SetToggle);
            SetToggleState(enable);
            _started = true;
        }
        public void SetToggleState(bool enable)
        {
            if (_isetShowHide != null)
            {
                SetAnimation(enable);
            }
            else if (_isPlayAnimation != null)
            {
                PlayAnimation(enable);
            }
            this.isOn = enable;
        }
        void SetAnimation(bool enable)
        {
            if (enable)
            {
                SetInteractable(false);
                _isetShowHide.OnSetShow();
            }
            else
            {
                _isetShowHide.OnSetHide();
            }
        }
        void PlayAnimation(bool enable)
        {
            if (enable)
            {
                SetInteractable(false);
                gameObject.PlayShowAnimation();
            }
            else
            {
                gameObject.PlayHideAnimation();
            }
        }
        void SetToggle(bool isOn)
        {
            if (_started)
            {
                SetInteractable(!isOn);
                if (_isPlayAnimation != null)
                {
                    if (isOn)
                    {
                        var current = interactable;
                        interactable = false;
                        gameObject.PlayShowAnimation(() => interactable = current);
                    }
                    else
                    {
                        var current = interactable;
                        interactable = false;
                        gameObject.PlayHideAnimation(() => interactable = current);
                    }
                }
            }
        }
        void SetInteractable(bool interactable)
        {
            if (!InteractableWhenOn)
            {
                this.interactable = interactable;
            }
        }
        public void OnAddListener(Action<bool> callback = null)
        {
            _onClick += callback;
        }
        public void OnRemoveListener(Action<bool> callback = null)
        {
            _onClick -= callback;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!Blocked)
            {
                base.OnPointerClick(eventData);
                _onClick?.Invoke(isOn);
            }
            else
            {
                //PlayAnimation(!isOn);
                _onClick?.Invoke(!isOn);
            }
        }

        public bool OnClickButton()
        {
            if (interactable)
            {
                isOn = !isOn;
                _onClick?.Invoke(isOn);
                return true;
            }
            return false;
        }
    }
}