using System;
using UnityEngine;

namespace UnnamedGame
{
    public partial class BoardItemController : MonoBehaviour
    {
        enum MergeState
        {
            Non,
            Swap,
            Merge,
        }
        void MergeBoardItems(BoardItemData boardItem)
        {
            var nearestCell = _cellController.GetNearestCell(boardItem.CellId, boardItem.CurrentPosition, out float distance);
            var hasItemOnCell = nearestCell.HasBoardItem;
            var mergeable = hasItemOnCell && CanMerge(nearestCell.BoardItemData.TileData, boardItem.TileData);
            var state = GetMergeState(distance, mergeable);
            if (state == MergeState.Merge)
            {
                SetLayer(boardItem.BoardItemObject, _mergeLayer);
                MergeItem(boardItem, nearestCell);
            }
            else if (state == MergeState.Swap)
            {
                if (hasItemOnCell)
                {
                    SwapItems(boardItem, nearestCell
                        , () => SetLayer(boardItem.BoardItemObject, _normalLayer));
                }
                else
                {
                    MoveBoardItemOnBoard(boardItem, nearestCell
                        , () => SetLayer(boardItem.BoardItemObject, _normalLayer));
                }
            }
            else
            {
                SetLayer(boardItem.BoardItemObject, _normalLayer);
                boardItem.ResetPosition();
            }
        }
        void MergeItem(BoardItemData boardItem1, BoardItemData boardItem2)
        {
            SetLayer(boardItem1.BoardItemObject, _mergeLayer);
            var cell2 = _cellController.GetCell(boardItem2.CellId);
            MergeItem(boardItem1, cell2);
        }
        void MergeItem(BoardItemData boardItem, CellData nearestCell)
        {
            var cellData = _cellController.GetCell(boardItem.CellId);
            cellData.Release();
            var nearestBoardItem = nearestCell.BoardItemData;
            nearestBoardItem.ItemState = ItemState.Merge;
            boardItem.UpdatePositionThenRelease(nearestBoardItem.Position
                , () => nearestBoardItem.ItemState = ItemState.Ready);
            var tileData = nearestBoardItem.TileData;
            tileData.Value++;
            nearestBoardItem.SetTileData(tileData);
            _selectedBoardItem = nearestBoardItem;
        }
        void SwapItems(BoardItemData boardItem, CellData nearestCell, Action callback)
        {
            var cellData = _cellController.GetCell(boardItem.CellId);
            var nearestBoardItem = nearestCell.BoardItemData;
            LinkBoardItemToCell(nearestBoardItem, cellData);
            LinkBoardItemToCell(boardItem, nearestCell);
            var position1 = boardItem.Position;
            var position2 = nearestBoardItem.Position;
            boardItem.UpdatePosition(position2, callback);
            nearestBoardItem.UpdatePosition(position1);
        }
        MergeState GetMergeState(float distance, bool mergeable)
        {
            if (mergeable && distance <= _distanceMergeable)
            {
                return MergeState.Merge;
            }
            if (distance <= _distanceSwappable)
            {
                return MergeState.Swap;
            }
            return MergeState.Non;
        }

        bool CanMerge(TileData tile1, TileData tile2)
        {
            return tile1.Value == tile2.Value;
        }
    }
}
