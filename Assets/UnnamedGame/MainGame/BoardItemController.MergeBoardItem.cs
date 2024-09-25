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
        bool _autoMerge;
        void MergeBoardItems(BoardItemData boardItem)
        {
            var nearestCell = _cellController.GetNearestCell(boardItem.CellId, boardItem.CurrentPosition, out float distance);
            var hasItemOnCell = nearestCell.HasBoardItem;
            var mergeable = hasItemOnCell && CanMerge(nearestCell.BoardItemData, boardItem);
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
            SetLayer(boardItem1.BoardItemObject, _autoMerge ? _movingLayer : _mergeLayer);
            var cell2 = _cellController.GetCell(boardItem2.CellId);
            MergeItem(boardItem1, cell2);
        }
        void MergeItem(BoardItemData boardItem, CellData nearestCell)
        {
            var cellData = _cellController.GetCell(boardItem.CellId);
            cellData.Release();
            var nearestBoardItem = nearestCell.BoardItemData;
            nearestBoardItem.ItemState = ItemState.Merge;
            var tileData = nearestBoardItem.TileData;
            tileData.Value++;
            boardItem.SetTileData(tileData);
            boardItem.UpdatePositionThenRelease(nearestBoardItem.Position
                , () =>
                {
                    nearestBoardItem.SetTileData(tileData);
                    nearestBoardItem.ItemState = ItemState.Ready;
                }, _autoMerge);
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
        bool CanMerge(BoardItemData boardItem, BoardItemData other)
        {
            var sameType = boardItem.TileData.Mergable(other.TileData);
            return sameType;
        }
    }
}
