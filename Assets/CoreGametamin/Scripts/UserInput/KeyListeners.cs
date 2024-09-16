using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Gametamin.Core
{
    [DisallowMultipleComponent]
    public class KeyListeners : MonoBehaviour, IKeyListeners
    {
        Dictionary<KeyCode, IListener> _listeners;
        Dictionary<KeyCode, IListener> _Listeners => _listeners ??= new();
        interface IListener
        {
            void OnKeyPress();
            void OnLogger(KeyCode keyCode);
        }
        struct AnonymousListener : IListener
        {
            Action _onClick;
            public AnonymousListener(Action onClick = null)
            {
                _onClick = onClick;
            }
            public void OnLogger(KeyCode keyCode)
            {
                Debug.Log($"Anonymous {keyCode}");
            }
            void IListener.OnKeyPress()
            {
                _onClick?.Invoke();
            }
        }
        struct KeyListener : IListener
        {
            public GameObject listener;
            public KeyListener(GameObject listener)
            {
                this.listener = listener;
            }
            public void OnKeyPress()
            {
                KeyPressed();
            }
            public void OnLogger(KeyCode keyCode)
            {
                Debug.Log($"{listener.name} {keyCode}");
            }
            void KeyPressed()
            {
                if (listener.IsActiveInHierarchySafe())
                {
                    var ibutton = listener.GetComponentSafe<IClickButton>();
                    ibutton?.OnClickButton();
                }
            }
        }

        public void OnAddListener(KeyCode keyCode, Action onClick = null)
        {
            if (!_Listeners.ContainsKey(keyCode))
            {
                _Listeners.Add(keyCode, new AnonymousListener(onClick));
            }
        }
        public void OnAddListener(GameObject listener, KeyCode keyCode)
        {
            if (!_Listeners.ContainsKey(keyCode))
            {
                _Listeners.Add(keyCode, new KeyListener(listener));
            }
        }
        public void OnRemoveListener(KeyCode keyCode)
        {
            if (_Listeners.ContainsKey(keyCode))
            {
                _Listeners.Remove(keyCode);
            }
        }
        public void OnKeyPress(KeyCode keyCode)
        {
            if (_Listeners.TryGetValue(keyCode, out IListener ilistener))
            {
                ilistener.OnKeyPress();
            }
        }
        public void OnRegisterKeyListenning()
        {
            //Debug.Log($"AddListener {name}");
            KeyManager.AddListener(this);
        }
        public void OnRemoveKeyListenning()
        {
            //Debug.Log($"RemoveListener {name}");
            KeyManager.RemoveListener(this);
        }
        public void OnLogger()
        {
            Debug.Log($"============start log listener {name}============");
            foreach (var item in _Listeners)
            {
                item.Value.OnLogger(item.Key);
            }
            Debug.Log($"============end log listener {name}============");
        }

    }
}

