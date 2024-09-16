#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public interface IForceLoadEditorMode
    {
        void ForceLoadSprite();
    }
}
#endif