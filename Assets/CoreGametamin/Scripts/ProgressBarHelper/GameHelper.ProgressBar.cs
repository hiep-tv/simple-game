using System;
using DG.Tweening;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class ProgressBarHelper
    {
        static int _indexCacheSize = 0;
        static int _indexCacheCurrentValue = 1;
        const float _updateProgressBarDuration = .5f;
        public static void UpdateProgressBarReference(this GameObjectReference iref, int next, int max, Action callback)
        {
            var progressBar = iref.GetGameObjectReference(GameObjectReferenceID.Progress_bar);
            progressBar.UpdateProgressBar(next, max, callback);
        }
        public static void UpdateProgressBar(this GameObject progressBar, int next, int max, Action callback)
        {
            var cacher = progressBar.GetCacheValue(_indexCacheCurrentValue);
            var current = cacher.GetInt();
            progressBar.UpdateProgressBar(current, next, max, callback);
        }
        public static void UpdateProgressBarReference(this GameObjectReference iref, int current, int next, int max, Action callback)
        {
            var progressBar = iref.GetGameObjectReference(GameObjectReferenceID.Progress_bar);
            progressBar.UpdateProgressBar(current, next, max, callback);
        }
        public static void UpdateProgressBar(this GameObject progressBar, int current, int next, int max, Action callback)
        {
            if (next > current)
            {
                progressBar.CacheCurrentValue(next);
                var intValue = current;
                var progressRect = progressBar.GetComponentReference<RectTransform>(GameObjectReferenceID.Image);
                var textObject = progressBar.GetGameObjectReference(GameObjectReferenceID.Text);
                DOVirtual.Float(current, next, _updateProgressBarDuration, value =>
                {
                    var newValue = Mathf.RoundToInt(value);
                    if (newValue != intValue)
                    {
                        intValue = newValue;
                        textObject.SetTextSafe($"{intValue}/{max}");
                    }
                    else
                    {
                        progressRect.SetProgressBar(value, max);
                    }
                }).OnComplete(() => callback?.Invoke());
            }
            else
            {
                progressBar.SetProgressBar(next, max);
                callback?.Invoke();
            }
        }
        /// <summary>
        /// return GameObjectReference of progress bar
        /// </summary>
        public static void SetProgressBarReference(this Component iref, int current, int max, bool hasTextProgress = true)
        {
            var progressBar = iref.GetGameObjectReference(GameObjectReferenceID.Progress_bar);
            progressBar.SetProgressBar(current, max, hasTextProgress);
        }
        /// <summary>
        /// return GameObjectReference of progress bar
        /// </summary>
        public static GameObjectReference SetProgressBarReference(this GameObjectReference iref, int current, int max, bool hasTextProgress = true)
        {
            var progressBar = iref.GetGameObjectReference(GameObjectReferenceID.Progress_bar);
            return progressBar.SetProgressBar(current, max, hasTextProgress);
        }
        /// <summary>
        /// return GameObjectReference of progress bar
        /// </summary>
        public static GameObjectReference SetProgressBarReference(this GameObject parent, int current, int max, bool hasTextProgress = true)
        {
            var progressBar = parent.GetGameObjectReference(GameObjectReferenceID.Progress_bar);
            return progressBar.SetProgressBar(current, max, hasTextProgress);
        }
        /// <summary>
        /// return GameObjectReference of progress bar
        /// </summary>
        public static GameObjectReference SetProgressBar(this GameObject progressBar, int current, int max, bool hasTextProgress = true)
        {
            if (!progressBar.IsNullSafe())
            {
                progressBar.CacheCurrentValue(current);
                var bar = progressBar.GetComponentReference<RectTransform>(GameObjectReferenceID.Image);
                bar.SetProgressBar(current, max);
                if (hasTextProgress)
                {
                    progressBar.SetTextReference($"{current}/{max}");
                }
            }
            return progressBar.GetComponentSafe<GameObjectReference>();
        }
        static void SetProgressBar(this RectTransform bar, float current, float max)
        {
            if (bar.IsNullSafe()) return;
            current = Mathf.Min(current, max);
            var sizeDelta = bar.sizeDelta;
            var cacheValue = bar.GetCacheValue(_indexCacheSize) ?? bar.CacheValue(sizeDelta.x, _indexCacheSize);
            var width = (float)cacheValue.Value;
            sizeDelta.x = width * current / max;
            bar.sizeDelta = sizeDelta;
        }
        static void CacheCurrentValue(this GameObject progressBar, int value)
        {
            progressBar.CacheValue(value, _indexCacheCurrentValue);
        }
    }
}
