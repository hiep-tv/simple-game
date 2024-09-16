using System;
using UnityEngine;

namespace Gametamin.Core
{
    [Serializable]
    public class GameObjectReferenceData
    {
        [SerializeField][GameObjectReferenceID] string _id;
        [SerializeField] GameObject _target;
        [SerializeField] bool _ignoreCopy;
        public bool IgnoreCopy
        {
            get => _ignoreCopy;
            set => _ignoreCopy = value;
        }
        public GameObjectReferenceData()
        {

        }
        public GameObjectReferenceData(string id, GameObject target)
        {
            _id = id;
            _target = target;
        }
        public GameObjectReferenceData(GameObjectReferenceData other)
        {
            _id = other.Id;
            _target = other.Target;
        }
        public void SetData(string id, GameObject target)
        {
            _id = id;
            _target = target;
        }
        public string Id => _id;
        public GameObject Target
        {
            get => _target;
            set => _target = value;
        }
    }
}
