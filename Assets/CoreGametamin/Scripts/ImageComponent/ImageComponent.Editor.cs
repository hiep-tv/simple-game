#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
namespace Gametamin.Core
{
    public partial class ImageComponent
    {
        //public override void CheckNativeSize(Sprite sprite)
        //{
        //    if (sprite.border == Vector4.zero)
        //    {
        //        if (_source.type != Image.Type.Simple)
        //        {
        //            _source.type = Image.Type.Simple;
        //            SaveImage(_source);
        //        }
        //        var rect = _Source.rectTransform;
        //        var stretched = rect.anchorMin == Vector2.zero && rect.anchorMax == Vector2.one;
        //        if (stretched)
        //        {
        //            var parent = Helper.GetComponentSafe<RectTransform>(_Source.rectTransform.parent.gameObject);
        //            var parentCenter = parent.anchorMin == Vector2.one / 2f && parent.anchorMax == Vector2.one / 2f;
        //            if (parentCenter)
        //            {
        //                parent.sizeDelta = sprite.GetSpriteSize();
        //            }
        //        }
        //        else if (rect.anchorMin == Vector2.one / 2f && rect.anchorMax == Vector2.one / 2f)
        //        {
        //            rect.sizeDelta = sprite.GetSpriteSize();
        //        }
        //    }
        //}
    }
}
#endif