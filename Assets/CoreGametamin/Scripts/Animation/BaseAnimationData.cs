using System;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
namespace Gametamin.Core
{
    public abstract class BaseAnimationData : ScriptableObject//, IAnimationData
    {
        [SerializeField] float _toStartSpeed;
        [SerializeField] float _toEndSpeed;
        [SerializeField] protected Ease _toStartEase = Ease.Linear;
        [SerializeField] protected Ease _toEndEase = Ease.Linear;
        [NonSerialized] protected float _toStartDuration, _toEndDuration;
        [NonSerialized] bool _initialized;
        protected void Init()
        {
            if (!_initialized)
            {
                //if (!Helper.IsEditor)
                {
                    _initialized = true;
                }
                var distance = Distance;
                _toStartDuration = distance / _toStartSpeed;
                _toEndDuration = distance / _toEndSpeed;
            }
        }
        protected abstract float Distance { get; }
        public float ToStartSpeed { get => _toStartSpeed; set => _toStartSpeed = value; }
        public float ToEndSpeed { get => _toEndSpeed; set => _toEndSpeed = value; }
        public Ease ToStartEase { get => _toStartEase; set => _toStartEase = value; }
        public Ease ToEndEase { get => _toEndEase; set => _toEndEase = value; }
        public void OnHide(GameObject target, Action callback = null)
        {
            Init();
            PlayHide(target, callback);
        }
        public void OnShow(GameObject target, Action callback = null)
        {
            Init();
            PlayShow(target, callback);
        }

        public void OnReset(GameObject target)
        {
            Init();
            ResetAnimation(target);
        }
        protected abstract void PlayShow(GameObject target, Action callback = null);
        protected abstract void PlayHide(GameObject target, Action callback = null);
        protected abstract void ResetAnimation(GameObject target);

#if UNITY_EDITOR
        public void OnGUI2(Rect contentPosition, ref float currentLabelWidth)
        {
            var space = EditorGUIUtility.standardVerticalSpacing;
            var startX = contentPosition.x;
            var startWidth = contentPosition.width;
            var width = contentPosition.width / 2f - space / 2f;
            contentPosition.width = width;
            ToStartSpeed = EditorGUIHelper.GUIFloatField(contentPosition, ToStartSpeed, "Start Speed", ref currentLabelWidth);
            contentPosition.x += width + space;
            EditorGUIHelper.GUIEnumWithSearch(contentPosition, ToStartEase, value =>
            {
                ToStartEase = value;
                Save();
            });
            contentPosition.x = startX;
            contentPosition.y += EditorGUIUtility.singleLineHeight + space;
            ToEndSpeed = EditorGUIHelper.GUIFloatField(contentPosition, ToEndSpeed, "End Speed", ref currentLabelWidth);
            contentPosition.x += width + space;
            EditorGUIHelper.GUIEnumWithSearch(contentPosition, ToEndEase, value =>
            {
                ToEndEase = value;
                Save();
            });
            contentPosition.width = startWidth;
            contentPosition.y += EditorGUIUtility.singleLineHeight + space;
            contentPosition.x = startX;
            OnCustomGUI(contentPosition, ref currentLabelWidth);
            if (GUI.changed)
            {
                Save();
            }
        }
        public void OnSave()
        {
            Save();
        }
        void Save()
        {
            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssetIfDirty(this);
            }
        }
        protected virtual void OnCustomGUI(Rect contentPosition, ref float currentLabelWidth)
        {

        }

#endif
    }
}
