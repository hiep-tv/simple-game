#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace Gametamin.Core
{
    public abstract partial class ReferenceNameFactory<T, U>
    {
        public abstract string DefaultFactory { get; }

        List<U> _factories = new();
        public List<U> Factories
        {
            get
            {
                if (_factories.GetCountSafe() == 0)
                {
                    LoadFactories();
                }
                return _factories;
            }
        }
        public int FactorySelectedIndex
        {
            get;
            set;
        }
        public void LoadFactories(Action<U> callback = null)
        {
            _factories.SafeClear();
            var path = this.GetAssetDirectoryPath();
            AssetDatabaseHelper.FindAssetsDatabase<U>(path
                , resut =>
                {
                    _factories.Add(resut);
                    callback?.Invoke(resut);
                });
        }
        public U GetSelectedFactory()
        {
            var selectedFactory = GetFactoryBySuffixName();
            if (selectedFactory == null)
            {
                if (!FactorySuffixName.IsNullOrEmptySafe())
                {
                    string itemName = $"{DefaultFactory}.{FactorySuffixName}";
                    var path = this.GetAssetDirectoryPath();
                    selectedFactory = path.CreateScriptableObject<U>(itemName);
                    Factories.Add(selectedFactory);
                }
                else
                {
                    selectedFactory = Factories.GetSafe(0);
                }
            }
            return selectedFactory;
        }
        U GetFactoryBySuffixName()
        {
            U result = default;
            Factories.For(factory =>
            {
                var name = factory.name.Split(".").GetSafe(1);
                if (name.EqualsSafe(FactorySuffixName))
                {
                    result = factory;
                }
            });
            return result;
        }
        bool IsNameExist(string newName)
        {
            var exist = false;
            Factories.ForBreakable(factory =>
            {
                exist = factory.IsNameExist(newName);
                return exist;
            });
            return exist;
        }
    }
}
#endif