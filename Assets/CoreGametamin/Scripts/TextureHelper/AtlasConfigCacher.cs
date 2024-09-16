using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gametamin.Core
{
    public static partial class AtlasConfigCacher
    {
        static List<string> _keys;
        static List<string> _Keys => _keys ??= new List<string>();
        static Dictionary<string, AtlasConfig> _atlasConfig;
        static Dictionary<string, AtlasConfig> _AtlasConfig => _atlasConfig ??= new();
        static Dictionary<string, AsyncOperationHandle<AtlasConfig>> _handler;
        static Dictionary<string, AsyncOperationHandle<AtlasConfig>> _Handler => _handler ??= new();
        static Dictionary<string, AsyncOperationHandle<IList<AtlasConfig>>> _handlerVariant;
        static Dictionary<string, AsyncOperationHandle<IList<AtlasConfig>>> _HandlerVariant => _handlerVariant ??= new();
        static Dictionary<string, Action<AtlasConfig>> _atlasConfigListeners;
        static Dictionary<string, Action<AtlasConfig>> _AtlasConfigListeners => _atlasConfigListeners ??= new();
        static List<string> _clearOnLoadSceneLabels;
        static List<string> _ClearOnLoadSceneLabels => _clearOnLoadSceneLabels ??= new();
        public static void GetAtlasConfig(string atlasAddressable, Action<AtlasConfig> callback)
        {
            //if (atlasAddressable.Contains("roll"))
            //{
            //    UnityEngine.Debug.Log($"atlasAddressable={atlasAddressable}");
            //}
            if (_AtlasConfig.ContainsKey(atlasAddressable))
            {
                callback?.Invoke(_AtlasConfig[atlasAddressable]);
            }
            else
            {
                var isLoading = _AtlasConfigListeners.ContainsKey(atlasAddressable);
                AddAtlasConfigListener(atlasAddressable, callback);
                if (!isLoading)
                {
                    AddressabelAssetHelper.LoadAddressableAssetAsync<AtlasConfig>(atlasAddressable, handler =>
                    {
                        var listenner = _AtlasConfigListeners[atlasAddressable];
                        _AtlasConfigListeners[atlasAddressable] = null;
                        _AtlasConfigListeners.Remove(atlasAddressable);
                        if (handler.Status.OperationHandleSucceeded())
                        {
                            var atlas = handler.Result;
                            if (!atlas.Immortal)
                            {
                                _ClearOnLoadSceneLabels.Add(atlasAddressable);
                            }
                            if (!_AtlasConfig.ContainsKey(atlasAddressable))
                            {
                                _AtlasConfig.Add(atlasAddressable, handler.Result);
                            }
                            if (!_Handler.ContainsKey(atlasAddressable))
                            {
                                _Handler.Add(atlasAddressable, handler);
                            }
                            listenner?.Invoke(atlas);
                        }
                        else
                        {
                            handler.ReleaseAddressablesAsset();
                            listenner?.Invoke(default);
                        }
                    });
                }
            }
        }
        public static void GetAtlasVariantConfig(this string atlasAddressable, string atlasAddressableVariant, Action<AtlasConfig> callback)
        {
            //if (atlasAddressable.Contains("roll"))
            //{
            //    UnityEngine.Debug.Log($"{atlasAddressable}");
            //}
            //if (atlasAddressableVariant.Contains("roll"))
            //{
            //    UnityEngine.Debug.Log($"{atlasAddressableVariant}");
            //}
            if (_AtlasConfig.ContainsKey(atlasAddressable))
            {
                callback?.Invoke(_AtlasConfig[atlasAddressable]);
            }
            else
            {
                var isLoading = _AtlasConfigListeners.ContainsKey(atlasAddressable);
                AddAtlasConfigListener(atlasAddressable, callback);
                if (!isLoading)
                {
                    _Keys.Clear();
                    _Keys.Add(atlasAddressable);
                    _Keys.Add(atlasAddressableVariant);
                    AsyncOperationHandle<IList<AtlasConfig>> handler = default;
                    handler = AddressabelAssetHelper.LoadAddressableAssetsAsync<AtlasConfig>(_Keys
                       , atlas =>
                       {
                           if (!_AtlasConfig.ContainsKey(atlasAddressable))
                           {
                               var listenner = _AtlasConfigListeners[atlasAddressable];
                               _AtlasConfigListeners[atlasAddressable] = null;
                               _AtlasConfigListeners.Remove(atlasAddressable);
                               if (!atlas.Immortal)
                               {
                                   _ClearOnLoadSceneLabels.Add(atlasAddressable);
                               }
                               _AtlasConfig.Add(atlasAddressable, atlas);
                               listenner?.Invoke(atlas);
                           }
                       }, () =>
                       {
                           if (!_HandlerVariant.ContainsKey(atlasAddressable))
                           {
                               _HandlerVariant.Add(atlasAddressable, handler);
                           }
                       });
                }
            }
        }
        static void AddAtlasConfigListener(string atlasName, Action<AtlasConfig> callback)
        {
            if (_AtlasConfigListeners.ContainsKey(atlasName))
            {
                _AtlasConfigListeners[atlasName] += callback;
            }
            else
            {
                _AtlasConfigListeners.Add(atlasName, callback);
            }
        }
        public static void ClearCache()
        {
            _ClearOnLoadSceneLabels.For(atlasAddressable =>
            {
                ClearCache(atlasAddressable);
            }, false);
        }
        public static void ClearCache(string atlasAddressable)
        {
            if (_Handler.ContainsKey(atlasAddressable))
            {
                _Handler[atlasAddressable].ReleaseAddressablesAsset();
                _AtlasConfig.Remove(atlasAddressable);
            }
            if (_HandlerVariant.ContainsKey(atlasAddressable))
            {
                _HandlerVariant[atlasAddressable].ReleaseAddressablesAsset();
                _HandlerVariant.Remove(atlasAddressable);
            }
        }
    }
}
