using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gametamin.Core
{
    public static partial class AddressabelAssetHelper
    {
        public static bool OperationHandleSucceeded(this AsyncOperationStatus status)
        {
            return status == AsyncOperationStatus.Succeeded;
        }
        public static void LoadGameObjectAddressable(this AssetReference assetReference, Action<GameObject> onCompleted, Transform parent = null)
        {
            LoadAddressableAssetAsync<GameObject>(assetReference, resultHandler =>
            {
                if (resultHandler.Status.OperationHandleSucceeded())
                {
                    var targetObject = resultHandler.Result.PoolItem(parent);
                    onCompleted?.Invoke(targetObject);
                }
                else
                {
                    onCompleted?.Invoke(new GameObject("NewGameObject"));
                }
                ReleaseAddressablesAsset(resultHandler);
            });
        }
        public static void LoadPrefabAddressable(this GameObjectReference iref, AssetReference assetReference, Action<GameObject> onCompleted)
        {
            LoadAddressableAssetAsync<GameObject>(assetReference, resultHandler =>
            {
                if (resultHandler.Status.OperationHandleSucceeded())
                {
                    iref.AddDestroyListener(() => resultHandler.ReleaseAddressablesAsset());
                    onCompleted?.Invoke(resultHandler.Result);
                }
                else
                {
                    ReleaseAddressablesAsset(resultHandler);
                    onCompleted?.Invoke(new GameObject("NewGameObject"));
                }
            });
        }
        public static void LoadGameObjectAddressable(this string key, Action<GameObject> onCompleted, Transform parent = null)
        {
            LoadAddressableAssetAsync<GameObject>(key, resultHandler =>
            {
                if (resultHandler.Status.OperationHandleSucceeded())
                {
                    var targetObject = resultHandler.Result.PoolItem(parent);
                    onCompleted?.Invoke(targetObject);
                }
                else
                {
                    onCompleted?.Invoke(new GameObject(key));
                }
                ReleaseAddressablesAsset(resultHandler);
            });
        }
        public static void LoadPrefabAddressable(this GameObjectReference iref, string key, Action<GameObject> onCompleted)
        {
            LoadAddressableAssetAsync<GameObject>(key, resultHandler =>
            {
                if (resultHandler.Status.OperationHandleSucceeded())
                {
                    iref.AddDestroyListener(() => resultHandler.ReleaseAddressablesAsset());
                    onCompleted?.Invoke(resultHandler.Result);
                }
                else
                {
                    ReleaseAddressablesAsset(resultHandler);
                    onCompleted?.Invoke(new GameObject(key));
                }
            });
        }
        public static void InitializeAddressableAssetAsync(Action onCompleted = null)
        {
            //LoadingTracker.StartTrack("Addressables");
            var handler = Addressables.InitializeAsync(true);
            handler.Completed += OnCompleted;
            void OnCompleted(AsyncOperationHandle<IResourceLocator> result)
            {
                //LoadingTracker.EndTrack("Addressables");
                onCompleted?.Invoke();
            }
        }
        public static void GetDownloadSizeAsync(this string key, Action<float> callback)
        {
            //if (key.Contains("roll"))
            //{
            //    Debug.Log($"key={key}");
            //}
            var handler = Addressables.GetDownloadSizeAsync(key);
            handler.Completed += OnCompleted;
            void OnCompleted(AsyncOperationHandle<long> result)
            {
                callback?.Invoke(result.Result);
            }
        }
        public static AsyncOperationHandle DownloadDependenciesAsync(this string key, Action<bool> callback = null)
        {
            //if (key.Contains("roll"))
            //{
            //    Debug.Log($"key={key}");
            //}
            var handler = Addressables.DownloadDependenciesAsync(key, true);
            handler.Completed += OnCompleted;
            void OnCompleted(AsyncOperationHandle result)
            {
                callback?.Invoke(result.Status.OperationHandleSucceeded());
            }
            return handler;
        }
        public static void LoadAddressableAssetAsync<T>(this string key, Action<AsyncOperationHandle<T>> onCompleted = null)
        {
            //if (key.Contains("roll"))
            //{
            //    Debug.Log($"key={key}");
            //}
            var handler = Addressables.LoadAssetAsync<T>(key);
            handler.Completed += OnCompleted;
            void OnCompleted(AsyncOperationHandle<T> result)
            {
                if (onCompleted != null)
                {
                    onCompleted.Invoke(result);
                }
                else
                {
                    ReleaseAddressablesAsset(handler);
                }
            }
        }
        public static void LoadAddressableAssetAsync<T>(this AssetReference assetReference, Action<AsyncOperationHandle<T>> onCompleted = null)
        {
            var handler = Addressables.LoadAssetAsync<T>(assetReference);
            handler.Completed += OnCompleted;
            void OnCompleted(AsyncOperationHandle<T> result)
            {
                if (onCompleted != null)
                {
                    onCompleted.Invoke(result);
                }
                else
                {
                    ReleaseAddressablesAsset(handler);
                }
            }
        }
        public static AsyncOperationHandle<IList<T>> LoadAddressableAssetsAsync<T>(this List<string> keys, Action<T> callback = null, Action onCompleted = null, Addressables.MergeMode mergeMode = Addressables.MergeMode.Intersection)
        {
            //foreach (var key in keys)
            //{
            //    Debug.Log($"key={key}");
            //}
            var handler = Addressables.LoadAssetsAsync(keys, callback, mergeMode, true);
            handler.Completed += OnCompleted;
            void OnCompleted(AsyncOperationHandle<IList<T>> result)
            {
                if (onCompleted != null)
                {
                    onCompleted.Invoke();
                }
                else
                {
                    ReleaseAddressablesAsset(handler);
                }
            }
            return handler;
        }
        public static void ReleaseAddressablesAsset<T>(this AsyncOperationHandle<T> handler)
        {
            if (handler.IsValid())
            {
                Addressables.Release(handler);
            }
        }
        public static void ReleaseAddressablesAsset(this AsyncOperationHandle handler)
        {
            if (handler.IsValid())
            {
                Addressables.Release(handler);
            }
        }
        public static void ReleaseAddressablesAsset(this AssetReference assetReference)
        {
            if (assetReference.IsValid())
            {
                Addressables.Release(assetReference);
            }
        }
        public static void ReleaseAddressablesAsset<T>(this AsyncOperationHandle<IList<T>> handler)
        {
            if (handler.IsValid())
            {
                Addressables.Release(handler);
            }
        }
        public static void CheckExistKey()
        {
#if UNITY_EDITOR || DEBUG_MODE
            Debug.Log("====Start CheckExistKey=====");
            var keyCount = AddressableLabels.Values.Length;
            GetSize(keyCount);
            void GetSize(int count)
            {
                if (count > 0)
                {
                    var key = AddressableLabels.Values[count - 1];
                    if (HasKey(key))
                    {
                        GetDownloadSizeAsync(key
                            , size =>
                            {
                                if (size > 0f)
                                {
                                    Debug.Log($"Need download: key={key}, size={size / 1000000f} (mb)");
                                }
                                else
                                {
                                    Debug.Log($"key={key} exist!");
                                }
                                GetSize(count - 1);
                            });
                    }
                    else
                    {
                        GetSize(count - 1);
                    }
                }
                else
                {
                    Debug.Log("====End CheckExistKey=====");
                }

            }
#endif
        }
        static bool HasKey(string key)
        {
            foreach (var l in Addressables.ResourceLocators)
            {
                if (l.Locate(key, typeof(object), out _))
                {
                    return true;
                }
            }
            return false;
        }
    }
}