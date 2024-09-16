using System;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class UnityMessageEventListener : MonoBehaviour
    {
        event Action _onStartCallback;
        bool _started;
        void Start()
        {
            _started = true;
            var callback = _onStartCallback;
            _onStartCallback = null;
            callback?.Invoke();
        }
        public void OnAddOnStartCallback(Action callback)
        {
            if (_started)
            {
                callback?.Invoke();
            }
            else
            {
                _onStartCallback += callback;
            }
        }
    }
}
