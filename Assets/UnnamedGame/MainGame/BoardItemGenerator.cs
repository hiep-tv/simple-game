using System;
using System.Collections.Generic;
using Gametamin.Core;
using UnityEngine;

namespace UnnamedGame
{
    public partial class BoardItemGenerator : MonoBehaviour
    {
        [SerializeField] List<BoardItemData> _boardItemDatas = new();
        Vector2 _position;
        GameObject _boardItemPool;
        public void Construct(Vector2 position, GameObject pool)
        {
            _position = position;
            _boardItemPool = pool;
        }

        public void Generate(Action<BoardItemData, bool> onCreate)
        {
            var newItem = false;
            var boardItemData = GetBoardItemData();
            if (boardItemData == null)
            {
                var boardItemObject = _boardItemPool.GetGameObjectInGroup(transform);
                boardItemData = new BoardItemData(boardItemObject, _position);
                _boardItemDatas.Add(boardItemData);
                newItem = true;
            }
            else
            {
                boardItemData.SetBoardItemPosition(_position);
            }
            onCreate?.Invoke(boardItemData, newItem);
        }
        BoardItemData GetBoardItemData()
        {
            BoardItemData result = default;
            _boardItemDatas.ForBreakable(item =>
            {
                if (item.ItemState == ItemState.InPool)
                {
                    result = item;
                    return true;
                }
                return false;
            });
            return result;
        }
    }
}
