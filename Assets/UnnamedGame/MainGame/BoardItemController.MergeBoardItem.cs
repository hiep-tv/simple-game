using Gametamin.Core;
using UnityEngine;

namespace UnnamedGame
{
    public partial class BoardItemController : MonoBehaviour
    {
        void MergeBoardItems()
        {
            var column = _boardSize.x;
            var row = _boardSize.y;
            row.For(rowIndex =>
            {
                column.For(columnIndex =>
                {
                    var cellData = _cellController.GetCell(rowIndex, columnIndex);
                    if (cellData.HasBoardItem)
                    {

                    }
                });
            });
        }
    }
}
