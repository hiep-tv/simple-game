using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gametamin.Core
{
    public class MenuTabBehaviour : MonoBehaviour
    {
        [Tooltip("Percentage width of selected tab in relation to parent")]
        [SerializeField] float _selectedPercent;
        float _normalWidth, _selectedWidth;
        List<RectTransform> _tabs;
        List<RectTransform> _Tabs => _tabs ??= new();
        RectTransform _rect;
        RectTransform _Rect => gameObject.GetComponentSafe(ref _rect);
        public float SelectedPercent
        {
            get => _selectedPercent;
            set => _selectedPercent = value;
        }
        int _lastTab = -1;
        public void Init(float width)
        {
            var tabCount = _Tabs.GetCountSafe();
            var delta = width / tabCount;
            _selectedWidth = delta * _selectedPercent;
            _normalWidth = (width - _selectedWidth) / (tabCount - 1);
        }
        public void AddTab(GameObject tab)
        {
            _Tabs.Add(tab.GetComponentSafe<RectTransform>());
        }
        public void SetTabChanged(int selected)
        {
            if (_lastTab == selected) return;
            var count = _Tabs.GetCountSafe();
            _lastTab = selected;
            _Tabs.For((item, index) =>
            {
                var sizeDelta = item.sizeDelta;
                sizeDelta.x = index == selected ? _selectedWidth : _normalWidth;
                item.sizeDelta = sizeDelta;
            });
            LayoutRebuilder.ForceRebuildLayoutImmediate(_Rect);
        }
    }
}
