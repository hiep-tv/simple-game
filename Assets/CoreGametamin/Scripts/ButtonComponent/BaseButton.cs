using UnityEngine;

namespace Gametamin.Core
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ButtonBlockable))]
    public abstract partial class BaseButton : MonoBehaviour, IClickButton, IBlockButton
    {
        [Tooltip("If useFastClick = false, button will be blocked in [blockTime]")]
        [SerializeField] bool useFastClick = false;
        [SerializeField] float blockTime = 0.5f;
        bool isBlocking;
        IBlockable _iblockable;
        IBlockable _IBlockable => gameObject.GetComponentSafe(ref _iblockable);
        ITakeButtonClick[] _listeners;
        ITakeButtonClick[] _Listeners
        {
            get
            {
                if (_listeners == null)
                {
                    _listeners = gameObject.GetComponentsSafe<ITakeButtonClick>();
                }
                return _listeners;
            }
        }
        public abstract bool Interactable
        {
            get;
            set;
        }
        protected virtual void Awake()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            OnButtonAwake();
        }
        protected virtual void OnButtonAwake()
        {
        }
        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            OnButtonEnable();
        }
        protected virtual void OnButtonEnable()
        {
        }
        protected virtual void Start()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            OnButtonStart();
        }
        protected virtual void OnButtonStart()
        {
        }
        protected void OnClick()
        {
            if (Clickable)
            {
                Click();
                if (!useFastClick)
                {
                    isBlocking = true;
#if DOTWEEN
                    blockTime.DelayCall(OnEndBlockButton);
#else
                    OnEndBlockButton();
#endif
                }
            }
        }
        bool Clickable
        {
            get
            {
                if (Interactable && !isBlocking)
                {
                    return _IBlockable.IsBlocker;
                }
                return false;
            }
        }
        public bool OnClickButton()
        {
            var clickable = Clickable;
            if (clickable)
            {
                Click();
                if (!useFastClick)
                {
                    isBlocking = true;
#if DOTWEEN
                    blockTime.DelayCall(OnEndBlockButton);
#else
                    OnEndBlockButton();
#endif
                }
            }
            return clickable;
        }
        void Click()
        {
            UnblockOthers();
            SendClickEvent();
            ClickButton();
        }
        void SendClickEvent()
        {
            _Listeners.For(item => item.OnTakeButtonClick());
        }
        void OnEndBlockButton()
        {
            if (!this.IsNullSafe())
            {
                isBlocking = false;
            }
        }
        protected virtual void ClickButton()
        {

        }
        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            OnButtonDisable();
        }
        protected virtual void OnButtonDisable()
        {
        }
        public virtual void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        public void OnBlockOthers()
        {
            _IBlockable.OnBlockOthers();
        }
        public void OnUnblockOthers()
        {
            UnblockOthers();
        }
        void UnblockOthers()
        {
            _IBlockable.OnUnblockOthers();
        }
        protected virtual void OnDestroy()
        {
            UnblockOthers();
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            OnButtonDestroy();
        }
        protected virtual void OnButtonDestroy()
        {
        }
    }
}