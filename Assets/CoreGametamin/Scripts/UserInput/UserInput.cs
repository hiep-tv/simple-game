using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Gametamin.Core
{
    public class UserInput : MonoBehaviour
    {
        public static bool BlockOtherButtons { get; }
        static UserInput _instance;
        public static UserInput Instance
        {
            get
            {
                if (_instance == null)
                {
                    var canvas = PoolHelper.CreateCanvasScreenSpaceCamera(typeof(UserInput).Name);
                    canvas.sortingOrder = 100;
                    _instance = canvas.gameObject.GetOrAddComponentSafe<UserInput>();
                    _instance.Init();
                    //Context.MainCanvas = canvas;
                    //_instance = FindAnyObjectByType<UserInput>();
                }
                return _instance;
            }
        }
        GameObject _blockRaycast;
        bool _canUpdate => Enabled;

        void Init()
        {
            _blockRaycast = new GameObject("Raycast", typeof(RectTransform));
            _blockRaycast.transform.SetParent(transform, false);
            var rect = _blockRaycast.GetComponent<RectTransform>();
            rect.anchorMin = Vector3.zero;
            rect.anchorMax = Vector3.one;
            rect.sizeDelta = Vector3.zero;
            var image = _blockRaycast.AddComponent<Image>();
            image.SetAlpha(0f);
            _blockRaycast.SetActiveSafe(!_enabled);
        }
        public static event Action<Vector2> OnPointerDown;
        public static event Action<Vector2> OnPointerUp;
        public static event Action<Vector2> OnPointerDrag;
        public static event Action<Vector2> OnStationary;
        bool _enabled;
        bool _Enabled
        {
            get => _enabled && !_forceBlock;
            set
            {

#if DEBUG_MODE
                if (ShowDebug)
                {
                    Debug.Log($"user input: {_enabled} => {value}");
                }
#endif
                if (_ForceBlock) return;
                _enabled = value;
                SetBlockRayCast(!value);
            }
        }
        bool _forceBlock;
        bool _ForceBlock
        {
            get => _forceBlock;
            set
            {
                _enabled = value;
                _forceBlock = value;
                SetBlockRayCast(value);
            }
        }
        void SetBlockRayCast(bool enabled)
        {
            if (enabled)
            {
                transform.SetAsLastSibling();
            }
            _blockRaycast.SetActiveSafe(enabled);
        }
        public static bool ForceBlock
        {
            get => Instance._ForceBlock;
            set => Instance._ForceBlock = value;
        }
        public static bool Enabled
        {
            get => Instance._Enabled;
            set => Instance._Enabled = value;
        }
        public static bool EscapeEnabled
        {
            get;
            set;
        } = true;
        void Awake()
        {
            _Enabled = true;
            transform.SetParent(null);
            transform.SetAsLastSibling();
        }
        public static void TrySetEnabled(bool enabled)
        {
            if (_instance != null)
            {
                _instance._Enabled = enabled;
            }
        }
        private void Update()
        {
            if (_canUpdate)
            {
#if !UNITY_EDITOR
                if (Input.touchSupported)
                {
                    if (Input.touchCount == 1)
                    {
                        var touch = Input.GetTouch(0);
                        if (touch.phase == TouchPhase.Began)
                        {
                            PointerDown(touch.position);
                        }
                        else if (touch.phase == TouchPhase.Moved)
                        {
                            PointerDrag(touch.position);
                        }
                        else if (touch.phase == TouchPhase.Stationary)
                        {
                            PointerStationary(touch.position);
                        }
                        else if (touch.phase == TouchPhase.Ended)
                        {
                            PointerUp(touch.position);
                        }
                    }
                }
                else
#endif
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        PointerDown(Input.mousePosition);
                    }
                    else if (Input.GetMouseButton(0))
                    {
                        PointerDrag(Input.mousePosition);
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        PointerUp(Input.mousePosition);
                    }
                }
            }
        }
        static bool Pause;
        void PointerDown(Vector2 position)
        {
            OnPointerDown?.Invoke(position);
        }
        void PointerStationary(Vector2 position)
        {
            OnStationary?.Invoke(position);
        }
        void PointerDrag(Vector2 position)
        {
            OnPointerDrag?.Invoke(position);
        }
        void PointerUp(Vector2 position)
        {
            OnPointerUp?.Invoke(position);
        }
        public static bool IsPointerOverGameObject()
        {
            if (Input.touchSupported)
            {
                for (int i = 0, count = Input.touchCount; i < count; i++)
                {
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    {
                        return true;
                    }
                }
                return false;
            }
            var moseOver = EventSystem.current.IsPointerOverGameObject(-1);
            return moseOver;
        }
        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
        private void OnDestroy()
        {
            OnPointerDown = null;
            OnPointerUp = null;
            OnPointerDrag = null;
            OnStationary = null;
        }
#if DEBUG_MODE
        public static bool ShowDebug
        {
            get => LocalPrefs.GetBool("userinpu_debug", false);
            set => LocalPrefs.SetBool("userinpu_debug", value);
        }
#endif
    }
}