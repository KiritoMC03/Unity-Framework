using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Rect = General.CSV.Rect;


namespace General.Editor
{
    public class CSVConvertor
    {
        #region Fields

        private const char TrimChars = '_';

        #endregion

        #region Methods

        internal AssemblyData GetTypes(Data data)
        {
            List<DataType> dataTypes = new List<DataType>();
            Dictionary<string, (int, Type)> types = new Dictionary<string, (int, Type)>();
            string pattern = @"\d+";
            Regex rgx = new Regex(pattern);
            foreach (List<string> item in data.GetData)
            {
                string name = rgx.Split(item[0])[0];
                name = name.TrimEnd(TrimChars);
                if (types.ContainsKey(name))
                {
                    types.TryGetValue(name, out (int, Type) num);
                    num.Item1++;
                    types[name] = num;
                }
                else
                {
                    types.Add(name, (1, GetCurrentType(item[1])));
                }
            }

            foreach (KeyValuePair<string, (int, Type)> item in types)
                dataTypes.Add(new DataType(GetCurrentType(item.Value.Item2, item.Value.Item1), item.Key));

            return new AssemblyData(dataTypes, data.Headers);
        }


        internal List<List<string>> GetData(TextAsset textAsset, Rect rectData)
        {
            string d = "\"";
            string pattern = $@"({d}\d+)(,)(\d+{d})";
            string replaced = Regex.Replace(textAsset.text, pattern, m => m.Groups[1].Value + '|' + m.Groups[3].Value);
            string[] text = replaced.Replace(d, "").Replace(",,", "")
                .Split('\n');
            if (rectData.EndLine == -1) rectData.EndLine = text.Length;
            List<List<string>> list = new List<List<string>>();
            if (rectData.EndColum == -1) rectData.EndColum = text[rectData.StartLine].Split(',').Length;

            for (int i = rectData.StartLine; i < rectData.EndLine; i++)
            {
                list.Add(new List<string>());
                string[] subText = text[i].Split(',');
                for (int j = rectData.StartColumn; j < rectData.EndColum; j++)
                    list[i - rectData.StartLine].Add(subText[j]);
            }

            foreach (List<string> line in list)
                for (int i = 0; i < line.Count; i++)
                {
                    line[i] = line[i].Trim('/', '\\', '\a', '\b', '\r', '\f', '\v', '\t', '.', '*');
                    line[i] = line[i].Replace("|", ",");
                }

            return list;
        }

        internal List<string> GetHeader(TextAsset textAsset, Rect rectData)
        {
            List<string> headers = GetData(textAsset, rectData)[0];
            for (int index = 0; index < headers.Count; index++)
                headers[index] = headers[index].Replace(" ", "")
                    .Trim('/', '\\', '\a', '\b', '\n', '\r', '\f', '\v', '\t', ',', '.', '*', '|');

            return headers;
        }

        private Type GetCurrentType(string value)
        {
            if (int.TryParse(value, out int num) && !value.Contains(","))
                return num.GetType();
            if (float.TryParse(value, out float numf))
                return numf.GetType();
            if (bool.TryParse(value, out bool boolean))
                return boolean.GetType();
            return value.GetType();
        }

        private Type GetCurrentType(Type type, int count)
        {
            if (count > 1)
            {
                if (type == typeof(int))
                    return typeof(int[]);
                if (type == typeof(float))
                    return typeof(float[]);
                if (type == typeof(bool))
                    return typeof(bool[]);
                if (type == typeof(string))
                    return typeof(string[]);
            }

            return type;
        }

        #endregion
    }
}