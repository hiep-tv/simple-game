using UnityEngine;
using UnityEngine.UI;

namespace Gametamin.Core
{
    public partial class CommonButton : BaseButton, ICommonUIButton
    {
        Button _button;
        Button _Button
        {
            get
            {
                if (_button == null)
                {
                    _button = gameObject.GetOrAddComponentSafe<Button>();
                    _button.transition = Selectable.Transition.None;
#if UNITY_EDITOR
                    var nav = _button.navigation;
                    nav.mode = Navigation.Mode.None;
                    _button.navigation = nav;
                    _button.hideFlags |= HideFlags.HideInInspector;
#endif
                }
                return _button;
            }
        }
        public override bool Interactable
        {
            get => _Button.interactable;
            set => _Button.interactable = value;
        }
        protected override void Awake()
        {
            base.Awake();
            _Button.onClick.AddListener(OnClick);
        }
    }
}