#if UNITY_EDITOR
using UnityEditor;

namespace Gametamin.Core
{
    [CustomEditor(typeof(SpriteDirectLoader))]
    public class SpriteDirectLoaderEditor : AtlasConfigEditor
    {
        protected override bool _ShowSearchGUI => false;
        protected override bool _ShowEditableGUI => false;
        protected override bool _Editable => true;
        SpriteLoader _Loader;
        SpriteDirectLoader _DirectLoader;
        private void Awake()
        {
            _DirectLoader = (SpriteDirectLoader)serializedObject.targetObject;
            _Loader = _DirectLoader.GetComponentSafe<SpriteLoader>();
            _Loader.AddAtlasChangeListener(AtlasNameChanged);
        }
        void AtlasNameChanged(string atlasName)
        {
            //Debug.Log(atlasName);
        }
        protected override void OnCustomGUI()
        {

        }
        private void OnDestroy()
        {
            _Loader.RemoveAtlasChangeListener(AtlasNameChanged);
        }
    }
}
#endif