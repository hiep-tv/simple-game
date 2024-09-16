using System;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class UnityMessageEventListener : MonoBehaviour
    {
        event Action _onDisableCallback;
        bool _disabled;
        void OnDisable()
        {
#if DEBUG_MODE
            OnDisableLogger();
#endif
            _disabled = true;
            _enabled = false;
            _onDisableCallback?.Invoke();
        }
        public void OnAddOnDisableCallback(Action callback, bool raiseIfDisabled)
        {
            _onDisableCallback += callback;
            if (_disabled && raiseIfDisabled)
            {
                callback?.Invoke();
            }
        }
        public void OnRemoveOnDisableCallback(Action callback)
        {
            _onDisableCallback -= callback;
        }
#if DEBUG_MODE
        [SerializeField] bool _logOnDisable;
        void OnDisableLogger()
        {
            if (_logOnDisable)
            {
                Debug.Log($"OnDisble: {name}");
            }
        }
#endif
    }
}
