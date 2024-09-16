using UnityEngine;
namespace Gametamin.Core
{
    public class PoolGameObjectReference : MonoBehaviour
    {
        [SerializeField] GameObjectReferenceData[] _poolDatas;

        public void For(System.Action<GameObject> callback = null)
        {
            _poolDatas.For(item => callback?.Invoke(item.Target));
        }
        public GameObject OnGetPool(string id)
        {
            GameObject result = default;
            var exist = false;
            _poolDatas.ForBreakable(item =>
            {
                exist = item.Id.EqualsSafe(id);
                if (exist)
                {
#if UNITY_EDITOR||DEBUG
                    if (item.Target == null)
                    {
                        Log($"ID {id} attached on {name} found but pool is missing!");
                    }
#endif
                    result = item.Target;
                }
                return exist;
            });
#if UNITY_EDITOR||DEBUG
            if (!exist)
            {
                Log($"No pool has id \"{id}\" attached on {name}!");
            }
#endif
            return result;
        }
        public void OnSetPool(GameObjectReferenceData[] poolDatas)
        {
            _poolDatas = poolDatas;
        }
#if UNITY_EDITOR||DEBUG
        void Log(string message)
        {
            Debug.LogError(message);
        }
#endif
    }
}
