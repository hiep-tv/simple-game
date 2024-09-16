#if UNITY_EDITOR
using UnityEngine;

namespace Gametamin.Core
{
    public class GameObjectReferenceNameFactory : ReferenceNameFactory<GameObjectReferenceNameFactory>
    {
        protected override string _referenceName => "GameObjectReferenceID";
    }
}
#endif