using System;
using UnityEngine;

namespace Gametamin.Core
{
    public interface IKeyListeners : IKeyListener
    {
        void OnRegisterKeyListenning();
        void OnRemoveKeyListenning();
        void OnAddListener(GameObject listeners, KeyCode keyCode);
        void OnAddListener(KeyCode keyCode, Action onClick = null);
    }
}
