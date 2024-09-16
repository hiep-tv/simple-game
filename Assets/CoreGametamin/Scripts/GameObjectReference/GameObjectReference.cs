using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gametamin.Core
{
    public enum GameObjectReferenceCopyType
    {
        Itself,// only itself
        All,//all children and itself
        ChildrenOnly,// only children
    }
    [DisallowMultipleComponent]
    public partial class GameObjectReference : MonoBehaviour
    {
        [SerializeField] GameObjectReferenceCopyType _copyType;
        public GameObjectReferenceCopyType CopyType { get => _copyType; set => _copyType = value; }
        [SerializeField] List<GameObjectReferenceData> _datas;
        public List<GameObjectReferenceData> Datas => _datas;
        public T OnGetComponent<T>(string id)
        {
            var resultGameObject = OnGetGameObject(id);
            return resultGameObject.GetComponentSafe<T>();
        }
        public Transform OnGetTransform(string id)
        {
            var resultGameObject = OnGetGameObject(id);
            if (resultGameObject != null)
            {
                return resultGameObject.transform;
            }
            return default;
        }
        public bool HasGameObject(string id)
        {
            return IsExistID(id);
        }
        public GameObject OnGetGameObject(string id)
        {
            GameObject result = default;
            var exist = false;
            _datas.ForBreakable(item =>
            {
                exist = id.Equals(item.Id);
                if (exist)
                {
                    if (item.Target == null)
                    {
                        //Assert.LogError($"ID {id} attached on {name} found but gameObject is missing!");
                    }
                    result = item.Target;
                }
                return exist;
            });
            if (!exist)
            {
                //Assert.LogError($"No gameObject has id {id} attached on {name}!");
            }
            return result;
        }
        public void OnAddReference(GameObjectReferenceData referenceData)
        {
            if (IsExistID(referenceData.Id, out GameObjectReferenceData itemData))
            {
                //Assert.LogError($"referenceData {itemData.Id} is exist with {itemData.Target.name}");
            }
            else
            {
                _datas.Add(referenceData);
            }
        }
        public void OnAddReferences(GameObjectReference other)
        {
            OnAddReferences(other.Datas);
        }
        public void OnAddReferences(List<GameObjectReferenceData> datas)
        {
            //_datas.AddRange(datas);
            datas.For(item =>
            {
                if (!item.IgnoreCopy)
                {
                    _datas.Add(item);
                }
            });
        }
        public void OnSetReferences(GameObjectReference other)
        {
            OnSetReferences(other.Datas);
        }
        public void OnSetReferences(List<GameObjectReferenceData> datas)
        {
            //datas.For(data => OnSetReference(data));
            datas.For(data =>
            {
                if (!data.IgnoreCopy)
                {
                    OnSetReference(data);
                }
            });
        }
        public void OnSetReference(string id, GameObject target)
        {
            if (IsExistID(id, out GameObjectReferenceData itemData))
            {
                itemData.Target = target;
            }
            else
            {
                _datas.Add(new GameObjectReferenceData(id, target));
            }
        }
        public void OnSetReference(GameObjectReferenceData data)
        {
            if (IsExistID(data.Id, out int indexData))
            {
                _datas.SetSafe(data, indexData);
            }
            else
            {
                _datas.Add(data);
            }
        }
        bool IsExistID(string id, out GameObjectReferenceData itemData)
        {
            var exist = false;
            GameObjectReferenceData result = default;
            _datas.ForBreakable(item =>
            {
                exist = id.EqualsSafe(item.Id);
                if (exist)
                {
                    if (item.Target == null)
                    {
                        //Assert.LogError($"ID {id} attached on {name} found but gameObject is missing!");
                    }
                    result = item;
                }
                return exist;
            });
            itemData = result;
            return exist;
        }
        bool IsExistID(string id, out int indexData)
        {
            var exist = false;
            var indexResult = -1;
            _datas.ForBreakable((item, index) =>
            {
                exist = id.EqualsSafe(item.Id);
                if (exist)
                {
                    if (item.Target == null)
                    {
                        //Assert.LogError($"ID {id} attached on {name} found but gameObject is missing!");
                    }
                    indexResult = index;
                }
                return exist;
            });
            indexData = indexResult;
            return exist;
        }
        bool IsExistID(string id)
        {
            var exist = false;
            _datas.ForBreakable((item, index) =>
            {
                exist = id.EqualsSafe(item.Id);
                return exist;
            });
            return exist;
        }
        public void OnAddReference(string id, GameObject target)
        {
            OnAddReference(new GameObjectReferenceData(id, target));
        }
        public void OnGetGameObjects(string id, Action<GameObject> callback)
        {
            _datas.For(item =>
            {
                if (id.EqualsSafe(item.Id))
                {
                    callback?.Invoke(item.Target);
                }
            });
        }
        public void OnGetGameObjects(string id, Action<GameObject, int> callback)
        {
            var index = 0;
            _datas.For(item =>
            {
                if (id.EqualsSafe(item.Id))
                {
                    callback?.Invoke(item.Target, index++);
                }
            });
        }
    }
}
