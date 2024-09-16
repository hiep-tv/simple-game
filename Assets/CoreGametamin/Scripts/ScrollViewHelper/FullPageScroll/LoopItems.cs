using System;
using UnityEngine;

namespace Gametamin.Core
{
    [Serializable]
    public class LoopItems
    {
        [SerializeField] RectTransform _viewport;
        [SerializeField] RectTransform[] _cells;
        [SerializeField] Bounds _recyclableViewBounds = new();
        [SerializeField] int _itemCount;
        [SerializeField] int _poolCount, currentItemCount, leftMostCellIndex, rightMostCellIndex;
        [SerializeField] bool _recycling;
        Action<GameObject, int> _onUpdateItem;
        public LoopItems(int itemCount, int currentIndex, RectTransform viewport, RectTransform[] cells, Action<GameObject, int> onUpdateItem = null)
        {
            _viewport = viewport;
            _cells = cells;
            _itemCount = itemCount;
            _poolCount = _cells.GetCountSafe();
            UpdateData(currentIndex, onUpdateItem);
        }
        public void UpdateData(int currentIndex, Action<GameObject, int> onUpdateItem = null)
        {
            _onUpdateItem = onUpdateItem;
            currentItemCount = currentIndex + _poolCount;
            leftMostCellIndex = 0;
            rightMostCellIndex = _poolCount - 1;
            if (currentItemCount >= _itemCount)
            {
                currentItemCount -= _itemCount;
            }
            var _distance = _viewport.GetRectTransformWidth();
            _cells.For((item, index) =>
            {
                item.SetAnchoredPositionXSafe(index * _distance);
                var indexData = index + currentIndex;
                if (indexData >= _itemCount)
                {
                    indexData -= _itemCount;
                }
#if UNITY_EDITOR
                item.gameObject.name = $"cell_{indexData}";
#endif
                _onUpdateItem?.Invoke(item.gameObject, indexData);
            });
        }
        public void UpdateItems(bool rightToLeft)
        {
            if (_recycling) return;
            SetRecyclingBounds();
            if (rightToLeft)
            {
                RecycleRightToleft();
            }
            else
            {
                RecycleLeftToRight();
            }
        }
        void RecycleLeftToRight()
        {
            _recycling = true;
            int n = 0;
            //Recycle until cell at left is avaiable and current item count smaller than datasource
            while (_cells[leftMostCellIndex].MaxX() < _recyclableViewBounds.min.x)
            {
                //Move Left most cell to right
                var rightMostCell = _cells[rightMostCellIndex];
                float posX = rightMostCell.anchoredPosition.x + rightMostCell.sizeDelta.x;
                var leftMostCell = _cells[leftMostCellIndex];
                leftMostCell.anchoredPosition = new Vector2(posX, leftMostCell.anchoredPosition.y);
#if UNITY_EDITOR
                leftMostCell.gameObject.name = $"cell_{currentItemCount}";
#endif
                _onUpdateItem?.Invoke(leftMostCell.gameObject, currentItemCount);
                //set new indices
                rightMostCellIndex = leftMostCellIndex;
                leftMostCellIndex = (leftMostCellIndex + 1) % _poolCount;
                currentItemCount++;
                if (currentItemCount >= _itemCount)
                {
                    currentItemCount = 0;
                }
                n++;
            }
            _recycling = false;
        }
        void RecycleRightToleft()
        {
            _recycling = true;
            int n = 0;
            //to determine if content size needs to be updated
            //Recycle until cell at Right end is avaiable and current item count is greater than cellpool size
            while (_cells[rightMostCellIndex].MinX() > _recyclableViewBounds.max.x)
            {
                //Move Right most cell to left
                var leftMostCell = _cells[leftMostCellIndex];
                float posX = leftMostCell.anchoredPosition.x - leftMostCell.sizeDelta.x;
                var rightMostCell = _cells[rightMostCellIndex];
                rightMostCell.anchoredPosition = new Vector2(posX, rightMostCell.anchoredPosition.y);
                currentItemCount--;
                if (currentItemCount < 0)
                {
                    currentItemCount = _itemCount - 1;
                }
                //Cell for row at
                var indexCell = currentItemCount - _poolCount;
                if (indexCell < 0)
                {
                    indexCell += _itemCount;
                }
                _onUpdateItem?.Invoke(rightMostCell.gameObject, indexCell);
#if UNITY_EDITOR
                rightMostCell.gameObject.name = $"cell_{indexCell}";
#endif
                //set new indices
                leftMostCellIndex = rightMostCellIndex;
                rightMostCellIndex = (rightMostCellIndex - 1 + _poolCount) % _poolCount;
                n++;
            }
            _recycling = false;
        }
        float RecyclingThreshold = .2f;
        private readonly Vector3[] _corners = new Vector3[4];
        void SetRecyclingBounds()
        {
            _viewport.GetWorldCorners(_corners);
            float threshHold = RecyclingThreshold * (_corners[2].x - _corners[0].x);
            _recyclableViewBounds.min = new Vector3(_corners[0].x - threshHold, _corners[0].y);
            _recyclableViewBounds.max = new Vector3(_corners[2].x + threshHold, _corners[2].y);
        }
    }
    static class UIExtension
    {
        public static Vector3[] GetCorners(this RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return corners;
        }
        public static float MaxY(this RectTransform rectTransform)
        {
            return rectTransform.GetCorners()[1].y;
        }
        public static float MinY(this RectTransform rectTransform)
        {
            return rectTransform.GetCorners()[0].y;
        }
        public static float MaxX(this RectTransform rectTransform)
        {
            return rectTransform.GetCorners()[2].x;
        }
        public static float MinX(this RectTransform rectTransform)
        {
            return rectTransform.GetCorners()[0].x;
        }
    }
}