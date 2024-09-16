using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gametamin.Core
{
    public class FullPageScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] RectTransform _content;
        [SerializeField] RectTransform _viewport;
        [SerializeField] RectTransform[] _cells;
        [SerializeField] float _duration = 0.25f;
        LoopItems _loopItems;
        Tween _tween;
        Vector2 _pointerStartPosition, _contentStartPosition;
        bool _dragging, _dragged, _moveToRight, _canClick = true;
        int _itemIndex;
        float _distance;
        Action _onClick;
        public void Construct(int itemCount, int currentIndex, Action<GameObject, int> onUpdateItem = null)
        {
            _distance = _viewport.GetRectTransformWidth();
            if (_loopItems == null)
            {
                _loopItems = new(itemCount, currentIndex, _viewport, _cells, onUpdateItem);
            }
            else
            {
                _itemIndex = 0;
                _content.SetAnchoredPositionXSafe(0f);
                _loopItems.UpdateData(currentIndex, onUpdateItem);
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            _pointerStartPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_viewport, eventData.position, eventData.pressEventCamera, out _pointerStartPosition);
            _contentStartPosition = _content.anchoredPosition;
            _dragging = true;
            _dragged = false;
            _tween.SafeKillTween();
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (_dragging)
            {
                _canClick = false;
                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_viewport, eventData.position, eventData.pressEventCamera, out Vector2 localCursor))
                    return;
                var pointerDelta = localCursor - _pointerStartPosition;
                _dragged = Mathf.Abs(pointerDelta.x) >= Mathf.Abs(pointerDelta.y);
                if (_dragged)
                {
                    _moveToRight = pointerDelta.x > 0;
                    Vector2 position = _contentStartPosition + pointerDelta;
                    position.y = _content.anchoredPosition.y;
                    _content.anchoredPosition = position;
                    _loopItems.UpdateItems(_moveToRight);
                }
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (_dragged)
            {
                InternalMoveItem(_moveToRight);
                _tween = Move(_distance);
                _dragging = false;
                _canClick = true;
            }
        }
        public void MoveToRight()
        {
            _tween.SafeKillTween();
            InternalMoveItem(false);
        }
        public void MoveToLeft()
        {
            _tween.SafeKillTween();
            InternalMoveItem(true);
        }
        void InternalMoveItem(bool moveToRight)
        {
            _loopItems.UpdateItems(moveToRight);
            if (moveToRight)
            {
                _itemIndex++;
            }
            else
            {
                _itemIndex--;
            }
            _tween = Move(_distance);
        }
        public void AddClickListener(Action onClick = null)
        {
            _onClick = onClick;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_canClick)
            {
                _onClick?.Invoke();
            }
        }

        Tween Move(float positionX)
        {
            return _content.DOAnchorPosX(positionX * _itemIndex, _duration);
        }
        private void OnDestroy()
        {
            _tween.SafeKillTween();
        }
    }
}
