using UnityEngine;

namespace Gametamin.Core
{
    public interface IClickButton : IInteractable
    {
        bool OnClickButton();
    }
    public interface ITakeButtonClick
    {
        void OnTakeButtonClick();
    }
    public interface ISetButtonSFX
    {
        void OnSetSFX(AudioClip sfx);
    }
}
