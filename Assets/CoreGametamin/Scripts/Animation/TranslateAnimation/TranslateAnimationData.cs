using UnityEngine;
using DG.Tweening;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gametamin.Core
{
    [Serializable]
    public class TranslateAnimationData : BaseAnimationData
    {
        [SerializeField] Vector2 _startPosition;
        [SerializeField] Vector2 _endPosition;
        public Vector2 StartPosition
        {
            get => _startPosition;
            set => _startPosition = value;
        }
        public Vector2 EndPosition
        {
            get => _endPosition;
            set => _endPosition = value;
        }
        protected override float Distance => Vector2.Distance(_startPosition, _endPosition);
        protected override void PlayShow(GameObject target, Action callback = null)
        {
            Init();
            Translating(target, _endPosition, _toEndDuration, _toEndEase, callback);
        }
        protected override void PlayHide(GameObject target, Action callback = null)
        {
            Init();
            Translating(target, _startPosition, _toStartDuration, _toStartEase, callback);
        }
        void Translating(GameObject target, Vector2 position, float duration, Ease ease, Action callback = null)
        {
            var rect = target.GetComponentSafe<RectTransform>();
            rect.DOAnchorPos(position, duration).SetEase(ease).OnComplete(() => callback?.Invoke());
        }
        protected override void ResetAnimation(GameObject target)
        {
            target.SetAnchoredPosition(_startPosition);
        }
#if UNITY_EDITOR
        protected override void OnCustomGUI(Rect contentPosition, ref float currentLabelWidth)
        {
            StartPosition = EditorGUIHelper.GUIVector2Field(contentPosition, StartPosition, "Start Position", ref currentLabelWidth);
            contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EndPosition = EditorGUIHelper.GUIVector2Field(contentPosition, EndPosition, "End Position", ref currentLabelWidth);
        }
#endif
    }
}
