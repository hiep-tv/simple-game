#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Gametamin.Core
{
    public partial class PopupPoolDataFactory : ReferenceNameFactory<PopupPoolDataFactory, PoolDataFactory>
    {
        [SerializeField] List<PoolConfigData> _poolConfigs;
        //[SerializeField] UnityEngine.Object _generateFolder;
        protected override string _referenceName => "PoolConfig";

        public override string DefaultFactory => "PoolDataFactory";

        public static void AddPopup(PoolConfigData configData, bool generate = true)
        {
            Instance.AddPopupInternal(configData, generate);
        }
        void AddPopupInternal(PoolConfigData configData, bool generate = true)
        {
            if (!IsPopupExist(configData))
            {
                _poolConfigs.Add(configData);
                Save();
                if (generate)
                {
                    GenerateInternal();
                }
            }
        }
        public override void Generate()
        {
            GenerateInternal();
        }
        [ContextMenu("Generate")]
        void GenerateInternal()
        {
            GeneratePoolConfigs(_poolConfigs, _generateFolder.GetAssetPath());
            AddressableNameFactory.Instance.Generate();
            PoolNameReference.Instance.Generate();
        }
        bool IsPopupExist(PoolConfigData configData)
        {
            var exist = false;
            _poolConfigs.ForBreakable(popup =>
            {
                if (configData.PopupName.EqualsSafe(popup.PopupName))
                {
                    exist = true;
                }
                return exist;
            });
            return exist;
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
    }
}
#endif