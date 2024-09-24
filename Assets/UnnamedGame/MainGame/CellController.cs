using System.Collections.Generic;
using Gametamin.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnnamedGame
{
    public partial class CellController : MonoBehaviour
    {
        [SerializeField] List<CellData> _cellDatas = new();
        CellData _cellSelected;
        int _row => _boardSize.x;
        int _column => _boardSize.y;
        Vector2Int _boardSize;
        Vector2 _space, _cellSize;
        GameObject _cellPool;
        public void Construct(GameObject cellpool, Vector2 cellSize, Vector2Int boardSize, Vector2 space)
        {
            _boardSize = boardSize;
            _cellSize = cellSize;
            _space = space;
            _cellPool = cellpool;
            GenerateCells();
        }
        public void CheckNearByCell(Vector2 position)
        {
            if (_cellSelected != null)
            {
                var canSelect = NearBy(_cellSelected.Position, position);
                if (canSelect)
                {
                    _cellSelected.Highlight(true);
                    return;
                }
                else
                {
                    _cellSelected.Highlight(false);
                }
            }
            _cellDatas.ForBreakable(cellData =>
            {
                var canSelect = cellData.CanPutBoardItem && NearBy(cellData.Position, position);
                if (canSelect)
                {
                    _cellSelected = cellData;
                    _cellSelected.Highlight(true);
                }
                return canSelect;
            });
        }
        public bool GetNearByEmptyCell(Vector2 position, out CellData selectedCellData)
        {
            CellData result = default;
            var hasCell = false;
            _cellDatas.ForBreakable(cellData =>
            {
                var canPutOnCell = cellData.CanPutBoardItem && NearBy(cellData.Position, position);
                if (canPutOnCell)
                {
                    result = cellData;
                    cellData.Highlight(false);
                    hasCell = true;
                }
                return canPutOnCell;
            });
            selectedCellData = result;
            return hasCell;
        }
        public CellData GetNearestCell(int cellid, Vector2 position, out float distance)
        {
            CellData result = default;
            var minDistance = float.MaxValue;
            _cellDatas.For(cellData =>
            {
                if (cellData.CellId != cellid)
                {
                    var distance = Vector2.Distance(cellData.Position, position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        result = cellData;
                    }
                }
            });
            distance = minDistance;
            return result;
        }
        public bool GetNearestEmtyCell(Vector2 position, out CellData cellData)
        {
            CellData result = default;
            var hasCell = false;
            var minDistance = float.MaxValue;
            _cellDatas.ForReverse((cellData, index) =>
            {
                var distance = Vector2.Distance(cellData.Position, position);
                if (distance < minDistance)
                {
                    var canPutOnCell = cellData.CanPutBoardItem;
                    if (canPutOnCell)
                    {
                        hasCell = true;
                        minDistance = distance;
                        result = cellData;
                    }
                }

            });
            cellData = result;
            return hasCell;
        }
        public bool IsNearByCell(Vector2 position)
        {
            var nearByCell = false;
            _cellDatas.ForBreakable(cellData =>
            {
                nearByCell = NearBy(cellData.Position, position);
                return nearByCell;
            });
            return nearByCell;
        }
        bool NearBy(Vector2 pos1, Vector2 pos2)
        {
            return Vector2.Distance(pos1, pos2) <= 1f;
        }
        public void ReleaseCell(int cellId)
        {
            var cellData = _cellDatas.GetSafe(cellId);
            cellData.Release();
        }
        public CellData GetCell(int cellId)
        {
            return _cellDatas.GetSafe(cellId);
        }
        public CellData GetCell(int row, int colum)
        {
            if (row < 0 || colum < 0 || row >= _row || colum >= _column)
            {
                return default;
            }
            var cellId = _column * row + colum;
            return _cellDatas.GetSafe(cellId);
        }
    }
}
