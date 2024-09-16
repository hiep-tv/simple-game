using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Gametamin.Core
{
    public class TextMeshLinkHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Camera _camera;
        private TMP_Text _textComponent;
        private TMP_Text _TextComponent => gameObject.GetComponentSafe(ref _textComponent);
        Action<string> _onLinkClicked;
        public void AddListener(Camera camera, Action<string> onLinkClicked)
        {
            _camera = camera;
            _onLinkClicked = onLinkClicked;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointerClick");
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(_TextComponent, Input.mousePosition, _camera);
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = _TextComponent.textInfo.linkInfo[linkIndex];
                var linId = linkInfo.GetLinkID();
                Debug.Log($"clicked={linId}");
                _onLinkClicked?.Invoke(linId);
            }
        }
    }
}
