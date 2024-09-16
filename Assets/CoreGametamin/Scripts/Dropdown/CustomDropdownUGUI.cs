using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Gametamin.Core
{
    public class CustomDropdownUGUI : Dropdown
    {
        Action _onShow, _onHide;
        public Action OnShow { get => _onShow; set => _onShow = value; }
        public Action OnHide { get => _onHide; set => _onHide = value; }
        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            _onShow?.Invoke();
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            _onShow?.Invoke();
        }
        public override void Select()
        {
            _onHide?.Invoke();
            base.Select();
        }
    }
}
