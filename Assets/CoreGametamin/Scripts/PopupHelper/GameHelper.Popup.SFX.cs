using MergeGame;
using UnityEngine;

namespace Gametamin.Core
{
    public static partial class PopupHelper
    {
        public static AudioClip ClosePopupSFX
        {
            get
            {
                //#if DATA_ENABLED
                //                return UISoundID.ClosePopup.GetSound();
                //#else
                //                return AudioManager.Instance.GetClipWName(AudioClipName.Generic_Popup_Close.ToString());
                //#endif
                return default;
            }
        }
        public static AudioClip LeftButtonSFX
        {
            get
            {
#if DATA_ENABLED
                return UISoundID.ClosePopup.GetSound();
#else
                return default;
#endif
            }
        }
        public static AudioClip RightButtonSFX
        {
            get
            {
#if DATA_ENABLED
                return UISoundID.ClosePopup.GetSound();
#else
                return default;
#endif
            }
        }
        public static AudioClip MainButtonSFX
        {
            get
            {
#if DATA_ENABLED
                return UISoundID.ClosePopup.GetSound();
#else
                return default;
#endif
            }
        }
    }
}
