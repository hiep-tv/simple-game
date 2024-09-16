using System;
namespace Gametamin.Core
{
    public class TimeCountdownWithLabel : TimeCountdown
    {
        string _label;
        public string OnSetTime(string label, DateTime? endTime, Func<DateTime> onGetCurrentDatime = null, Action onEnded = null)
        {
            _label = label;
            OnSetTime(endTime, onGetCurrentDatime, onEnded);
            return GetText();
        }
        protected override string GetText(int seconds)
        {
            return _label;// string.Format(_label, seconds.ToHMSString());
        }
    }
}
