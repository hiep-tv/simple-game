namespace Gametamin.Core
{
    public enum AttributeType
    {
        Non,
        AtlasName,
        PopupData,
        GameObjectReferrentID,
        TextureNameReferrentID
    }
    public interface IAttributeValueChanged
    {
        void OnValueChanged(AttributeType attributeType);
    }
}
