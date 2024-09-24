using UnityEngine;

namespace UnnamedGame
{
    [RequireComponent(typeof(Camera))]
    public class AspectRatioCameraFitter : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        private void Start()
        {
            float unitsPerPixel = 10.8f / Screen.width;

            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            _camera.orthographicSize = desiredHalfHeight;
        }
    }
}
