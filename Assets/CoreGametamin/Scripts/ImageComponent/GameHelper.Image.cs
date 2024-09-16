
#if UNITY_EDITOR
//#define GAMETAMIN_DEBUG
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Gametamin.Core
{
    public static partial class ImageHelper
    {
        public static GameObjectReference SetImageReference(this Component component, Sprite sprite, string imageId = GameObjectReferenceID.Image)
        {
            if (!component.IsNullSafe())
            {
                return component.gameObject.SetImageReference(sprite, imageId);
            }
            return default;
        }
        public static GameObjectReference SetImageReference(this GameObject root, Sprite sprite, string imageId = GameObjectReferenceID.Image)
        {
            var @ref = root.GetComponentSafe<GameObjectReference>();
            @ref.SetImageReference(sprite, imageId);
            return @ref;
        }
        public static GameObjectReference SetImageReference(this GameObjectReference @ref, Sprite sprite, string imageId = GameObjectReferenceID.Image)
        {
            var itext = @ref.GetComponentReference<SpriteLoader>(imageId);
            itext.SetSpriteSafe(sprite);
            return @ref;
        }
        public static void SetPreserveAspectReference(this GameObject target, bool active, string imageId = GameObjectReferenceID.Image)
        {
            var image = target.GetComponentReference<Image>(imageId);
            image.SetPreserveAspect(active);
        }
        public static void SetPreserveAspect(this GameObject target, bool active)
        {
            if (!target.IsNullSafe())
            {
                var image = target.GetComponentSafe<Image>();
                image.SetPreserveAspect(active);
            }
        }
        public static void SetPreserveAspect(this Image image, bool active)
        {
            if (!image.IsNullSafe())
            {
                image.preserveAspect = active;
            }
        }
        public static void SetAlpha(this GameObject target, float alpha)
        {
            if (!target.IsNullSafe())
            {
                var image = target.GetComponentSafe<Image>();
                image.SetAlpha(alpha);
            }
        }
        public static void SetAlpha(this Image image, float alpha)
        {
            if (!image.IsNullSafe())
            {
                var color = image.color;
                color.a = alpha;
                image.color = color;
            }
        }
        public static GameObject SetImageColorReference(this GameObject parent, Color color, string imageId = GameObjectReferenceID.Image)
        {
            if (!parent.IsNullSafe())
            {
                var image = parent.GetComponentReference<Image>(imageId);
                image.SetColorSafe(color);
            }
            return parent;
        }
        public static Component SetImageColorReference(this Component component, Color color, string imageId = GameObjectReferenceID.Image)
        {
            if (!component.IsNullSafe())
            {
                var image = component.GetComponentReference<Image>(imageId);
                image.SetColorSafe(color);
            }
            return component;
        }
        public static void SetColorSafe(this Image image, Color color)
        {
            if (!image.IsNullSafe())
            {
                image.color = color;
            }
        }
        public static Sprite GetSpriteReference(this GameObject imageObject, string id)
        {
            var image = imageObject.GetComponentReference<Image>(id);
            return image.GetSprite();
        }
        public static Sprite GetSprite(this GameObject imageObject)
        {
            var image = imageObject.GetComponentSafe<Image>();
            return image.GetSprite();
        }
        public static Sprite GetSprite(this Image image)
        {
            if (!image.IsNullSafe())
            {
                return image.sprite;
            }
            return default;
        }
        public static Component SetImageMaterialReference(this Component component, Material material, string imageId = GameObjectReferenceID.Image)
        {
            if (!component.IsNullSafe())
            {
                var image = component.GetComponentReference<Image>(imageId);
                if (!image.IsNullSafe())
                {
                    image.material = material;
                }
            }
            return component;
        }
    }
}
