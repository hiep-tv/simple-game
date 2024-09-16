using UnityEngine;

namespace Gametamin.Core
{
    [DisallowMultipleComponent]
    public class ScalePopupWithScreen : ScaleUIWithScreen
    {
        [SerializeField] Transform[] _reverseScales;
        protected override void SetScale(Vector3 scale)
        {
            base.SetScale(scale);
            _reverseScales.For(item =>
            {
                var currentScale = item.localScale;
                item.localScale = currentScale.Divide(scale);
            });
        }
    }
}
