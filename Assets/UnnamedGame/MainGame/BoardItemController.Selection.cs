using System;
using Gametamin.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnnamedGame
{
    public partial class BoardItemController : MonoBehaviour
    {
        bool _autoStarted;
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
                    SetLayer(_selectedBoardItem.BoardItemObject, _movingLayer);
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
        void ReleaseBoardItem(Vector2 position)
        {
            if (CanSelecteBoardItem)
            {
                if (_selectedBoardItem != null)
                {
                    if (_selectedItemOnBoard)
                    {
                        MergeBoardItems(_selectedBoardItem);
                    }
                    else if (_cellController.GetNearestEmtyCell(position, out CellData cellData))
                    {
                        PutBoardItemOnBoard(_selectedBoardItem, cellData);
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
                var canSelect = data.ItemState == ItemState.Ready && NearBy(data.Position, position);
                if (canSelect)
                {
                    result = data;
                }
                return canSelect;
            });
            return result;
        }
        void PutBoardItemOnBoard(BoardItemData boardItem, CellData cellData)
        {
            var @char = 'A';// UnityEngine.Random.Range(97, 200);
            boardItem.SetTileData(new TileData(TileType.Alphabet, @char));
            MoveBoardItemOnBoard(boardItem, cellData
                , () => SetLayer(boardItem.BoardItemObject, _normalLayer));
            if (!_autoStarted)
            {
                _autoStarted = true;
                2f.DelayCall(() =>
                {
                    1f.DelayCallLoop(() => FindItemToMerge(_onboardBoardItems));
                });
            }
        }
        void MoveBoardItemOnBoard(BoardItemData boardItem, CellData cellData, Action callback = null)
        {
            if (boardItem.ItemState == ItemState.InQueue)
            {
                //boardItem.ItemState = ItemState.Ready;
                ChangeStateToReady(boardItem);
                _queueBoardItems.Remove(boardItem);
                _onboardBoardItems.Add(boardItem);
                if (_queueBoardItems.GetCountSafe() <= 0)
                {
                    GenerateQueueBoardItems();
                }
            }
            else
            {
                var oldCellId = boardItem.CellId;
                _cellController.ReleaseCell(oldCellId);
            }
            LinkBoardItemToCell(boardItem, cellData);
            boardItem.UpdatePosition(cellData.Position, callback);
        }
        void LinkBoardItemToCell(BoardItemData boardItem, CellData cellData)
        {
            boardItem.CellId = cellData.CellId;
            cellData.SetBoardItemData(boardItem);
        }
        void SetBoardItemPosition(Vector2 position)
        {
            _selectedBoardItem.BoardItemObject.SetPositionSafe(position);
            _cellController.CheckNearByCell(_selectedBoardItem.BoardItemTransform.position);
        }
        void ChangeStateToReady(BoardItemData boardItem)
        {
            1f.DelayCall(() =>
            {
                boardItem.ItemState = ItemState.Ready;
            });
        }
        bool NearBy(Vector2 pos1, Vector2 pos2)
        {
            return Vector2.Distance(pos1, pos2) <= 1f;
        }
        bool SelectedItemOnBoard(Vector2 position)
        {
            return _cellController.IsNearByCell(position);
        }
        void SetLayer(GameObject itemObject, int layer)
        {
            var sortingGroup = itemObject.GetComponentSafe<SortingGroup>();
            sortingGroup.sortingOrder = layer;
        }
    }
}
