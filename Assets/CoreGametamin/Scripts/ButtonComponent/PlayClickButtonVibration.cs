using UnityEngine;

namespace Gametamin.Core
{
    public class PlayClickButtonVibration : MonoBehaviour, ITakeButtonClick
    {
        public void OnTakeButtonClick()
        {
            //TODO play sfx
            Debug.Log("play vibration");
        }
    }
}
