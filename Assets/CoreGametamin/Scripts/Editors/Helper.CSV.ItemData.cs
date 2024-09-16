#if UNITY_EDITOR
//using System.Collections.Generic;

//namespace Gametamin.Core
//{
//    public static partial class CSVHelper
//    {
//        public static ItemData[] GetItemDatas(this Dictionary<string, object> rawData)
//        {
//            UnityEngine.Debug.Log("Load ItemData from Dictionary...");
//            UnityEngine.Debug.Log("Csv format: \"Item1,Value1,Level1,Item2,Value2,Level2,...\"");
//            var count = rawData.GetSimilarKeyCount("Item");
//            var _rewards = new ItemData[count];
//            rawData.GetSimilarKeys("Item", index => $"Item{index + 1}"
//            , (result, index) =>
//            {
//                var item = _rewards[index];
//                item.itemType = result.ToEnum<ItemConsumableType>();
//                _rewards[index] = item;
//            });
//            rawData.GetSimilarKeys("Item", index => $"Value{index + 1}"
//            , (result, index) =>
//            {
//                var item = _rewards[index];
//                item.itemNumber = result.ToIntSafe();
//                _rewards[index] = item;
//            });
//            rawData.GetSimilarKeys("Item", index => $"Level{index + 1}"
//            , (result, index) =>
//            {
//                var item = _rewards[index];
//                item.itemLevel = result.ToIntSafe(1);
//                _rewards[index] = item;
//            }, true);
//            return _rewards;
//        }
//    }
//}
#endif
