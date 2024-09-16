#if UNITY_EDITOR
namespace Gametamin.Core
{
    public class SpriteAtlasFactory : ReferenceNameFactory<SpriteAtlasFactory, SpriteAtlasDataFactory>
    {
        public override string DefaultFactory => "SpriteAtlasDataFactory";

        protected override string _referenceName => "";

        public override void Generate()
        {
            //TODO do nothing
        }
        public void AddAtlasConfig(AtlasConfig item, string label, string group)
        {
            var factory = GetSelectedFactory();
            factory.AddAtlasConfig(item, label, group);
        }
        public void MarkAddressable()
        {
            LoadFactories();
            Factories.For(item => item.MarkAddressable());
        }
    }
}
#endif