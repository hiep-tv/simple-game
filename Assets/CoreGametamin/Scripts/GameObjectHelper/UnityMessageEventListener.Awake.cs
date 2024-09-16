using System;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class UnityMessageEventListener : MonoBehaviour
    {
        event Action _onAwakeCallback;
        bool _awaked;
        void Awake()
        {
            _awaked = true;
            var callback = _onAwakeCallback;
            _onAwakeCallback = null;
            callback?.Invoke();
        }
        public void OnAddOnAwakeCallback(Action callback)
        {
            if (_awaked)
            {
                callback?.Invoke();
            }
            else
            {
                _onAwakeCallback += callback;
            }
        }
    }
}
