using TMPro;
using UnityEngine;
namespace Gametamin.Core
{
    public class TextMeshComponent : BaseTextComponent
    {
        [SerializeField] TMP_Text _textComponent;
        TMP_Text _TextComponent => gameObject.GetComponentSafe(ref _textComponent);
        public TMP_Text TextMesh => _TextComponent;
        public override string Text { get => _TextComponent.text; set => _TextComponent.text = value; }
#if UNITY_EDITOR
        protected override void LoadText(string text)
        {
            base.LoadText(text);
            UnityEditor.EditorUtility.SetDirty(_textComponent);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(_textComponent);
        }
#endif
    }
}