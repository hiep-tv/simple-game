using System;
using DG.Tweening;
using Gametamin.Core;
using UnityEngine;

namespace MergeGame
{
    public class ButtonTabAnimation : ScaleAndMoveAnimation
    {
        [SerializeField] GameObject _tabBoard;
        [SerializeField] Vector3 _startBoardPosition, _endBoardPosition;
        [SerializeField] float _boardMoveDuration;
        RectTransform _boardRect;
        RectTransform _BoardRect => _tabBoard.GetComponentSafe(ref _boardRect);
        Tween _current;
        public override void OnSetShow()
        {
            base.OnSetShow();
            KillTween();
            _BoardRect.SetAnchoredPositionYSafe(_endBoardPosition.y);
        }
        public override void OnSetHide()
        {
            base.OnSetHide();
            KillTween();
            _BoardRect.SetAnchoredPositionYSafe(_startBoardPosition.y);
        }
        public override void OnShow(Action callback = null)
        {
            base.OnShow(callback);
            MoveBoard(_endBoardPosition, _boardMoveDuration);
        }
        public override void OnHide(Action callback = null)
        {
            base.OnHide(callback);
            MoveBoard(_startBoardPosition, _boardMoveDuration);
        }
        void MoveBoard(Vector3 position, float duration)
        {
            KillTween();
            _current = _BoardRect.DOAnchorPosY(position.y, duration);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            KillTween();
        }
        private void OnDisable()
        {
            KillTween(true);
        }
        void KillTween(bool complete = false)
        {
            if (_current != null && _current.active)
            {
                _current.Kill(complete);
            }
        }
    }
}
