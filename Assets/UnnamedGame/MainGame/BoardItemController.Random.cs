using UnityEngine;

namespace UnnamedGame
{
    public partial class BoardItemController : MonoBehaviour
    {
        static TileData GetTileData()
        {
            var random = Random.Range(0, 9);
            if (random <= 2)
            {
                return new TileData(TileType.Numeric, '1');
            }
            if (random <= 6)
            {
                return new TileData(TileType.UpperCase, 'A');
            }
            return new TileData(TileType.LowerCase, 'a');
        }
    }
}
