using System;
using DG.Tweening;
using Gametamin.Core;
using UnityEngine;

namespace UnnamedGame
{
    [Serializable]
    public class BoardItemData
    {
        public static int MaxTile = 6;
        static float _moveDuration = .25f;
        static float _jumDuration = .45f;
        [SerializeField] GameObject _boardItemGameObject;
        [SerializeField] Vector2 _position;
        [SerializeField] TileData _tileData;
        [SerializeField] int _cellId = -1;
        public ItemState ItemState
        {
            get;
            set;
        }
        Transform _boardItemTransform;
        public TileData TileData => _tileData;
        public GameObject BoardItemObject => _boardItemGameObject;
        public Transform BoardItemTransform => _boardItemTransform;
        public Vector2 Position => _position;
        public Vector2 CurrentPosition => BoardItemTransform.position;
        public int CellId { get => _cellId; set => _cellId = value; }
        public BoardItemData(GameObject boardItemObject, Vector2 position)
        {
            _boardItemGameObject = boardItemObject;
            _boardItemTransform = _boardItemGameObject.transform;
            SetBoardItemPosition(position);
        }
        public void SetBoardItemPosition(Vector2 position)
        {
            _boardItemGameObject.SetLocalPositionSafe(position);
            _boardItemGameObject.SetActiveSafe(true);
            _position = _boardItemGameObject.transform.position;
            var @char = '?';
            SetTileData(new TileData(TileType.UpperCase, (char)@char));
            ItemState = ItemState.InQueue;
        }
        public void SetTileData(TileData tileDatas)
        {
            _tileData = tileDatas;
            _boardItemGameObject.SetTextReference(_tileData.Value);
        }
        public void UpdatePosition(Vector2 position, Action callback = null)
        {
            _position = position;
            MoveBoardItem(callback);
        }
        public void UpdatePositionThenRelease(Vector2 position, Action callback = null, bool jump = false)
        {
            ItemState = ItemState.Moving;
            _position = position;
            if (jump)
            {
                JumpBoardItem(Callback);
            }
            else
            {
                MoveBoardItem(Callback);
            }
            void Callback()
            {
                Release();
                callback?.Invoke();
            }
        }
        public void ResetPosition(Action callback = null)
        {
            MoveBoardItem(_position, _moveDuration, callback);
        }
        void JumpBoardItem(Action callback = null)
        {
            JumpBoardItem(_position, _jumDuration, callback);
        }
        void JumpBoardItem(Vector2 position, float duration, Action callback = null)
        {
            _boardItemTransform.DOJump(position, 1.2f, 1, duration).SetEase(Ease.OutBack)
                .OnComplete(() => callback?.Invoke());
        }
        void MoveBoardItem(Action callback = null)
        {
            MoveBoardItem(_position, _moveDuration, callback);
        }
        void MoveBoardItem(Vector2 position, float duration, Action callback = null)
        {
            _boardItemTransform.DOMove(position, duration).SetEase(Ease.OutBack)
                .OnComplete(() => callback?.Invoke());
        }
        public void Release()
        {
            ItemState = ItemState.InPool;
            _cellId = -1;
            _boardItemGameObject.SetActiveSafe(false);
        }
    }
}
