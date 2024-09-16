using UnityEngine;
namespace Gametamin.Core
{
    public static class DetectResolution
    {
        public const float EPSILON = 0.005f;
        const float baseScaleFactor = 9f / 16f;
        static bool IsCheck { get; set; } = false;
        static void CaculateResolution()
        {
            if (!IsCheck)
            {
                IsCheck = true;
                float aspect = Screen.width / (float)Screen.height;
                ScaleFactor(aspect);
            }
        }
        static float _uiscaleFactor, _orthographicSize;
        public static float UIScaleFactor
        {
            get
            {
                CaculateResolution();
                return _uiscaleFactor;
            }
            private set => _uiscaleFactor = value;
        }
        public static float OrthographicSize
        {
            get
            {
                CaculateResolution();
                return _orthographicSize;
            }
            private set => _orthographicSize = value;
        }

        static void ScaleFactor(float aspect)
        {
            float factor = baseScaleFactor / aspect;
            //OrthographicSize = (Screen.height / 1080f) + 1920 / 100f;
            OrthographicSize = Mathf.Max(1f, factor) * 1920f / 200f;
            UIScaleFactor = Mathf.Clamp01(factor);
        }
    }

}