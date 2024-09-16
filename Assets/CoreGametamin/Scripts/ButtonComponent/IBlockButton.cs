namespace Gametamin.Core
{
    public interface IBlockButton : IGetGameObject
    {
        void OnBlockOthers();
        void OnUnblockOthers();
    }
}
