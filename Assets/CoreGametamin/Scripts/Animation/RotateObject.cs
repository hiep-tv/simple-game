using UnityEngine;
namespace Gametamin.Core.IAP
{
    public class RotateObject : MonoBehaviour
    {
        [SerializeField]
        Transform targetTransform;
        [SerializeField]
        float speed;
        private void Start()
        {
            if (targetTransform == null)
            {
                targetTransform = transform;
            }
        }
        private void Update()
        {
            Vector3 rotate = targetTransform.eulerAngles;
            rotate.z += speed * Time.deltaTime;
            targetTransform.eulerAngles = rotate;
        }
    }

}