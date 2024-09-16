using UnityEngine;
namespace Gametamin.Core
{
    public class ButtonBlockable : MonoBehaviour, IBlockable
    {
        static bool _isBlockOthers { get; set; }
        bool _isBlocker { get; set; }
        public bool IsBlocker
        {
            get
            {
                if (_isBlockOthers)
                {
                    return _isBlocker;
                }
                return true;
            }
        }
        public void OnBlockOthers()
        {
            _isBlocker = true;
            _isBlockOthers = true;
        }
        public void OnUnblockOthers()
        {
            _isBlocker = false;
            _isBlockOthers = false;
        }
    }
    public interface IBlockable
    {
        bool IsBlocker { get; }
        void OnBlockOthers();
        void OnUnblockOthers();
    }
}
