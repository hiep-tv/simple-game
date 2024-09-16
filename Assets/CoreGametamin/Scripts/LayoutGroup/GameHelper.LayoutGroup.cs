using System;
using UnityEngine;

namespace Gametamin.Core
{
    public struct LayoutConfig
    {
        public string poolId;
        public string parentId;
        public string poolLayoutId;
        public int itemCount;
        int _itemPerLayout;
        public bool canAdjust;
        public int itemPerLayout
        {
            readonly get
            {
                if (canAdjust && _itemPerLayout > 2)
                {
                    if (itemCount > _itemPerLayout && itemCount - _itemPerLayout < 2)
                    {
                        return _itemPerLayout - 1;
                    }
                }
                return _itemPerLayout;
            }
            set => _itemPerLayout = value;
        }
        public float itemHeight;//=0 will ignore update parent size
        float _itemSpace;
        public float itemSpace
        {
            get => itemCount <= itemPerLayout ? 0 : _itemSpace;
            set => _itemSpace = value;
        }
        public LayoutConfig(string poolId, string parentId, string poolLayoutId, int itemCount, int itemPerLayout, float itemHeight, float itemSpace, bool canAdjust = true)
        {
            this.poolId = poolId;
            this.parentId = parentId;
            this.poolLayoutId = poolLayoutId;
            this.itemCount = itemCount;
            _itemPerLayout = itemPerLayout;
            this.itemHeight = itemHeight;
            _itemSpace = itemSpace;
            this.canAdjust = canAdjust;
        }
    }
    public static partial class LayoutGroupHelper
    {
        public static void CreateItemsInGroup(this GameObjectReference @ref, GameObject setItem, LayoutConfig layoutConfig, Action<GameObject, int> onCreate = null)
        {
            var itemPool = @ref.GetPoolReference(layoutConfig.poolId);
            @ref.CreateItemsInGroup(setItem, layoutConfig, itemPool, onCreate);
        }
        public static void CreateItemsInGroup(this GameObjectReference @ref, GameObject setItem, LayoutConfig layoutConfig, Transform layoutParent, Action<GameObject, int> onCreate = null)
        {
            var itemPool = @ref.GetPoolReference(layoutConfig.poolId);
            @ref.CreateItemsInGroup(setItem, layoutConfig, itemPool, layoutParent, onCreate);
        }
        public static void CreateItemsInGroup(this GameObjectReference @ref, GameObject setItem, LayoutConfig layoutConfig, GameObject itemPool, Action<GameObject, int> onCreate = null)
        {
            var layoutParent = setItem.GetTransformReference(layoutConfig.parentId);
            @ref.CreateItemsInGroup(setItem, layoutConfig, itemPool, layoutParent, onCreate);
        }
        public static void CreateItemsInGroup(this GameObjectReference @ref, GameObject setItem, LayoutConfig layoutConfig, GameObject itemPool, Transform layoutParent, Action<GameObject, int> onCreate = null)
        {
            var layoutPool = @ref.GetPoolReference(layoutConfig.poolLayoutId);
            var totalItem = layoutConfig.itemCount;
            GameObject layoutObject = default;
            var layoutCount = 0;
            var itemInLayout = 0;
            layoutConfig.itemCount.For(index =>
            {
                if (itemInLayout <= 0)
                {
                    layoutCount++;
                    itemInLayout = layoutConfig.itemPerLayout;
                    layoutObject = layoutPool.GetGameObjectInGroup(layoutParent);
                    layoutObject.SetAsLastSiblingSafe();
                    setItem.CacheItem(layoutObject, -layoutCount);
                }
                var newItem = itemPool.GetGameObjectInGroup(layoutObject.transform);
                setItem.CacheItem(newItem, index);
                onCreate?.Invoke(newItem, index);
                itemInLayout--;
            });
            if (layoutConfig.itemHeight > 0)
            {
                setItem.SetGroupItemHight(layoutCount, layoutConfig.itemHeight, layoutConfig.itemSpace);
            }
        }

        public static void SetGroupItemHight(this GameObject setItem, int itemCount, float itemHight, float space = 0f)
        {
            var cacher = setItem.GetCacheValue(0);
            var size = setItem.GetRectTransformSizeDelta();
            cacher ??= setItem.CacheValue(size.y, 0);
            var origin = cacher.GetFloat(size.y);
            var height = itemCount * itemHight + (itemCount - 1) * space;
            size.y = origin + height;
            setItem.SetRectTransformSizeDelta(size);
        }
        public static float SetGroupItemWidth(this GameObject setItem, int itemCount, float itemWidth)
        {
            var size = setItem.GetRectTransformSizeDelta();
            var width = itemCount * itemWidth;
            size.x += width;
            setItem.SetRectTransformSizeDelta(size);
            return size.x;
        }
        public static void ResetGroupItemHight(this GameObject setItem)
        {
            var cacher = setItem.GetCacheValue(0);
            var origin = cacher.GetFloat(0f);
            setItem.SetRectTransformHeight(origin);
        }
    }
}
