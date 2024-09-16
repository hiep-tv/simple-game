#if UNITY_EDITOR
using System.IO;
using UnityEditor;
namespace Gametamin.Core
{
    public static class ClassGenerator
    {
        public static void GenerateEnumClass(string className, string[] displayNames, string[] values, string path)
        {
            var builder = path.GetBuilder();
            builder.AppendFormat($"/{className}.cs");
            var fullpath = builder.GetValueInBuilder();
            //UnityEngine.Debug.Log($"class fullpath={fullpath}");
            using (StreamWriter streamWriter = new StreamWriter(fullpath))
            {
                streamWriter.WriteLine("//Auto generate. Do not edit!");
                streamWriter.WriteLine("public enum " + className);
                streamWriter.WriteLine("{");
                //streamWriter.WriteLine("\t" + "Non" + " = 0,");
                for (int i = 0; i < displayNames.Length; i++)
                {
                    var value = values[i];
                    if (value.IsNullOrEmptySafe())
                    {
                        value = "Non";
                    }
                    streamWriter.WriteLine("\t" + value + $" = {i},");
                }
                streamWriter.WriteLine("}");
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        public static void GenerateStaticClass(string className, string[] displayNames, string[] values, string path, string groupName = default)
        {
            var builder = path.GetBuilder();
            if (groupName.IsNullOrEmptySafe())
            {
                builder.AppendFormat($"/{className}.cs");
            }
            else
            {
                builder.AppendFormat($"/{className}.{groupName}.cs");
            }
            var fullpath = builder.GetValueInBuilder();
            //UnityEngine.Debug.Log($"class fullpath={fullpath}");
            using (StreamWriter streamWriter = new StreamWriter(fullpath))
            {
                streamWriter.WriteLine("//Auto generate. Do not edit!");
                streamWriter.WriteLine("public static partial class " + className);
                streamWriter.WriteLine("{");
                for (int i = 0; i < displayNames.Length; i++)
                {
                    var entryName = displayNames[i].Replace(" ", "").Replace("(", "_").Replace(")", "");
                    if (char.IsDigit(entryName[0]))
                    {
                        entryName = $"_{entryName}";
                    }
                    char[] arrayChars = entryName.ToCharArray();
                    arrayChars[0] = char.ToUpper(arrayChars[0]);
                    streamWriter.WriteLine($"\tpublic const string {new string(arrayChars)}" + $" = \"{values[i]}\";");
                }
                streamWriter.WriteLine("}");
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        static string Values = "Values";
        public static void GenerateStaticClassValues(string className, string[] values, string path, string groupName = default)
        {
            var builder = path.GetBuilder();
            if (groupName.IsNullOrEmptySafe())
            {
                groupName = Values;
                builder.Append($"/{className}.{groupName}.cs");
            }
            else
            {
                groupName = $"{groupName}{Values}";
                builder.Append($"/{className}.{groupName}.cs");
            }
            var fullpath = builder.GetValueInBuilder();
            //UnityEngine.Debug.Log($"Values fullpath={fullpath}");
            using (StreamWriter streamWriter = new StreamWriter(fullpath))
            {
                streamWriter.WriteLine("//Auto generate. Do not edit!");
                streamWriter.WriteLine("#if UNITY_EDITOR || DEBUG_MODE");
                streamWriter.WriteLine("public static partial class " + className);
                streamWriter.WriteLine("{");
                streamWriter.WriteLine($"\tpublic static string[] {groupName} =");
                streamWriter.WriteLine("\t{");
                for (int i = 0; i < values.Length; i++)
                {
                    streamWriter.WriteLine($"\t\t\"{values[i]}\",");
                }
                streamWriter.WriteLine("\t};");

                streamWriter.WriteLine("}");
                streamWriter.WriteLine("#endif");
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif