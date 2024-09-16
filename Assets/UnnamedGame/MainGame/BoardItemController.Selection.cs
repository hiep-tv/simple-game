using DG.Tweening;
using Gametamin.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnnamedGame
{
    public partial class BoardItemController : MonoBehaviour
    {
        static float _deltaSelectedY = 1.2f;
        bool _canSelecteBoardItem = true;
        public bool CanSelecteBoardItem => _canSelecteBoardItem;
        bool _selectedItemOnBoard;
        BoardItemData _selectedBoardItem;
        void SelectBoardItem(Vector2 position)
        {
            if (CanSelecteBoardItem)
            {
                _selectedBoardItem = GetSelectedBoardItem(position);
                if (_selectedBoardItem != null)
                {
                    _selectedItemOnBoard = SelectedItemOnBoard(position);
                    if (_selectedItemOnBoard)
                    {
                        _cellController.CheckNearByCell(_selectedBoardItem.CurrentPosition);
                    }
                }
            }
        }
        void DragBoardItem(Vector2 position)
        {
            if (CanSelecteBoardItem && _selectedItemOnBoard)
            {
                if (_selectedBoardItem != null)
                {
                    SetBoardItemPosition(position);
                }
            }
        }
        void ReleaseBoardItem()
        {
            if (CanSelecteBoardItem)
            {
                if (_selectedBoardItem != null)
                {
                    if (_selectedItemOnBoard)
                    {
                        if (_cellController.GetNearByCell(_selectedBoardItem.CurrentPosition, out CellData cellData))
                        {
                            PutBoardItemOnBoard(cellData);
                        }
                        else
                        {
                            _selectedBoardItem.ResetPosition();
                        }
                    }
                    else if (_cellController.GetEmtyCell(out CellData cellData))
                    {
                        PutBoardItemOnBoard(cellData);
                    }
                }
            }
        }
        BoardItemData GetSelectedBoardItem(Vector2 position)
        {
            BoardItemData result = default;
            _queueBoardItems.ForBreakable(data =>
            {
                var canSelect = NearBy(data.Position, position);
                if (canSelect)
                {
                    result = data;
                }
                return canSelect;
            });
            result ??= GetSelectedBoardItemOnBoard(position);
            return result;
        }
        BoardItemData GetSelectedBoardItemOnBoard(Vector2 position)
        {
            BoardItemData result = default;
            _onboardBoardItems.ForBreakable(data =>
            {
                var canSelect = NearBy(data.Position, position);
                if (canSelect)
                {
                    result = data;
                }
                return canSelect;
            });
            return result;
        }
        void PutBoardItemOnBoard(CellData cellData)
        {
            _selectedBoardItem.UpdatePosition(cellData.Position);
            if (_selectedBoardItem.IsQueue)
            {
                _selectedBoardItem.IsQueue = false;
                _queueBoardItems.Remove(_selectedBoardItem);
                _onboardBoardItems.Add(_selectedBoardItem);
                if (_queueBoardItems.GetCountSafe() <= 0)
                {
                    GenerateQueueBoardItems();
                }
            }
            else
            {
                var oldCellId = _selectedBoardItem.CellId;
                _cellController.ReleaseCell(oldCellId);
            }
            _selectedBoardItem.CellId = cellData.CellId;
            cellData.SetBoardItemData(_selectedBoardItem);

            //MergeBoardItems(cellData.Row, cellData.Column);
        }
        void SetBoardItemPosition(Vector2 position)
        {
            var boardItemPosition = GetWorldPoint(position);
            _selectedBoardItem.BoardItemObject.SetPositionSafe(boardItemPosition);
            _cellController.CheckNearByCell(_selectedBoardItem.BoardItemTransform.position);
        }
        bool NearBy(Vector2 pos1, Vector2 pos2)
        {
            return Vector2.Distance(pos1, pos2) <= 1f;
        }
        bool SelectedItemOnBoard(Vector2 position)
        {
            return _cellController.IsNearByCell(position);
        }
    }
}
