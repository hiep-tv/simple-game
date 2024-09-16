using System;
using UnityEngine;
namespace Gametamin.Core
{
    public class TimeCountdown : MonoBehaviour
    {
        DateTime? _endTime;
        Action _endCallback;
        float _remainingTime = -1;
        float _deltaTime;
        int _seconds;
        protected ITextComponent _itextComponent;
        protected virtual ITextComponent _ITextComponent => gameObject.GetComponentSafe(ref _itextComponent);
        Func<DateTime> _onGetCurrentDatime = null;
        public bool Ended => _remainingTime <= 0;
        public virtual bool CanSetEndLabel
        {
            get;
            set;
        } = true;
        void OnEnable()
        {
            if (!Ended)
            {
                UpdateTime();
            }
        }
        void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                if (!Ended)
                {
                    UpdateTime();
                }
            }
        }
        public void SetEnded()
        {
            _remainingTime = 0;
        }
        public void OnSetTime(DateTime? endTime, Func<DateTime> onGetCurrentDatime = null, Action onEnded = null)
        {
            _endTime = endTime;
            _onGetCurrentDatime = onGetCurrentDatime;
            _endCallback = onEnded;
            if (_endTime.HasValue)
            {
                UpdateTime();
            }
            else
            {
                _remainingTime = 0;
                _seconds = 0;
                _ITextComponent.Text = string.Empty;
                onEnded?.Invoke();
            }
        }
        protected virtual string GetText()
        {
            return GetText(_seconds);
        }
        protected virtual string GetText(int seconds)
        {
            return seconds.ToString();//.ToHMSString();
        }
        protected virtual void OnEnded()
        {
            _remainingTime = 0;
            if (CanSetEndLabel)
            {
                _ITextComponent.Text = TextReferenceID.Finished.GetTextById();
            }
            _endCallback?.Invoke();
        }
        void UpdateTime()
        {
            if (_endTime.HasValue)
            {
                var current = _onGetCurrentDatime?.Invoke();
                if (current.HasValue)
                {
                    SetRemainingTime((float)_endTime.Value.Subtract(current.Value).TotalSeconds);
                }
            }
            else
            {
                SetRemainingTime(0);
            }
        }
        void SetRemainingTime(float remainingTime)
        {
            _remainingTime = remainingTime;
            _deltaTime = 0;

            int seconds = remainingTime > 0 ? Mathf.CeilToInt(remainingTime) : 0;
            if (seconds <= 0)
            {
                OnEnded();
            }
            else
            {
                if (_seconds != seconds)
                {
                    _seconds = seconds;
                    _ITextComponent.Text = GetText(seconds);
                    if (seconds <= 0)
                    {
                        OnEnded();
                    }
                }
            }
        }
        void Update()
        {
            if (_remainingTime > 0)
            {
                _deltaTime += Time.deltaTime;
                if (_deltaTime > 1)
                {
                    SetRemainingTime(_remainingTime - _deltaTime);
                }
            }
        }
    }
}