//using UnityEngine;

//namespace Gametamin.Core
//{
//    public partial class CustomImageComponent : MonoBehaviour, ISetMaterial, ISetSprite
//    {
//        [SerializeField] UIMeshBehaviour _source;
//        [SerializeField] bool _nativeSize;
//        UIMeshBehaviour _Source => gameObject.GetComponentSafe(ref _source);
//        Material _Material;
//        public void OnSetMaterial(Material material)
//        {
//            if (material != null)
//            {
//                _Material = _Source.material;
//                _Source.material = material;
//            }
//            else
//            {
//                _Source.material = _Material;
//            }
//        }
//        public void OnSetSprite(Sprite texture)
//        {
//            _Source.sprite = texture;
//        }
//        public Sprite OnGetSprite()
//        {
//            return _Source.sprite;
//        }
//    }
//}