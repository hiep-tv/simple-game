using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static class KeyListenerHelper
    {
        public static GameObjectReference AddKeyListener(this Component component, string listenerId, KeyCode keyCode = KeyCode.Escape)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.AddKeyListener(listenerId, keyCode);
            }
            return default;
        }
        public static GameObjectReference AddKeyListener(this Component component, GameObject listener, KeyCode keyCode = KeyCode.Escape)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.AddKeyListener(listener, keyCode);
            }
            return default;
        }
        public static GameObjectReference AddKeyListener(this GameObject rootObject, string listenerId, KeyCode keyCode = KeyCode.Escape)
        {
            var listener = rootObject.GetGameObjectReference(listenerId);
            return rootObject.AddKeyListener(listener, keyCode);
        }
        public static GameObjectReference AddKeyListener(this GameObject rootObject, GameObject listener, KeyCode keyCode = KeyCode.Escape)
        {
            if (!rootObject.IsNullSafe())
            {
                var @ref = rootObject.GetComponentSafe<GameObjectReference>();
                @ref.AddKeyListener(listener, keyCode);
                return @ref;
            }
            return default;
        }
        public static GameObjectReference AddKeyListener(this GameObjectReference @ref, string listenerId, KeyCode keyCode = KeyCode.Escape)
        {
            var listener = @ref.GetGameObjectReference(listenerId);
            return @ref.AddKeyListener(listener, keyCode);
        }
        public static GameObjectReference AddKeyListener(this GameObjectReference @ref, GameObject listener, KeyCode keyCode = KeyCode.Escape)
        {
            if (!@ref.IsNullSafe() && !listener.IsNullSafe())
            {
                var manager = @ref.GetOrAddComponentSafe<KeyListeners>();
                if (!manager.IsNullSafe())
                {
                    manager.OnAddListener(listener, keyCode);
                }
            }
            return @ref;
        }
        public static GameObjectReference AddKeyListener(this GameObjectReference @ref, Action onClick = null, KeyCode keyCode = KeyCode.Escape)
        {
            if (!@ref.IsNullSafe())
            {
                var manager = @ref.GetOrAddComponentSafe<KeyListeners>();
                if (!manager.IsNullSafe())
                {
                    manager.OnAddListener(keyCode, onClick);
                }
            }
            return @ref;
        }
        public static GameObjectReference RemoveKeyListener(this GameObjectReference @ref, KeyCode keyCode = KeyCode.Escape)
        {
            if (!@ref.IsNullSafe())
            {
                var manager = @ref.GetOrAddComponentSafe<KeyListeners>();
                if (!manager.IsNullSafe())
                {
                    manager.OnRemoveListener(keyCode);
                }
            }
            return @ref;
        }
        public static GameObjectReference RegisterKeyListenning(this Component component)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.RegisterKeyListenning();
            }
            return default;
        }
        public static GameObjectReference RegisterKeyListenning(this GameObject rootObject)
        {
            if (!rootObject.IsNullSafe())
            {
                var @ref = rootObject.GetComponentSafe<GameObjectReference>();
                @ref.RegisterKeyListenning();
                return @ref;
            }
            return default;
        }
        public static GameObjectReference RegisterKeyListenning(this GameObjectReference @ref)
        {
            if (!@ref.IsNullSafe())
            {
                var manager = @ref.GetOrAddComponentSafe<KeyListeners>();
                if (!manager.IsNullSafe())
                {
                    manager.OnRegisterKeyListenning();
                }
            }
            return @ref;
        }
        public static GameObjectReference RemoveKeyListenning(this Component component)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.RemoveKeyListenning();
            }
            return default;
        }
        public static GameObjectReference RemoveKeyListenning(this GameObject rootObject)
        {
            if (!rootObject.IsNullSafe())
            {
                var @ref = rootObject.GetComponentSafe<GameObjectReference>();
                @ref.RemoveKeyListenning();
                return @ref;
            }
            return default;
        }
        public static GameObjectReference RemoveKeyListenning(this GameObjectReference @ref)
        {
            if (!@ref.IsNullSafe())
            {
                var manager = @ref.GetOrAddComponentSafe<KeyListeners>();
                if (!manager.IsNullSafe())
                {
                    manager.OnRemoveKeyListenning();
                }
            }
            return @ref;
        }
    }
}
