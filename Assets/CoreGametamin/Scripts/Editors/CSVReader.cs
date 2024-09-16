using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.IO;
namespace Gametamin.Core
{
    public static class CSVReader
    {
        static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r|\\n";
        static string LINE_SPLIT_RE2 = @"\r\n|\n\r|\n|\r";
        static char[] TRIM_CHARS = { '\"', '"' };

        public static List<Dictionary<string, object>> Read(string file)
        {
            string path = Application.persistentDataPath + "/Resources/" + file + ".csv";
            string fileContents = "";
            path = path.Replace("\\", "/");

#if READ_LOCAL_CSV
        TextAsset data = Resources.Load(file) as TextAsset;
        fileContents = data.text;
#else
            if (File.Exists(path))
            {
                var sourse = new StreamReader(path);
                fileContents = sourse.ReadToEnd();
                sourse.Close();
            }
            else
            {
                TextAsset data = Resources.Load(file) as TextAsset;
                if (data != null)
                    fileContents = data.text;
            }
#endif
            return GetListFromContent(fileContents);
        }

        public static List<Dictionary<string, object>> GetListFromContent(string fileContents)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            var lines = Regex.Split(fileContents, LINE_SPLIT_RE);

            if (lines.Length <= 1)
                return list;

            var header = Regex.Split(lines[0], SPLIT_RE);
            for (var j = 0; j < header.Length; j++)
            {
                string value = header[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                header[j] = value;
                //Debug.Log (value);
            }

            for (var i = 1; i < lines.Length; i++)
            {
                var values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length == 0 || values[0] == "")
                    continue;

                var entry = new Dictionary<string, object>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    if (value.Equals(""))
                    {
                        continue;
                    }

                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    object finalvalue = value;
                    int n;
                    float f;
                    if (int.TryParse(value, out n))
                    {
                        finalvalue = n;
                    }
                    else if (float.TryParse(value, out f))
                    {
                        finalvalue = f;
                    }

                    entry[header[j]] = finalvalue;
                }

                list.Add(entry);
            }

            return list;
        }

        //-->| Override accept empty value
        public static List<Dictionary<string, object>> Read(string file, bool _canEmptyValue = false)
        {
            string path = Application.persistentDataPath + "/Resources/" + file + ".csv";
            string fileContents = "";
            path = path.Replace("\\", "/");

#if READ_LOCAL_CSV
        TextAsset data = Resources.Load(file) as TextAsset;
        fileContents = data.text;
#else
            if (File.Exists(path))
            {
                var sourse = new StreamReader(path);
                fileContents = sourse.ReadToEnd();
                sourse.Close();
            }
            else
            {
                TextAsset data = Resources.Load(file) as TextAsset;
                if (data != null)
                    fileContents = data.text;
            }
#endif
            return GetListFromContent(fileContents, _canEmptyValue);
        }

        //
        private static List<Dictionary<string, object>> GetListFromContent(string fileContents, bool _canEmptyValue = false)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            var lines = Regex.Split(fileContents, LINE_SPLIT_RE);


            if (lines.Length <= 1)
                return list;

            var header = Regex.Split(lines[0], SPLIT_RE);
            for (var j = 0; j < header.Length; j++)
            {
                string value = header[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                header[j] = value;
                //            Debug.Log("value" + value);
            }

            for (var i = 1; i < lines.Length; i++)
            {
                var values = Regex.Split(lines[i], SPLIT_RE);
                if ((values.Length == 0 || values[0] == ""))
                    continue;

                var entry = new Dictionary<string, object>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    if (value.Equals("") && !_canEmptyValue)
                    {
                        continue;
                    }

                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    object finalvalue = value;
                    int n;
                    float f;
                    if (int.TryParse(value, out n))
                    {
                        finalvalue = n;
                    }
                    else if (float.TryParse(value, out f))
                    {
                        finalvalue = f;
                    }

                    entry[header[j]] = finalvalue;
                }

                list.Add(entry);
            }

            return list;
        }

        //--<| Override accept empty value


        public static List<Dictionary<string, object>> GetListFromContent3(string fileContents)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            var lines = Regex.Split(fileContents, LINE_SPLIT_RE2);

            if (lines.Length <= 1)
                return list;

            var header = Regex.Split(lines[0], SPLIT_RE);
            for (var j = 0; j < header.Length; j++)
            {
                string value = header[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                header[j] = value;
            }

            for (var i = 1; i < lines.Length; i++)
            {
                var values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length == 0 || values[0] == "")
                    continue;

                var entry = new Dictionary<string, object>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    if (value.Equals(""))
                    {
                        continue;
                    }
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\n", "\n");
                    object finalvalue = value;
                    int n;
                    float f;
                    if (int.TryParse(value, out n))
                    {
                        finalvalue = n;
                    }
                    else if (float.TryParse(value, out f))
                    {
                        finalvalue = f;
                    }

                    entry[header[j]] = finalvalue;
                }

                list.Add(entry);
            }

            return list;
        }
    }
}
