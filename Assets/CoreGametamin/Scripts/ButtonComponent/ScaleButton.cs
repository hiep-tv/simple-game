#if DOTWEEN
using DG.Tweening;
#endif
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gametamin.Core
{
    public class ScaleButton : BaseScaleButton, UnityEngine.EventSystems.IPointerDownHandler,
       UnityEngine.EventSystems.IPointerUpHandler, UnityEngine.EventSystems.IPointerExitHandler, UnityEngine.EventSystems.IPointerEnterHandler
    {
        void UnityEngine.EventSystems.IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            PointerDown();
        }

        void UnityEngine.EventSystems.IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            PointerExit();
        }

        void UnityEngine.EventSystems.IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            PointerUp();
        }

        void UnityEngine.EventSystems.IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter();
        }

    }
    public class BaseScaleButton : MonoBehaviour, ISetScale
    {
        [SerializeField]
        Transform _scaleTarget;
        [SerializeField]
        float _duration = 0.1f;
        [SerializeField]
        float _scaleFactor = 1.1f;
#if DOTWEEN
        IInteractable _iinteractable;
        IPlayButtonAnimation _playAnimation;
        IPlayButtonAnimation _PlayAnimation => gameObject.GetComponentSafe(ref _playAnimation);
        IBlockable _iblockable;
        IBlockable _IBlockable => gameObject.GetComponentSafe(ref _iblockable);
        Vector3 _startScale = Vector3.one;
        bool _isPointerDown;
        Tween _tween;
        bool _IsPointerEnter
        {
            set
            {
                if (_tween.IsActive())
                {
                    _tween.Kill();
                    _tween = null;
                }
                if (value && _IBlockable.IsBlocker)
                {
                    if (_PlayAnimation != null)
                    {
                        _PlayAnimation.OnHide(() =>
                        {
                            _tween = _scaleTarget.DOScale(_startScale * _scaleFactor, _duration);
                        });
                    }
                    else
                    {
                        _tween = _scaleTarget.DOScale(_startScale * _scaleFactor, _duration);
                    }
                }
                else
                {
                    _tween = _scaleTarget.DOScale(_startScale, _duration).OnComplete(() =>
                    {
                        if (_PlayAnimation != null)
                        {
                            _PlayAnimation.OnShow();
                        }
                    });
                }
            }
        }
        void Start()
        {
            _iinteractable = gameObject.GetComponentSafe<IInteractable>();
            _startScale = _scaleTarget.localScale;
        }
        protected void Scale()
        {
            if (_iinteractable != null)
            {
                _IsPointerEnter = _iinteractable.Interactable;
            }
            else
            {
                _IsPointerEnter = true;
            }
        }
        private void OnDestroy()
        {
            if (_tween.IsActive())
            {
                _tween.Kill();
                _tween = null;
            }
        }
#endif
        public void SetTarget(GameObject target)
        {
            _scaleTarget = target.transform;
        }
        protected void PointerDown()
        {
#if DOTWEEN
            _isPointerDown = true;
            Scale();
#endif
        }
        protected void PointerUp()
        {
#if DOTWEEN
            _IsPointerEnter = false;
            _isPointerDown = false;
#endif
        }
        protected void PointerEnter()
        {
#if DOTWEEN
            if (_isPointerDown)
            {
                Scale();
            }
#endif
        }
        protected void PointerExit()
        {
#if DOTWEEN
            _IsPointerEnter = false;
#endif
        }

        public void OnSetScale(Vector3 scale)
        {
            _startScale = scale;
            _scaleTarget.localScale = scale;
        }
    }
}