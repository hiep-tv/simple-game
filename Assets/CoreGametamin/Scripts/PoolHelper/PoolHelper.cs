using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gametamin.Core
{
    public static partial class PoolHelper
    {
        public interface IRelease
        {
            void OnRelease();
        }
        static List<IRelease> _releases;
        static List<IRelease> _Releases => _releases ??= new List<IRelease>();
        static Camera _cameraUI;
        public static Camera CameraUI
        {
            get
            {
                if (_cameraUI == null || _cameraUI.gameObject == null)
                {
                    _cameraUI = UnityEngine.Object.FindObjectOfType<MainSceneObject>().GetComponentReference<Camera>(GameObjectReferenceID.CameraUI);
                    if (_cameraUI == null)
                    {
                        _cameraUI = Camera.main;
                    }
                }
                return _cameraUI;
            }
        }
        static Transform _canvasObject;
        public static Transform ParentPopup
        {
            get
            {
                if (_canvasObject == null || _canvasObject.gameObject == null)
                {
                    _canvasObject = CreateCanvasScreenSpaceCamera().transform;
                }
                return _canvasObject;
            }
        }
        const string _UI = "UI", _CanvasUIName = "CanvasUI";
        public static Canvas CreateCanvasScreenSpaceCamera(string name = _CanvasUIName)
        {
            var newGameObject = new GameObject(name)
            {
                layer = LayerMask.NameToLayer(_UI)
            };
            var canvas = newGameObject.GetOrAddComponentSafe<Canvas>();
            var scaler = newGameObject.GetOrAddComponentSafe<CanvasScaler>();
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            newGameObject.GetOrAddComponentSafe<GraphicRaycaster>();
            canvas.worldCamera = CameraUI;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.sortingLayerID = SortingLayer.NameToID(_UI);
            canvas.sortingOrder = 10;
            canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord2;
            return canvas;
        }
        static BasePoolManager<T> GetPoolManager<T>()
        {
            var instance = new BasePoolManager<T>();
            _Releases.Add(instance);
            return instance;
        }
        public static void Release()
        {
            _releases.For(instance => instance.OnRelease(), false);
            _CacheValues.Clear();
            _CacheItems.Clear();
        }
    }
}
