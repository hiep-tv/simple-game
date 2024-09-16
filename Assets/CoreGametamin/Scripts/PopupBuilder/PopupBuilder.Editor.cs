#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace Gametamin.Core
{
    public static partial class PopupBuilder
    {
        public static void CreatePopupEditorMode(this PopupData popupData)
        {
            var iref = CreatePopupEditorMode(popupData, PoolHelper.ParentPopup, "");
            Selection.activeGameObject = iref.gameObject;
        }
        public static GameObjectReference CreatePopupEditorMode(PopupData config, Transform parentCanvas, string popupName = null)
        {
            var popupObject = config.Popup.PoolItem(parentCanvas);
            if (!string.IsNullOrEmpty(popupName))
            {
                popupObject.name = popupName;
            }
            var rootRef = popupObject.GetComponentSafe<GameObjectReference>();
            if (rootRef != null)
            {
                var datas = config.PopupParts;
                //var iadd = popupObject.GetComponentSafe<IAddObjetHasAnimation>();
                datas.For(data =>
                {
                    var parent = rootRef.GetTransformReference(data.ParentID) ?? rootRef.transform;
                    var item = data.Target.PoolItem(parent);
                    if (parent == null)
                    {
                        item.SetupCanvasCamera();
                    }
                    var ichildRef = item.GetComponentSafe<GameObjectReference>();
                    if (ichildRef != null)
                    {
                        var copyToRoot = ichildRef.CopyType;
                        if (copyToRoot != GameObjectReferenceCopyType.ChildrenOnly)
                        {
                            rootRef.OnSetReference(data.Id, item);
                        }
                        if (copyToRoot != GameObjectReferenceCopyType.Itself)
                        {
                            rootRef.OnAddReferences(ichildRef);
                        }
                    }
                    else
                    {
                        if (!data.Id.EqualsSafe(GameObjectReferenceID.Empty))
                        {
                            rootRef.OnSetReference(data.Id, item);
                        }
                    }
                    //if (iadd != null && Application.isPlaying)
                    //{
                    //    var ianimation = GetComponentSafe<IShowHideAnimation>(item);
                    //    if (ianimation != null)
                    //    {
                    //        iadd.OnAdd(item);
                    //    }
                    //}
                });
            }
            //var childs = GetComponentsInChildrenSafe<ITextureName>(popupObject, true);
            //childs.For(item => LoadTextureFromAtlas(item));
            //var directs = GetComponentsInChildrenSafe<TexureDirectLoader>(popupObject, true);
            //directs.For(item => item.LoadTexture());
            return rootRef;
        }
        public static GameObject GetCopyPopupPart(this PopupPartCopyType copyType, GameObject target, string folder, string name)
        {
            return copyType switch
            {
                PopupPartCopyType.Non => default,
                PopupPartCopyType.Use => target,
                PopupPartCopyType.Variant => target.CreateVariantsPrefab(folder, name),
                PopupPartCopyType.Duplicate => target.CreateCopyPrefab(folder, name),
                _ => default,
            };
        }
    }
}
#endif