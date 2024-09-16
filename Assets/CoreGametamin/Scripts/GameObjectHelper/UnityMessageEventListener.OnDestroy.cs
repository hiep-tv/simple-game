using System;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class UnityMessageEventListener : MonoBehaviour
    {
        event Action _onDestroyCallback;
        bool _destroyed;
        void OnDestroy()
        {
            _destroyed = true;
            var callback = _onDestroyCallback;
            _onDestroyCallback = null;
            callback?.Invoke();
        }
        public void OnAddOnDestroyCallback(Action callback)
        {
            if (_destroyed)
            {
                callback?.Invoke();
            }
            else
            {
                _onDestroyCallback += callback;
            }
        }
    }
}
