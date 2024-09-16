using System;
using System.Collections.Generic;
using UnityEngine;
namespace Gametamin.Core
{
    public class KeyManager : MonoBehaviour
    {
        static KeyManager _instance;
        static KeyManager Instance => GameObjectHelper.CreateGameObject(ref _instance, null, typeof(KeyManager).Name);
        public static Action<KeyCode> OnAnyKeyDown { get; set; }
        static bool IsActive => _instance != null;
        bool _enableEscape = true;
#if UNITY_EDITOR
        private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));
#endif
        bool _isControl;
        private Stack<IKeyListener> _listeners = new Stack<IKeyListener>();
        private IKeyListener _currentListener;
        internal void InternalAddListener(IKeyListener listener)
        {
            //Assert.IsNotInList(_listeners, listener);
            _listeners.Push(listener);
            _currentListener = listener;
        }
        internal void InternalRemoveListener(IKeyListener listener)
        {
            _listeners.TryPop(out IKeyListener _);
            if (_listeners.TryPeek(out IKeyListener result))
            {
                _currentListener = result;
            }
            else
            {
                _currentListener = null;
            }
            //if (_listeners.Remove(listener))
            //{
            //    if (_listeners.Count > 0)
            //    {
            //        if (_currentListener == listener)
            //        {
            //            _currentListener = _listeners[_listeners.Count - 1];
            //        }
            //    }
            //    else
            //    {
            //        _currentListener = null;
            //    }
            //}
        }
        private void Update()
        {
            //#if DEBUG_MODE
            //            if (Gametamin.Manager.UpdateKey()) return;
            //#endif
            if (!UserInput.Enabled)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_enableEscape && UserInput.EscapeEnabled)
                {
                    UpdateKey(KeyCode.Escape);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                UpdateKey(KeyCode.Return);
            }
            else
            {
#if UNITY_EDITOR//|| !UNITY_ANDROID || !UNITY_IOS
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode item in keyCodes)
                    {
                        if (Input.GetKeyDown(item))
                        {
                            UpdateKey(item);
                        }
                    }
                }

#if UNITY_OSX || UNITY_EDITOR_OSX
                if (Input.GetKey(KeyCode.LeftCommand)
                   || Input.GetKey(KeyCode.RightCommand))
#else
                if (Input.GetKey(KeyCode.LeftControl)
                        || Input.GetKey(KeyCode.RightControl))
#endif
                {
                    _isControl = true;
                }
                else
                {
#if UNITY_OSX || UNITY_EDITOR_OSX
                    if (Input.GetKeyUp(KeyCode.LeftCommand)
                       || Input.GetKeyUp(KeyCode.RightCommand))
#else
                    if (Input.GetKeyUp(KeyCode.LeftControl)
                                || Input.GetKeyUp(KeyCode.RightControl))
#endif
                    {
                        _isControl = false;
                    }
                }
#endif
            }
        }
        void UpdateKey(KeyCode keyCode)
        {
            if (UserInput.Enabled)
            {
                if (_currentListener != null)
                {
                    _currentListener.OnKeyPress(keyCode);
                }
                OnAnyKeyDown?.Invoke(keyCode);
            }
        }
        [ContextMenu("Log")]
        void LogListeners()
        {
            foreach (var item in _listeners)
            {
                item.OnLogger();
            }
            //_listeners.ForReverse((item, index) => item.OnLogger());
        }
        public static void AddListener(IKeyListener listener)
        {
            Instance.InternalAddListener(listener);
        }
        public static void RemoveListener(IKeyListener listener)
        {
            if (IsActive)
            {
                Instance.InternalRemoveListener(listener);
            }
        }
        public static bool EnableBackKey => Instance._enableEscape;
        public static bool IsControl => Instance._isControl;
    }
    public interface IKeyListener
    {
        void OnKeyPress(KeyCode keyCode);
        void OnLogger();
    }
}
