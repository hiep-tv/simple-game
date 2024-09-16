using System;

namespace Gametamin.Core
{
    public delegate void CreatePopupCallback(GameObjectReference @ref, bool reopen);
    public static partial class PopupHelper
    {
        public static void ShowConfirmPopup(this string title, string message, Action onClose = null)
        {
            PoolConfig.ConfirmPopup.ShowOrCreatePopup((@ref, reopen) =>
            {
                @ref.SetButtonCloseEvent2(onClose);
                @ref.SetConfirmPopupInfo(title, message);
                @ref.SetMainButtonEvent(onClose)
                .SetTextReference(TextReferenceID.OK.GetTextById())
                .HidePopupOnClick(@ref);
                @ref.PlayShowPopupAnimation(() =>
                {
                    if (!reopen)
                    {
                        UserInput.Enabled = true;
                    }
                });
            });
        }
        public static void ShowConfirmPopup(CreatePopupCallback onCreate, Action onClose = null)
        {
            PoolConfig.ConfirmPopup.ShowOrCreatePopup((@ref, reopen) =>
            {
                @ref.SetButtonCloseEvent2(onClose);
                onCreate?.Invoke(@ref, reopen);
            });
        }
        public static void ShowConfirmPopup2Button(CreatePopupCallback onCreate, Action onClose = null)
        {
            PoolConfig.ConfirmOptionPopup.ShowOrCreatePopup((@ref, reopen) =>
            {
                @ref.SetButtonCloseEvent2(onClose);
                onCreate?.Invoke(@ref, reopen);
            });
        }
        public static GameObjectReference SetConfirmPopupInfo(this GameObjectReference @ref, string textTitle, string textMessage)
        {
            @ref.SetTitleReference(textTitle);
            @ref.SetMessageReference(textMessage);
            return @ref;
        }
    }
}
