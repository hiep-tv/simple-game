using System;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class UnityMessageEventListener : MonoBehaviour
    {
        event Action _onEnableCallback;
        bool _enabled;
        void OnEnable()
        {
#if DEBUG_MODE
            OnEnableLogger();
#endif
            _enabled = true;
            _disabled = false;
            var callback = _onEnableCallback;
            _onEnableCallback = null;
            callback?.Invoke();
        }
        public void OnAddOnEnableCallback(Action callback, bool raiseIfEnabled)
        {
            _onEnableCallback += callback;
            if (_enabled && raiseIfEnabled)
            {
                callback?.Invoke();
            }
        }
        public void OnRemoveOnEnableCallback(Action callback)
        {
            _onEnableCallback -= callback;
        }
#if DEBUG_MODE
        [SerializeField] bool _logOnEnable;
        void OnEnableLogger()
        {
            if (_logOnEnable)
            {
                Debug.Log($"OnEnable: {name}");
            }
        }
#endif
    }
}
