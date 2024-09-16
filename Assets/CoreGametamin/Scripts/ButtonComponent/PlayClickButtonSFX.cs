using UnityEngine;

namespace Gametamin.Core
{
    public class PlayClickButtonSFX : MonoBehaviour, ITakeButtonClick, ISetButtonSFX
    {
        AudioClip _sfx;
        public void OnSetSFX(AudioClip sfx)
        {
            _sfx = sfx;
        }
        public void OnTakeButtonClick()
        {
            if (_sfx != null)
            {
                //SoundManager.PlaySingleSound(_sfx);
            }
        }
    }
}
