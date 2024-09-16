using UnityEngine;
using UnityEngine.UI;
namespace Gametamin.Core
{
    public class TextComponent : BaseTextComponent
    {
        [SerializeField]
        Text _textComponent;
        [SerializeField]
        Text _TextComponent
        {
            get
            {
                if (_textComponent == null)
                {
                    _textComponent = GetComponent<Text>();
                }
                return _textComponent;
            }
        }
        public override string Text { get => _TextComponent.text; set => _TextComponent.text = value; }
    }
}
