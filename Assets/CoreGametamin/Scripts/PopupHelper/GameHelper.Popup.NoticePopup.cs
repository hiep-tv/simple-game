using DG.Tweening;
using MergeGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gametamin.Core
{
    public static partial class PopupHelper
    {
        static Tween _noticeMessageDelayCall;
        static float _noticeScaleDuation = .25f, _noticeMoveY = 250f, _noticeMoveDuation = 1.5f;
        static float _noticeDelay = .5f, _noticeFadeDuation = 0.15f;
        static float _minWidth = 500f, _maxWidth = 1000f, _minHeight = 120f;
        public static void ShowNoticeMessage(this string message)
        {
            //AudioClipName.Notice_Message.PlaySound();
            message.ShowNoticeMessage(Vector3.zero);
        }
        public static void ShowNoticeMessage(this string message, Vector3 position)
        {
            PoolConfig.NoticePopup.ShowOrCreatePopup((@ref, reopen) =>
            {
                if (!reopen)
                {
                    UserInput.Enabled = true;
                }
                @ref.SetNoticeMessage(message, position);
            });
        }
        static void SetNoticeMessage(this GameObjectReference @ref, string message, Vector3 position)
        {
            var textComponent = @ref.GetComponentReferenceLayer2<TMP_Text>(GameObjectReferenceID.ContentPopup, GameObjectReferenceID.Text);
            textComponent.SetTextSafe(message);
            var size = textComponent.GetPreferredValues(message);
            var textTransfrom = @ref.SetMessagePosition(position, size);
            textTransfrom.DOKill();
            _noticeMessageDelayCall.SafeKillTween();
            var canvasGroup = textTransfrom.ResetNoticMessage();
            textTransfrom.DOScale(1f, _noticeScaleDuation);
            textTransfrom.DOLocalMoveY(textTransfrom.localPosition.y + _noticeMoveY, _noticeMoveDuation)
            .OnComplete(() =>
            {
                _noticeMessageDelayCall = _noticeDelay.DelayCall(() =>
                {
                    canvasGroup.DOFade(0f, _noticeFadeDuation);
                }).OnComplete(() => textTransfrom.ResetNoticMessage());
            });
        }
        static Transform SetMessagePosition(this GameObjectReference @ref, Vector3 position, Vector2 size)
        {
            var parent = @ref.GetComponentReference<RectTransform>(GameObjectReferenceID.ContentPopup);
            var messageObject = parent.GetComponentReference<RectTransform>(GameObjectReferenceID.Message);
            LayoutRebuilder.ForceRebuildLayoutImmediate(messageObject);
            var viewport = parent.rect.size;
            messageObject.sizeDelta = new Vector2(Mathf.Clamp(size.x + 200, _minWidth, _maxWidth), Mathf.Max(size.y + 30f, _minHeight));
            size = messageObject.rect.size;
            var targetLocalPosition = parent.InverseTransformPoint(position);
            var localPosition = targetLocalPosition;
            localPosition.y += size.y / 2f;
            var leftLimit = -viewport.x / 2f + size.x / 2f;
            var bottomLimit = -viewport.y / 2f + size.y / 2f;
            localPosition.x = Mathf.Clamp(localPosition.x, leftLimit, -leftLimit);
            localPosition.y = Mathf.Clamp(localPosition.y, bottomLimit, -bottomLimit);
            localPosition.z = messageObject.localPosition.z;
            messageObject.localPosition = localPosition;
            return messageObject;
        }
        static CanvasGroup ResetNoticMessage(this Transform target)
        {
            var canvasGroup = target.GetOrAddComponentSafe<CanvasGroup>();
            canvasGroup.alpha = 1f;
            target.localScale = Vector3.zero;
            return canvasGroup;
        }
    }
}
