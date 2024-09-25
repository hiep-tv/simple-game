using DG.Tweening;
using Gametamin.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnnamedGame
{
    public partial class BoardItemController : MonoBehaviour
    {
        void CheckAutoSpawn()
        {
            var total = 1000;
            var position = new Vector2(0, -8);
            Tween _delayCall = null;
            _delayCall = .3f.DelayCallLoop(() =>
            {
                if (_cellController.GetNearestEmtyCell(position, out CellData cellData))
                {
                    var boardItem = GetSelectedBoardItem(position);
                    PutBoardItemOnBoard(boardItem, cellData);
                    total--;
                    if (total == 0)
                    {
                        _delayCall.SafeKillTween();
                    }
                }
            });
        }
    }
}
