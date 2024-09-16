using System.Collections.Generic;
using Gametamin.Core;
using UnityEngine;

namespace UnnamedGame
{
    public partial class CellController : MonoBehaviour
    {
        void GenerateCells()
        {
            if (_cellDatas.GetCountSafe() > 0)
            {
                _cellDatas.For(cell => cell.CellObject.SetActiveSafe(false));
                _cellDatas.SafeClear();
            }
            else
            {
                _cellDatas = new();
            }
            Generate(_cellDatas, _cellPool);
        }
        void Generate(List<CellData> cellDatas, GameObject pool)
        {
            var startX = -(_column * _cellSize[0] + (_column - 1) * _space[0]) / 2f + _cellSize[0] / 2f;
            var startY = (_row * _cellSize[1] + (_row - 1) * _space[1]) / 2f - _cellSize[1] / 2f;
            _row.For(rowIndex =>
            {
                var posY = startY - rowIndex * _cellSize[1] - rowIndex * _space[1];
                _column.For(columnIndex =>
                {
                    var posX = startX + columnIndex * _cellSize[0] + columnIndex * _space[0];
                    var itemObject = pool.GetGameObjectInGroup(transform);
                    itemObject.SetLocalPositionSafe(new Vector3(posX, posY));
                    var cellId = _column * rowIndex + columnIndex;
                    var cellData = new CellData(cellId, rowIndex, columnIndex, itemObject, true, false);
                    cellDatas.Add(cellData);
                });
            });
        }
    }
}
