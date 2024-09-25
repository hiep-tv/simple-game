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

        float _distanceMergeable;
        float _distanceSwappable;

        int _mergeLayer = 0;
        int _normalLayer = 1;
        int _movingLayer = 2;

        public BoardItemController SetControllers(BoardItemGenerator boardItemGenerator, CellController cellController)
        {
            _mainCamera = Camera.main;
            _boardItemGenerator = boardItemGenerator;
            _cellController = cellController;
            RegisterInputEvent();
            GenerateQueueBoardItems();
            CheckAutoSpawn();
            return this;
        }
        public BoardItemController SetData(Vector2 cellSize)
        {
            var distance = Mathf.Sqrt(2 * Mathf.Pow(cellSize.x / 2f, 2));
            _distanceMergeable = distance + distance / 3f;
            _distanceSwappable = distance + distance / 6f;
            return this;
        }
        void GenerateQueueBoardItems()
        {
            _boardItemGenerator.Generate((data, newItem) =>
            {
                _queueBoardItems.Add(data);
                if (newItem)
                {
                    _onboardBoardItems.Add(data);
                }
            });
        }
    }
}
