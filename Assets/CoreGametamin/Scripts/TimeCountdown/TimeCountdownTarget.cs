using UnityEngine;

namespace Gametamin.Core
{
    public class TimeCountdownTarget : TimeCountdown
    {
        [SerializeField] GameObject _textObject;
        protected override ITextComponent _ITextComponent => _textObject.GetComponentSafe(ref _itextComponent);
    }
}
