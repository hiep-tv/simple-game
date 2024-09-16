using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gametamin.Core
{
    public partial class CommonButton
    {
        GameObject _popup;
        public Action OnClickListener { get => _onListener; set => _onListener = value; }

        Action _onListener, _onPreListener, _onLastListener;
        public void OnAddListener(Action onClick, ClickButtonEventType clickType)
        {
            if (clickType == ClickButtonEventType.OnClick)
            {
                _onListener = onClick; return;
            }
            if (clickType == ClickButtonEventType.PreClick)
            {
                _onPreListener += onClick; return;
            }
            _onLastListener += onClick;
        }
        public void OnAddListener(Action onClick)
        {
            _onListener = onClick;
        }
        public void OnAddPreListener(Action onClick, bool force = false)
        {
            if (force)
            {
                _onPreListener = null;
            }
            _onPreListener += onClick;
        }
        public void OnAddLastListener(Action onClick)
        {
            _onLastListener += onClick;
        }
        public void OnRemoveLastListener(Action onClick)
        {
            _onLastListener -= onClick;
        }
        protected override void ClickButton()
        {
            if (UserInput.Enabled)
            {
                if (_popup.IsNullSafe())
                {
                    ButtonClicked();
                }
                else
                {
                    _popup.PlayHidePopupAnimation(ButtonClicked);
                }
            }
        }
        void ButtonClicked()
        {
            _onPreListener?.Invoke();//Pre click
            _onListener?.Invoke();//On click
            _onLastListener?.Invoke();//Last click
            _onPreListener = null;
            _onLastListener = null;
        }
        public void OnAddPopup(GameObject popup)
        {
            _popup = popup;
        }
        public void OnRemovePreListener(Action onClick)
        {
            _onPreListener -= onClick;
        }
    }
}