using System;
using System.Collections.Generic;
using Gametamin.Core;
using UnityEngine;

namespace UnnamedGame
{
    public partial class BoardItemController : MonoBehaviour
    {
        void FindItemToMerge(List<BoardItemData> items)
        {
            FindItemToMerge(items, boardItem1 =>
            {
                if (FindItemToMerge(items, boardItem1, out BoardItemData boardItem2))
                {
                    boardItem1.ItemState = ItemState.Merge;
                    boardItem2.ItemState = ItemState.Merge;
                    var delay = UnityEngine.Random.Range(0.2f, 0.4f);
                    delay.DelayCall(() =>
                    {
                        MergeItem(boardItem1, boardItem2);
                    });
                }
            });
        }
        void FindItemToMerge(List<BoardItemData> items, Action<BoardItemData> boardItem)
        {
            var total = items.GetCountSafe();
            for (var index = 0; index < total; index++)
            {
                var item = items[index];
                if (item.ItemState == ItemState.Ready)
                {
                    boardItem?.Invoke(item);
                }
            }
        }
        bool FindItemToMerge(List<BoardItemData> items, BoardItemData boardItem1, out BoardItemData boardItem2)
        {
            var total = items.GetCountSafe();
            TileData tileData = boardItem1.TileData;
            for (var index = 0; index < total; index++)
            {
                var item = items[index];
                if (item.CellId != boardItem1.CellId
                    && item.ItemState == ItemState.Ready
                    && item.TileData.Value == tileData.Value)
                {
                    boardItem2 = item;
                    return true;
                }
            }
            boardItem2 = default;
            return false;
        }
    }
}
