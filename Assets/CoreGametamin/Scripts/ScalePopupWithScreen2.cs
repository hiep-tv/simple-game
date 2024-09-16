using UnityEngine;

namespace Gametamin.Core
{
    [DisallowMultipleComponent]
    public class ScalePopupWithScreen2 : ScaleUIWithScreen
    {
        [SerializeField][GameObjectReferenceID] string[] _reverseScales;
        Vector3 _scale;
        bool _needScale;
        protected override void SetScale(Vector3 scale)
        {
            base.SetScale(scale);
            _needScale = true;
            _scale = scale;
        }
        private void Start()
        {
            if (_needScale)
            {
                _reverseScales.For(itemId =>
                {
                    var item = gameObject.GetTransformReference(itemId);
                    var currentScale = item.localScale;
                    item.localScale = currentScale.Divide(_scale);
                });
            }
        }
    }
}
