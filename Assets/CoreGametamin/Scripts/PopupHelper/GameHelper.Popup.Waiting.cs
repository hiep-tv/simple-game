using System;
using UnityEngine;
using DG.Tweening;

namespace Gametamin.Core
{
    public static partial class PopupHelper
    {
        static float _waitingFadeDuation = 0.15f;
        public static void ShowWaiting(this string message, Action callback = null, bool useIcon = true)
        {
            ShowWaiting(null, message, callback, useIcon);
        }
        public static void ShowWaiting(Action callback = null, bool useIcon = true)
        {
            ShowWaiting(null, TextReferenceID.Waiting.GetTextById(), callback, useIcon);
        }
        public static void ShowWaiting(CreatePopupCallback onCreate, Action callback = null, bool useIcon = true)
        {
            ShowWaiting(onCreate, TextReferenceID.Waiting.GetTextById(), callback, useIcon);
        }
        public static void ShowWaiting(CreatePopupCallback onCreate, string message, Action callback = null, bool useIcon = true)
        {
            PoolConfig.WaitingPopup.ShowOrCreatePopup((@ref, reopen) =>
            {
                @ref.SetTextReference(message, GameObjectReferenceID.ContentPopup, GameObjectReferenceID.Text)
                .SetButtonInteractiveReference(GameObjectReferenceID.ButtonCloseOutside, false);
                @ref.SetActiveGameObjectReference(GameObjectReferenceID.Icon, useIcon);
                @ref.SetActiveGameObjectReference(GameObjectReferenceID.Text, !message.IsNullOrEmptySafe());
                @ref.SetActiveSafe(true);
                @ref.SetAsLastSiblingSafe();
                var canvasGroup = @ref.GetOrAddComponentSafe<CanvasGroup>();
                canvasGroup.DOFade(1f, _waitingFadeDuation)
                .OnComplete(() => callback?.Invoke());
                onCreate?.Invoke(@ref, reopen);
            });
        }
        public static void HideWaiting(Action callback = null)
        {
            var @ref = PoolReferenceID.Waiting.GetPopup();
            @ref.HideWaiting(callback);
        }
        public static void HideWaiting(this GameObjectReference @ref, Action callback = null)
        {
            if (@ref != null)
            {
                var canvasGroup = @ref.GetOrAddComponentSafe<CanvasGroup>();
                canvasGroup.DOFade(0f, _waitingFadeDuation)
                .OnComplete(() =>
                {
                    @ref.SetActiveSafe(false);
                    callback?.Invoke();
                });
            }
            else
            {
                callback?.Invoke();
            }
        }
    }
}
