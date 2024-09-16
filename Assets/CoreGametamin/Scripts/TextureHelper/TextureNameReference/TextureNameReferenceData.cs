using System;
using UnityEngine;

namespace Gametamin.Core
{
    [Serializable]
    public class TextureNameReferenceData
    {
        [SerializeField][TextureNameReferenceID] string _id;
        [SerializeField] string _textureName;
        public string Id { get => _id; set => _id = value; }
        public string TextureName => _textureName;
        public TextureNameReferenceData(string id, string textureName)
        {
            _id = id;
            _textureName = textureName;
        }
    }
}