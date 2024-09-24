using System;
using DG.Tweening;
using Gametamin.Core;
using UnityEngine;

namespace UnnamedGame
{
    public enum ItemState
    {
        Non,
        InPool,
        InQueue,
        Moving,
        Merge,
        Ready,
    }
    [Serializable]
    public class BoardItemData
    {
        public static int MaxTile = 6;
        static float _moveDuration = .25f;
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
            SetTileData(new TileData(TileType.Alphabet, (char)@char));
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
            MoveBoardItem(_position, _moveDuration, callback);
        }
        public void UpdatePositionThenRelease(Vector2 position, Action callback = null)
        {
            ItemState = ItemState.Moving;
            _position = position;
            MoveBoardItem(_position, _moveDuration, () =>
            {
                Release();
                callback?.Invoke();
            });
        }
        public void ResetPosition(Action callback = null)
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
    [Serializable]
    public struct TileData
    {
        public static int DefaultValue = 0;
        public static bool IsDefaultValue(int value) => value == DefaultValue;

        [SerializeField] TileType _type;
        [SerializeField] char _value;
        public char Value { get => _value; set => _value = value; }
        public bool LockTile { get; set; }
        public bool IsDefaultValue() => Value == DefaultValue;

        public TileData(TileType type, char value)
        {
            _value = value;
            _type = type;
            LockTile = false;
        }
        public override string ToString()
        {
            return $"(TileType={_type}, Value={Value})";
        }
    }
    public enum TileType
    {
        Non,
        Numeric,
        Alphabet
    }
}
