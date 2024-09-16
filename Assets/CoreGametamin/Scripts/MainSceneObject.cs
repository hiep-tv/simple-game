using UnityEngine;

namespace Gametamin.Core
{
    public class MainSceneObject : GameObjectReference
    {
        private void OnDestroy()
        {
            AtlasConfigCacher.ClearCache();
            PopupHelper.Release();
            PoolHelper.Release();
        }
    }
}