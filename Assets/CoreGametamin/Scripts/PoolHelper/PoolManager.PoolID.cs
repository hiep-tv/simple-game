using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PoolHelper
    {
        public static GameObject GetGameObject(PoolReferenceID id)
        {
            return PoolManagerPoolID.GetGameObject(id);
        }
        public static void AddGameObject(PoolReferenceID id, GameObject instance)
        {
            PoolManagerPoolID.AddGameObject(id, instance);
        }
        public static GameObject GetGameObjectInGroup(this PoolReferenceID id, GameObject poolObject, Transform parent)
        {
            return PoolManagerPoolID.GetGameObjectInGroup(id, poolObject, parent);
        }
        public static void ReleasePool(this PoolReferenceID id, Action<GameObject> callback = null)
        {
            PoolManagerPoolID.ReleasePool(id, callback);
        }
        class PoolManagerPoolID : BasePoolManager<PoolReferenceID>
        {
        }
    }
}
