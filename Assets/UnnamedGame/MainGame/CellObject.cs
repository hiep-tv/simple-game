using Gametamin.Core;
using UnityEngine;

namespace UnnamedGame
{
    public class CellObject : MonoBehaviour
    {
        [SerializeField] GameObject _normalBoard, _highlightBoard;

        public void Highlight(bool active)
        {
            _highlightBoard.SetActiveSafe(active);
        }
    }
}
