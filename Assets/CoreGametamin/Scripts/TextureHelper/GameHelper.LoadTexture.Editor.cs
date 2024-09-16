#if UNITY_EDITOR
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class LoadTextureHelper
    {
        //static bool _clickedCopy;
        //static string _atlasName = string.Empty;
        public static void GUILoadAndClearTexture(GameObject target)
        {
            EditorGUIHelper.HorizontalLayout(() =>
            {
                EditorGUIHelper.GUIButton("Clear Texture", () =>
                {
                    var childs = target.GetComponentsInChildrenSafe<SpriteLoader>(true);
                    childs.For(item => item.SetSprite(default));
                });
                EditorGUIHelper.GUIButton("Load Texture", () =>
                {
                    var childs = target.GetComponentsInChildrenSafe<SpriteLoader>(true);
                    childs.For(item => item.ForceLoadSprite());
                    //TODO direct load
                    //var directs = EditorGUIHelper.GetComponentsInChildrenSafe<TexureDirectLoader>(target, true);
                    //directs.For(item => item.LoadTexture());
                });
                //_clickedCopy = EditorGUIHelper.GUIStringWithSearch("Atlas Label", _atlasName, AddressableLabels.Values, _clickedCopy
                //        , (value, index) => _atlasName = value, 80f);

                //EditorGUIHelper.GUIButton("Update Atlas Name", () =>
                //{
                //    if (_atlasName.SafeContains("_atlas"))
                //    {
                //        var childs = EditorGUIHelper.GetComponentsInChildrenSafe<ITextureName>(target, true);
                //        childs.For(item => item.AtlasName = _atlasName);
                //    }
                //});
            });
        }
        public static Sprite GetSprite(this string spriteName)
        {
            return AtlasEditorSettings.GetSprite(spriteName);
        }
    }
}
#endif