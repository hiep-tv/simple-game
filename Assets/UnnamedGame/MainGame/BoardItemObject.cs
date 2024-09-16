using System;
using DG.Tweening;
using Gametamin.Core;
using UnityEngine;

namespace UnnamedGame
{
    public class BoardItemObject : MonoBehaviour
    {
        [SerializeField] GameObject[] _tiles;
        [SerializeField] TileData _tileDatas;
        public void Construct(TileData tileDatas)
        {
            _tileDatas = tileDatas;
        }
    }
}
