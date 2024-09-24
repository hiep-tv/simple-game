using Gametamin.Core;
using UnityEngine;

namespace UnnamedGame
{
    public partial class BoardController : MonoBehaviour
    {
        [SerializeField] Vector2Int _boardSize;
        [SerializeField] Vector2 _cellSpace, _cellSize;
        [SerializeField] Transform _generatePosition;
        [SerializeField] GameObject _boardItemPool;
        [SerializeField] GameObject _cellPool;
        [SerializeField] GameObject _boardBackground;
        private void Start()
        {
            OnStart();
        }
        [NaughtyAttributes.Button]
        void OnStart()
        {
            SetBoardSize();

            var boardItemGenerator = gameObject.GetComponentSafe<BoardItemGenerator>();
            boardItemGenerator.Construct(_generatePosition.localPosition, _boardItemPool);

            var cellController = gameObject.GetComponentSafe<CellController>();
            cellController.Construct(_cellPool, _cellSize, _boardSize, _cellSpace);

            var boardItemController = gameObject.GetComponentSafe<BoardItemController>();
            boardItemController.SetControllers(boardItemGenerator, cellController)
                .SetData(_cellSize);

            UserInput.Enabled = true;
        }
        [NaughtyAttributes.Button]
        void SetBoardSize()
        {
            var renderer = _boardBackground.GetComponentSafe<SpriteRenderer>();
            var width = _boardSize[1] * _cellSize[0] + _boardSize[1] * _cellSpace[0];
            var height = _boardSize[0] * _cellSize[1] + _boardSize[0] * _cellSpace[1];
            renderer.size = new Vector2(width, height);
        }
    }
}
