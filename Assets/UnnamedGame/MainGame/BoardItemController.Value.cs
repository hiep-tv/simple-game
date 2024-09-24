using UnityEngine;

namespace UnnamedGame
{
    public partial class BoardItemController : MonoBehaviour
    {
        static char NextChar(char current)
        {
            current++;
            return current;
        }
    }
}
