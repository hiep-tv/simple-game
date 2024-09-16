using System;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PopupBuilder
    {
        /// <summary>
        /// callback
        /// - true if reopen popup, otherwise create new one
        /// </summary>
        public static void ShowOrCreatePopup(this PoolConfigData configs, Action<GameObjectReference, bool> callback = null)
        {
            ShowOrCreatePopup(configs, PoolHelper.ParentPopup, callback);
        }
        /// <summary>
        /// callback
        /// - true if reopen popup, otherwise create new one
        /// </summary>
        public static void ShowOrCreatePopup(this PoolConfigData configs, Transform parent, Action<GameObjectReference, bool> callback = null)
        {
            //Debug.Log($"configs.PoolId={configs.PoolId}");
            var blockInput = configs.PoolId.BroadcastStartOpenPopup();
            var popup = PoolHelper.GetGameObject(configs.PoolId);
            if (popup.IsNullSafe())
            {
                if (blockInput)
                {
                    UserInput.Enabled = false;
                }
                CreatePopup(configs, parent, iref =>
                {
                    callback?.Invoke(iref, false);
                });
            }
            else
            {
                callback?.Invoke(popup.GetComponentSafe<GameObjectReference>(), true);
            }
        }
        static void CreatePopup(this PoolConfigData poolData, Transform parent, Action<GameObjectReference> callback = null)
        {
            poolData.AddressableLabel.LoadAddressableAssetAsync<PopupData>(handler =>
            {
                if (handler.Status.OperationHandleSucceeded())
                {
                    var popupData = handler.Result;
                    CreatePopup(popupData, parent, iref =>
                    {
                        if (iref != null)
                        {
                            //LoadFonts(iref.gameObject, AddressableLabels.Font_asset, () => callback?.Invoke(iref));
                            PoolHelper.AddGameObject(poolData.PoolId, iref.gameObject);
                            iref.CacheValue(poolData.PoolId, 0);
                            var backKey = iref.GetBackKey();
                            if (!backKey.IsNullOrEmptySafe())
                            {
                                iref.AddKeyListener(backKey);
                            }
                            handler.ReleaseAddressablesAsset();
                            callback?.Invoke(iref);
                        }
                        else
                        {
                            callback?.Invoke(iref);
                        }
                    }, poolData.PoolId.ToString());
                }
                else
                {
                    handler.ReleaseAddressablesAsset();
                    callback?.Invoke(null);
                }
            });
        }
        static string GetBackKey(this GameObjectReference @ref)
        {
            var backKey = GameObjectReferenceID.ButtonClose;
            var ibutton = @ref.GetGameObjectReference(backKey);
            if (ibutton.IsNullSafe())
            {
                backKey = GameObjectReferenceID.ButtonCloseOutside;
                ibutton = @ref.GetGameObjectReference(backKey);
                if (ibutton.IsNullSafe())
                {
                    backKey = GameObjectReferenceID.Empty;
                }
            }
            return backKey;
        }
        static void CreatePopup(PopupData config, Transform parentCanvas, Action<GameObjectReference> callback = null, string popupName = null)
        {
            var iref = CreatePopup(config, parentCanvas, popupName);
            callback?.Invoke(iref);
        }
        static GameObjectReference CreatePopup(PopupData config, Transform parentCanvas, string popupName = null)
        {
            var popupObject = config.Popup.PoolItem(parentCanvas);
            if (!string.IsNullOrEmpty(popupName))
            {
                popupObject.name = popupName;
            }
            if (config.PoolDatas.GetCountSafe() > 0)
            {
                var setPools = popupObject.GetOrAddComponentSafe<PoolGameObjectReference>();
                setPools.OnSetPool(config.PoolDatas);
            }
            var rootRef = popupObject.GetComponentSafe<GameObjectReference>();
            if (rootRef != null)
            {
                var datas = config.PopupParts;
                datas.For(data =>
                {
                    var parent = rootRef.GetTransformReference(data.ParentID) ?? rootRef.transform;
                    var item = data.Target.PoolItem(parent);
                    var childRef = item.GetComponentSafe<GameObjectReference>();
                    if (childRef != null)
                    {
                        var copyToRoot = childRef.CopyType;
                        if (copyToRoot != GameObjectReferenceCopyType.ChildrenOnly)
                        {
                            rootRef.OnSetReference(data.Id, item);
                        }
                        if (copyToRoot != GameObjectReferenceCopyType.Itself)
                        {
                            rootRef.OnAddReferences(childRef);
                        }
                    }
                    else
                    {
                        if (!data.Id.EqualsSafe(GameObjectReferenceID.Empty))
                        {
                            rootRef.OnSetReference(data.Id, item);
                        }
                    }
                });
                //if (config.AnimationData != null)
                //{
                //    var ianimation = popupObject.GetOrAddComponentSafe<PlayAnimationGroup>();
                //    ianimation.AnimationData = config.AnimationData;
                //}
                //else
                //{
                //    var ianimation = popupObject.GetComponentSafe<IPlayAnimation>();
                //    if (ianimation != null)
                //    {
                //        popupObject.GetOrAddComponentSafe<PlayAnimationGroup>();
                //    }
                //}
            }
            return rootRef;
        }
    }
}
