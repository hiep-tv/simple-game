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
        [SerializeField] GameObject _boardItemGameObject;
        [SerializeField] Vector2 _position;
        [SerializeField] TileData _tileData;
        [SerializeField] bool _released;
        [SerializeField] int _cellId = -1;
        Transform _boardItemTransform;
        BoardItemObject _boardItemObject;
        public TileData TileData => _tileData;
        public GameObject BoardItemObject => _boardItemGameObject;
        public Transform BoardItemTransform => _boardItemTransform;
        public bool Released => _released;
        public Vector2 Position => _position;
        public Vector2 CurrentPosition => BoardItemTransform.position;
        public int CellId { get => _cellId; set => _cellId = value; }
        public bool IsQueue { get; set; }

        public BoardItemData(GameObject boardItemObject, Vector2 position, TileData tileData)
        {
            _boardItemGameObject = boardItemObject;
            _boardItemTransform = _boardItemGameObject.transform;
            SetBoardItemData(position, tileData);
        }
        public void SetBoardItemData(Vector2 position, TileData tileData)
        {
            _released = false;
            _tileData = tileData;
            _boardItemGameObject.SetLocalPositionSafe(position);
            _boardItemGameObject.SetActiveSafe(true);
            _position = _boardItemGameObject.transform.position;
            SetTiles(tileData);
            IsQueue = true;
        }
        void SetTiles(TileData tileDatas)
        {
            _boardItemObject = _boardItemGameObject.GetComponentSafe<BoardItemObject>();
            _boardItemObject.Construct(tileDatas);
        }
        public void UpdatePosition(Vector2 position, Action callback = null)
        {
            _position = position;
            MoveBoardItem(_position, _moveDuration, callback);
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
        public bool CanCombine(BoardItemData other)
        {
            var otherTiles = other.TileData;
            var canCombine = other.TileData.Value == TileData.Value;
            return canCombine;
        }

        public void Release()
        {
            _released = true;
            _cellId = -1;
            _boardItemGameObject.SetActiveSafe(false);
        }
    }
    [Serializable]
    public struct TileData
    {
        public static int DefaultValue = 0;

        [SerializeField] int _index;
        [SerializeField] int _value;
        public TileData(int index, int value)
        {
            _index = index;
            _value = value;
            LockTile = false;
        }
        public int Index { get => _index; set => _index = value; }
        public int Value { get => _value; set => _value = value; }
        public bool LockTile { get; set; }
        public bool IsDefaultValue() => Value == DefaultValue;
        public static bool IsDefaultValue(int value) => value == DefaultValue;
        public override string ToString()
        {
            return $"(Index={Index}, Value={Value})";
        }
    }
}
