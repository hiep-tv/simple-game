#if UNITY_EDITOR
using System;
using UnityEditor.IMGUI.Controls;

namespace Gametamin.Core
{
    public class DropdownEnumWindowEditor<T> : AdvancedDropdown where T : struct
    {
        Action<T> _onSelected;
        string _title;
        public DropdownEnumWindowEditor(string title, AdvancedDropdownState state, Action<T> onSelected) : base(state)
        {
            _onSelected = onSelected;
            _title = title;
        }
        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem(_title);
            var names = Enum.GetNames(typeof(T));
            names.For(name =>
            {
                root.AddChild(new AdvancedDropdownItem(name));
            });
            return root;
        }
        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            _onSelected?.Invoke(item.name.ToEnum<T>());
        }
    }
}
#endif