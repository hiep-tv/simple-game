using UnityEngine;

namespace Gametamin.Core
{
    public interface ISetSprite
    {
        void OnSetSprite(Sprite texture);
        Sprite OnGetSprite();
    }
}
