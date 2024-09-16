using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PoolHelper
    {
        public static GameObject GetGameObjectInGroup(this (int, int) id, GameObject poolObject, Transform parent)
        {
            return PoolManagerIntTuple.GetGameObjectInGroup(id, poolObject, parent);
        }
        public static void ReleasePool(this (int, int) id, Action<GameObject> callback = null)
        {
            PoolManagerIntTuple.ReleasePool(id, callback);
        }
        class PoolManagerIntTuple : BasePoolManager<(int, int)>
        {
        }
    }
}
