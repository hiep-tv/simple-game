using System;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class UnityMessageEventListener : MonoBehaviour
    {
        [ContextMenu("Listeners Count")]
        public void Log()
        {
            //Log("Awake", _onAwakeCallback);
            //Log("Enable", _onStartCallback);
            //Log("Start", _onStartCallback);
            //Log("Destroy", _onDestroyCallback);
        }
        void Log(string type, Action action)
        {
            var listeners = action.GetInvocationList();
            var count = 0;
            if (listeners != null)
            {
                count = listeners.Length;
            }
            Debug.Log($"{type} listeners count: {count}");
        }
    }
}
