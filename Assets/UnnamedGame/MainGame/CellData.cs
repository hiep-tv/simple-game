using System;
using Gametamin.Core;
using UnityEngine;

namespace UnnamedGame
{
    [Serializable]
    public class CellData
    {
        [SerializeField] GameObject _cellObject;
        [SerializeField] int _cellId;
        [SerializeField] int _row;
        [SerializeField] int _column;
        [SerializeField] bool _unlocked;
        [SerializeField] bool _hasBoardItem;
        CellObject _cell;
        BoardItemData _boardItemData;
        public GameObject CellObject => _cellObject;
        public Vector2 Position => _cellObject.transform.position;
        public int CellId => _cellId;
        public int Row => _row;
        public int Column => _column;
        public bool HasBoardItem { get => _hasBoardItem; set => _hasBoardItem = value; }
        public bool Unlocked { get => _unlocked; set => _unlocked = value; }
        public bool CanPutBoardItem => Unlocked && !HasBoardItem;
        public BoardItemData BoardItemData => _boardItemData;
        public CellData(int cellId, int row, int column, GameObject cellObject, bool unlocked, bool hasBoardItem)
        {
            _cellId = cellId;
            _row = row;
            _column = column;
            _cellObject = cellObject;
            _unlocked = unlocked;
            _hasBoardItem = hasBoardItem;
            _cell = cellObject.GetComponentSafe<CellObject>();
        }
        public void SetBoardItemData(BoardItemData boardItemData)
        {
            _boardItemData = boardItemData;
            _hasBoardItem = true;
        }
        public void Highlight(bool active)
        {
            _cell.Highlight(active);
        }
        public void Release()
        {
            _hasBoardItem = false;
            _boardItemData = default;
        }
    }
}