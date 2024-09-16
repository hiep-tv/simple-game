using System;
namespace Gametamin.Core
{
    public interface IToggleListener
    {
        bool IsOn { get; }
        void OnInit(bool isOn);
        void OnAddListener(Action<bool> callback = null);
        void OnRemoveListener(Action<bool> callback = null);
    }
    public interface IToggleInteractable : IInteractable
    {
        bool InteractableWhenOn { get; set; }
    }
}