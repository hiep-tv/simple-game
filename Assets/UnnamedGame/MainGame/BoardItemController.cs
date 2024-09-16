using System.Collections.Generic;
using UnityEngine;

namespace UnnamedGame
{
    public partial class BoardItemController : MonoBehaviour
    {
        BoardItemGenerator _boardItemGenerator;
        CellController _cellController;
        [SerializeField] List<BoardItemData> _onboardBoardItems = new();
        [SerializeField] List<BoardItemData> _queueBoardItems = new();
        Camera _mainCamera;
        Vector2Int _boardSize;
        public void Construct(BoardItemGenerator boardItemGenerator, CellController cellController, Vector2Int boardSize)
        {
            _boardSize = boardSize;
            _mainCamera = Camera.main;
            _boardItemGenerator = boardItemGenerator;
            _cellController = cellController;
            RegisterInputEvent();
            GenerateQueueBoardItems();
        }
        void GenerateQueueBoardItems()
        {
            _boardItemGenerator.Generate(data =>
            {
                _queueBoardItems.Add(data);
            });
        }
    }
}
