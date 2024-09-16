using System;

namespace Gametamin.Core
{
    public static partial class PopupBuilder
    {
        public static void ForceLoadDirectTexture(this GameObjectReference @ref, Action callback = null)
        {
            var loaders = @ref.GetComponentsInChildrenSafe<SpriteDirectLoader>();
            var loadCount = loaders.GetCountSafe();
            if (loadCount > 0)
            {
                loaders.For(loader => loader.LoadSprite(OnLoaded));
                void OnLoaded()
                {
                    loadCount--;
                    if (loadCount <= 0)
                    {
                        callback?.Invoke();
                    }
                }
            }
            else
            {
                callback?.Invoke();
            }
        }
    }
}
