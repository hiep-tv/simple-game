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
        public void Generate(Action<BoardItemData> onCreate)
        {
            var boardItemData = GetBoardItemData();
            if (boardItemData == null)
            {
                var boardItemObject = _boardItemPool.GetGameObjectInGroup(transform);
                boardItemData = new BoardItemData(boardItemObject, _position, default);
                _boardItemDatas.Add(boardItemData);
            }
            else
            {
                boardItemData.SetBoardItemData(_position, default);
            }
            onCreate?.Invoke(boardItemData);
        }
        BoardItemData GetBoardItemData()
        {
            BoardItemData result = default;
            _boardItemDatas.ForBreakable(item =>
            {
                if (item.Released)
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
