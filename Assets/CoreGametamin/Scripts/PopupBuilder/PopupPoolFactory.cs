#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class PopupPoolFactory : ReferenceNameFactory<PopupPoolFactory, PoolDataFactory>
    {
        List<PoolConfigData> _poolConfigs;
        List<PoolConfigData> _PoolConfigs => _poolConfigs ??= new();
        protected override string _referenceName => "PoolConfig";

        public override string DefaultFactory => "PoolDataFactory";

        public static void AddPopup(PoolConfigData configData)
        {
            Instance.AddPopupInternal(configData);
        }
        void AddPopupInternal(PoolConfigData configData)
        {
            if (!CheckNameExist(configData.PopupName))
            {
                var factory = GetSelectedFactory();
                factory.AddPopup(configData);
            }
        }
        public override void Generate()
        {
            LoadFactories();
            _PoolConfigs.SafeClear();
            Factories.For(item =>
            {
                var datas = item.PoolConfigs;
                GetUniqueName(datas, _PoolConfigs);
            });
            GeneratePoolConfigs(_PoolConfigs, _generateFolder.GetAssetPath());
            Save();
        }
        void GetUniqueName(List<PoolConfigData> source, List<PoolConfigData> destination)
        {
            source.For(item =>
            {
                var exits = false;
                destination.ForBreakable((item2, index) =>
                {
                    exits = item2.PopupName.EqualsSafe(item.PopupName);
                    return exits;
                });
                if (!exits)
                {
                    destination.Add(item);
                }
            });
        }
        public static void GeneratePoolConfigs(List<PoolConfigData> poolConfigs, string path)
        {
            StringBuilder _builder = new StringBuilder(path);
            var className = "PoolConfig";
            _builder.AppendFormat($"/{className}.cs");
            using (StreamWriter streamWriter = new StreamWriter(_builder.ToString()))
            {
                streamWriter.WriteLine("//Auto generate. Do not edit!");
                streamWriter.WriteLine("namespace Gametamin.Core");
                streamWriter.WriteLine("{");
                streamWriter.WriteLine("\tpublic static class " + className);
                streamWriter.WriteLine("\t{");
                // streamWriter.WriteLine("\t" + "Non" + " = -1,");
                for (int i = 0, max = poolConfigs.GetCountSafe(); i < max; i++)
                {
                    var data = poolConfigs[i];
                    char[] arrChars = $"_{data.PopupName}".ToCharArray();
                    arrChars[1] = char.ToLower(arrChars[1]);
                    var fieldName = new string(arrChars);
                    streamWriter.WriteLine($"\t\tstatic PoolConfigData {fieldName}" + ";");
                    arrChars = data.AddressableLabel.ToCharArray();
                    arrChars[0] = char.ToUpper(arrChars[0]);
                    streamWriter.WriteLine($"\t\tpublic static PoolConfigData {data.PopupName} = {fieldName} ??= new(PoolReferenceID.{data.PoolIdString}, AddressableLabels.{new string(arrChars)});");
                }
                streamWriter.WriteLine("\t}");
                streamWriter.WriteLine("}");
            }
            _builder.Clear();
            _builder.Append($"{path}{className}.cs");
            //filePath = _builder.ToString();
            _builder.Clear();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [ContextMenu("LoadPopup")]
        public void LoadPopup()
        {
            LoadFactories();
            Factories.For(item => item.LoadPopup());
        }
        public void MarkAddressable()
        {
            LoadFactories();
            Factories.For(item => item.MarkAddressable());
        }
        public void AddPopup(PopupData popupData, string label)
        {
            var factory = GetSelectedFactory();
            factory.AddPopup(popupData, label);
        }
    }
}
#endif