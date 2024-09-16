#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace Gametamin.Core
{
    public class DropdownStringWindowEditor : AdvancedDropdown
    {
        Action<string, int> _onSelected;
        string _title;
        string[] _values;
        int _emptyIndex = -1;
        static string _emptyValue = "None";
        List<string> _listValues;
        public float ItemWidth { get; private set; }
        public DropdownStringWindowEditor(string title, string[] values, AdvancedDropdownState state, Action<string, int> onSelected) : base(state)
        {
            _onSelected = onSelected;
            _title = title;
            _values = values;
        }
        public DropdownStringWindowEditor(string title, List<string> values, AdvancedDropdownState state, Action<string, int> onSelected) : base(state)
        {
            _onSelected = onSelected;
            _title = title;
            _listValues = values;
        }
        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem(_title);
            if (_listValues.GetCountSafe() > 0)
            {
                _listValues.For(AddChild);
            }
            else
            {
                _values.For(AddChild);
            }
            void AddChild(string name, int index)
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = _emptyValue;
                    _emptyIndex = index;
                }
                var item = new AdvancedDropdownItem(name)
                {
                    id = index
                };
                var width = item.name.GetLabelWidth();
                if (ItemWidth < width)
                {
                    ItemWidth = width;
                }
                root.AddChild(item);
            }
            return root;
        }
        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (item.id == _emptyIndex)
            {
                _onSelected?.Invoke(string.Empty, item.id);
            }
            else
            {
                _onSelected?.Invoke(item.name, item.id);
            }
        }
    }
}
#endif