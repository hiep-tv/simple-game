using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PopupHelper
    {
        public static event Action<PoolReferenceID, bool> OnPopupOpenedListener, OnPopupClosedListener;
        public static event Action<PoolReferenceID> OnStartOpenPopupListener, OnStartClosePopupListener;
        static Stack<PoolReferenceID> _popupStacks = new();
        public static PoolReferenceID TopPopupStack
        {
            get
            {
                if (_popupStacks.TryPeek(out PoolReferenceID result))
                {
                    //Debug.Log($"top popup={result}, cout={_popupStacks.Count} ");
                    return result;
                }
                return PoolReferenceID.Non;
            }
        }
        public static GameObjectReference PlayHidePopupAnimation(this GameObject popup, Action onCompleted = null)
        {
            var @ref = popup.GetComponentSafe<GameObjectReference>();
            return @ref.PlayHidePopupAnimation(onCompleted);
        }
        public static GameObjectReference HidePopupTemporary(this GameObjectReference @ref, Action onCompleted = null)
        {
            @ref.PlayHideAnimation(() =>
            {
                @ref.SetActiveSafe(false);
                var cacher = @ref.GetCacheValue(0);
                if (cacher != null)
                {
                    var poolId = (PoolReferenceID)cacher.Value;
                    //Debug.Log($"HidePopupTemporary={poolId}");
                    OnPopupClosedListener?.Invoke(poolId, false);
                }
                onCompleted?.Invoke();
            });
            return @ref;
        }
        public static GameObjectReference PlayHidePopupAnimation(this GameObjectReference @ref, Action onCompleted = null, bool hideOnCompleted = true)
        {
            if (!@ref.IsNullSafe())
            {
                var cacher = @ref.GetCacheValue(0);
                var poolId = (PoolReferenceID)cacher.Value;
                if (!Except(poolId))
                {
                    RemoveTopPopupStack(true);
                }
                @ref.RemoveKeyListenning();
                OnStartClosePopupListener?.Invoke(poolId);
                @ref.PlayHideAnimation(() =>
                {
                    if (hideOnCompleted)
                    {
                        @ref.SetActiveSafe(false);
                    }
                    //Debug.Log($"HidePopup={poolId}");
                    OnPopupClosedListener?.Invoke(poolId, true);
                    onCompleted?.Invoke();
                });
            }
            else
            {
                onCompleted?.Invoke();
            }
            return @ref;
        }
        public static GameObjectReference PlayShowPopupAnimation(this GameObject popup, Action onCompleted = null)
        {
            var @ref = popup.GetComponentSafe<GameObjectReference>();
            return @ref.PlayShowPopupAnimation(onCompleted);
        }
        public static GameObjectReference ReopenPopupAnimation(this GameObjectReference @ref, Action onCompleted = null)
        {
            @ref.SetAsLastSiblingSafe();
            @ref.SetActiveSafe(true);
            @ref.PlayShowAnimation(() =>
            {
                var cacher = @ref.GetCacheValue(0);
                var poolId = (PoolReferenceID)cacher.Value;
                //Debug.Log($"ReopenPopup={poolId}");
                OnPopupOpenedListener?.Invoke(poolId, false);
                onCompleted?.Invoke();
            });
            return @ref;
        }
        public static GameObjectReference PlayShowPopupAnimation(this GameObjectReference @ref, Action onCompleted = null)
        {
            if (!@ref.IsNullSafe())
            {
                var cacher = @ref.GetCacheValue(0);
                var poolId = (PoolReferenceID)cacher.Value;
                @ref.RegisterKeyListenning();
                @ref.SetAsLastSiblingSafe();
                @ref.SetActiveSafe(true);
                if (!Except(poolId))
                {
                    @ref.AddTracking(poolId);
                }
                @ref.PlayShowAnimation(() =>
                {
                    //Debug.Log($"ShowPopup={poolId}");
                    OnPopupOpenedListener?.Invoke(poolId, true);
                    onCompleted?.Invoke();
                });
            }
            else
            {
                onCompleted?.Invoke();
            }
            return @ref;
        }
        public static bool BroadcastStartOpenPopup(this PoolReferenceID poolId)
        {
            var canBroadcast = !Except(poolId);
            if (canBroadcast)
            {
                AddTopPopupStack(poolId);
                //Debug.Log($"Push={poolId}, cout={_popupStacks.Count}");
                OnStartOpenPopupListener?.Invoke(poolId);
            }
            return canBroadcast;
        }
        public static void AddTopPopupStack(PoolReferenceID referenceID)
        {
            //Debug.Log($"Push={referenceID}, cout={_popupStacks.Count}");
            _popupStacks.Push(referenceID);
        }
        public static void RemoveTopPopupStack(bool removeTracking = false)
        {
            _popupStacks.TryPop(out PoolReferenceID result);
            if (removeTracking)
            {
                RemoveTracking(result);
            }
            //Debug.Log($"Pop={result}, cout={_popupStacks.Count}");
        }
        public static void Release()
        {
            _popupStacks.Clear();
            ClearTracking();
            OnStartOpenPopupListener = null;
            OnStartClosePopupListener = null;
            OnPopupOpenedListener = null;
            OnPopupClosedListener = null;
        }
        static bool Except(PoolReferenceID poolReferenceID)
            => poolReferenceID == PoolReferenceID.Currency
            || poolReferenceID == PoolReferenceID.IngameBackground
            || poolReferenceID == PoolReferenceID.Rewardstack
            || poolReferenceID == PoolReferenceID.Dialouge_event
            || poolReferenceID == PoolReferenceID.Notice
            || poolReferenceID == PoolReferenceID.Waiting
            || poolReferenceID == PoolReferenceID.ViewReward
            || poolReferenceID == PoolReferenceID.ChallengeChecklist
            || poolReferenceID == PoolReferenceID.OrderContainer
            || poolReferenceID == PoolReferenceID.CollectJumpItem
            || poolReferenceID == PoolReferenceID.SkipReplaySimulation
            || poolReferenceID == PoolReferenceID.Specialorderreward
            ;
    }
}
