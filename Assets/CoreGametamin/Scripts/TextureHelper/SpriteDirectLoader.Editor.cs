#if UNITY_EDITOR
using UnityEngine;

namespace Gametamin.Core
{
    public partial class SpriteDirectLoader : IForceLoadEditorMode
    {
        [ContextMenu("Load")]
        public void ForceLoadSprite()
        {

        }
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                _Loader.SetLoadDirect(true);
            }
        }
        private void OnDestroy()
        {
            if (!Application.isPlaying)
            {
                _Loader.SetLoadDirect(false);
            }
        }
    }
}
#endif