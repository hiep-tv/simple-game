#if DEBUG_MODE || UNITY_EDITOR
#define CACHE_ITEM_TRACKING
#endif
#if CACHE_ITEM_TRACKING
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public class CacheItemDebugger : MonoBehaviour
    {
        [SerializeField] List<ItemData> _itemDatas;
        public void AddItem(GameObject itemObject, int id)
        {
            if (_itemDatas.GetCountSafe() == 0)
            {
                _itemDatas = new();
            }
            _itemDatas.Add(new ItemData(itemObject, id));
        }
        public void Clear()
        {
            _itemDatas.Clear();
        }
        public void Clear(GameObject itemObject)
        {
            _itemDatas.ForReverseBreakable((item, index) =>
            {
                var exist = item.ItemObject.GetInstanceIDSafe() == itemObject.GetInstanceIDSafe();
                if (exist)
                {
                    _itemDatas.RemoveAt(index);
                }
                return exist;
            });
        }
        [Serializable]
        class ItemData
        {
            [SerializeField] GameObject _itemObject;
            [SerializeField] int _id;

            public ItemData(GameObject itemObject, int id)
            {
                _itemObject = itemObject;
                _id = id;
            }

            public GameObject ItemObject => _itemObject;
        }
    }
}
#endif