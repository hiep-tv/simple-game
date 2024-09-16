namespace Gametamin.Core.IAP
{
    public static class DefineSymbols
    {
        public static bool IsEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }
        public static bool IsAndroid
        {
            get
            {
#if UNITY_ANDROID
                return true;
#else
            return false;
#endif
            }
        }
        public static bool IsIOS
        {
            get
            {
#if UNITY_IOS
            return true;
#else
                return false;
#endif
            }
        }
        public static bool IsDebugIAP
        {
            get
            {
#if DEBUG_IAP
                return true;
#else
                return false;
#endif
            }
        }
    }
}