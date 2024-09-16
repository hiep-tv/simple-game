using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Gametamin.Core
{
    public interface ISetMaterial
    {
        void OnSetMaterial(Material material);
    }
    [RequireComponent(typeof(Image))]
    public partial class ImageComponent : MonoBehaviour, ISetMaterial, ISetSprite
    {
        [SerializeField] Image _source;
        [SerializeField] bool _nativeSize;
        Image _Source => gameObject.GetComponentSafe(ref _source);
        Material _Material;
        public void OnSetMaterial(Material material)
        {
            if (material != null)
            {
                _Material = _Source.material;
                _Source.material = material;
            }
            else
            {
                _Source.material = _Material;
            }
        }
        public void OnSetSprite(Sprite texture)
        {
            _Source.sprite = texture;
            if (_nativeSize)
            {
                _Source.SetNativeSize();
            }
        }

        public Sprite OnGetSprite()
        {
            return _Source.sprite;
        }
    }
}