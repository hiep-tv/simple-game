#if PURCHASING_DISABLED
#define SERVICES_DISABLED
#endif
using System;
#if !SERVICES_DISABLED
using Unity.Services.Core;
using Unity.Services.Core.Environments;
#endif

namespace Gametamin.Core.IAP
{
    public static class InitializeGamingServices
    {
#if SERVICES_DISABLED
        public static void Initialize(Action onSuccess, Action<string> onError = null)
        {
            onSuccess?.Invoke();
        }
#else
        const string _environment = "production";
        public static async void Initialize(Action onSuccess, Action<string> onError = null)
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                try
                {
                    var options = new InitializationOptions().SetEnvironmentName(_environment);
                    await UnityServices.InitializeAsync(options);//.ContinueWith(task => onSuccess());
                    onSuccess?.Invoke();
                }
                catch (Exception exception)
                {
                    onError(exception.Message);
                }
            }
            else
            {
                onSuccess?.Invoke();
            }
        }
#endif
    }
}