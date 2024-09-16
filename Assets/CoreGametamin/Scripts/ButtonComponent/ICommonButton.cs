using System;

namespace Gametamin.Core
{
    public interface ICommonButton : IClickButton, IGetGameObject
    {
        void OnAddPreListener(Action onClick, bool force = false);
        void OnRemovePreListener(Action onClick);
        void OnAddListener(Action onClick, ClickButtonEventType clickType);
        void OnAddListener(Action onClick);
        void OnAddLastListener(Action onClick);
    }
}
