namespace Gametamin.Core
{
    public class PlayTriggerAnimator : PlayAnimator
    {
        protected override void PlayAnimation(string stateName)
        {
            _Animator.SetTrigger(stateName);
        }
    }
}
