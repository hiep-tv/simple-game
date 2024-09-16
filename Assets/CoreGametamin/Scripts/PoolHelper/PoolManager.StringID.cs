using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PoolHelper
    {
        public static GameObject GetGameObject(string id)
        {
            return PoolManagerString.GetGameObject(id);
        }
        public static void AddGameObject(string id, GameObject instance)
        {
            PoolManagerString.AddGameObject(id, instance);
        }
        class PoolManagerString : BasePoolManager<string>
        {

        }
    }
}
