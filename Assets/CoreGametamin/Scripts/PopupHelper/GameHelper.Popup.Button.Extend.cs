using System;

namespace Gametamin.Core
{
    public static partial class PopupHelper
    {
        public static GameObjectReference SetMainButtonEventLayer2(this GameObjectReference @ref, string parentId, Action onClick = null)
        {
            var parent = @ref.GetGameObjectReference(parentId);
            return parent.SetMainButtonEvent(onClick);
        }
        public static GameObjectReference SetLeftButtonEventLayer2(this GameObjectReference @ref, string parentId, Action onClick = null)
        {
            var parent = @ref.GetGameObjectReference(parentId);
            return parent.SetLeftButtonEvent(onClick);
        }
        public static GameObjectReference SetRightButtonEventLayer2(this GameObjectReference @ref, string parentId, Action onClick = null)
        {
            var parent = @ref.GetGameObjectReference(parentId);
            return parent.SetRightButtonEvent(onClick);
        }
    }
}
