using System;
namespace Gametamin.Core.IAP
{
    public static partial class IAPController
    {
        static int _ItemRestoredCount { get; set; }
        static bool _Restoring { get; set; }
        public static void RestorePurchases(Action<bool> callback)
        {
            if (_system != null)
            {
                UserInput.Enabled = false;
                TextReferenceID.Waiting.GetTextById().ShowWaiting(() =>
                {
                    //if (Helper.HasInternetConnection)
                    {
                        _ItemRestoredCount = 0;
                        _Restoring = true;
                        _system.RestorePurchases((result, message) =>
                        {
                            0.5f.DelayCall(() =>
                            {
                                PopupHelper.HideWaiting(() =>
                                {
                                    _Restoring = false;
                                    UserInput.Enabled = true;
                                    RestoreResult(result, message, callback);
                                });
                            });
                        });
                    }
                    //else
                    {
                        PopupHelper.HideWaiting(() =>
                        //Helper.ShowNetworkError(() =>
                        callback?.Invoke(false)
                        //)
                        );
                    }
                });
            }
            else
            {
                PopupHelper.ShowConfirmPopup(TextReferenceID.RestorePurchasesFailed.GetTextById(), TextReferenceID.BuyIAPUnknownMessage.GetTextById()
                    , () => callback?.Invoke(false));
            }
        }
        static void RestoreResult(bool result, string message, Action<bool> callback)
        {
            if (result)
            {
                RestoreSuccessful(callback);
            }
            else
            {
                if (message.IsNullOrEmptySafe())
                {
                    message = TextReferenceID.BuyIAPUnknownMessage.GetTextById();
                }
                PopupHelper.ShowConfirmPopup(TextReferenceID.RestorePurchasesFailed.GetTextById()
                    , message, () => callback?.Invoke(false));
            }
        }
        static void RestoreSuccessful(Action<bool> callback)
        {
            if (_ItemRestoredCount > 0)
            {
                _ItemRestoredCount = 0;
                PopupHelper.ShowConfirmPopup(TextReferenceID.RestorePurchasesSuccessful.GetTextById(), TextReferenceID.RestorePurchasesSuccessfulMessage.GetTextById()
                    , () => callback?.Invoke(true));
            }
            else
            {
                PopupHelper.ShowConfirmPopup(TextReferenceID.RestorePurchasesFailed.GetTextById(), TextReferenceID.RestorePurchasesFailedMessage.GetTextById()
                    , () => callback?.Invoke(false));
            }
        }

    }
}