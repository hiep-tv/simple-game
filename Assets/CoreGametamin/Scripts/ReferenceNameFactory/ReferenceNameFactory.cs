#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    public abstract partial class ReferenceNameFactory<T> : ReferenceNameFactory<T, ReferenceNameDataFactory> where T : ScriptableObject
    {
        public override string DefaultFactory => "ReferenceNameFactory";
    }
    public abstract partial class ReferenceNameFactory<T, U> : SingletonScriptableObject<T> where T : ScriptableObject where U : BaseDataFactory
    {
        [SerializeField][HideInInspector] protected UnityEngine.Object _generateFolder;
        public string PathFolder
        {
            get
            {
                if (_generateFolder != null)
                {
                    return _generateFolder.GetAssetPath();
                }
                return string.Empty;
            }
        }
        public string FactorySuffixName
        {
            get => EditorPrefs.GetString("_factory_suffix_name");
            set => EditorPrefs.SetString("_factory_suffix_name", value);
        }
        protected abstract string _referenceName { get; }
        public string ReferenceName => _referenceName;
        public abstract void Generate();
        public bool CheckAndAddName(string newName)
        {
            var exist = true;
            LoadFactories();
            if (!CheckNameExist(newName))
            {
                exist = AddName(newName);
            }
            return exist;
        }
        public bool AddName(string newName)
        {
            return AddNameInternal(newName);
        }
        protected bool AddNameInternal(string inputValue)
        {
            var isExist = false;
            var newName = ValidateName(inputValue);
            if (!newName.IsNullOrEmptySafe())
            {
                isExist = IsNameExist(newName);
                if (!isExist)
                {
                    var factory = GetSelectedFactory();
                    factory.AddName(newName, ValidateValue(inputValue));
                }
            }
            return isExist;
        }
        protected virtual string ValidateName(string name)
        {
            return name.ToCamelCase();
        }
        protected virtual string ValidateValue(string value)
        {
            return value.ToCamelCase();
        }
        public bool CheckNameExist(string newName)
        {
            var isExist = false;
            Factories.ForBreakable(item =>
            {
                isExist = item.IsNameExist(newName);
                return isExist;
            });
            return isExist;
        }
    }
}
#endif