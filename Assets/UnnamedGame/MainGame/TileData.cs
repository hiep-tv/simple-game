using System;
using UnityEngine;

namespace UnnamedGame
{
    [Serializable]
    public struct TileData
    {
        public static int DefaultValue = 0;
        public static bool IsDefaultValue(int value) => value == DefaultValue;

        [SerializeField] TileType _type;
        [SerializeField] char _value;
        public char Value { get => _value; set => _value = value; }
        public TileType Type => _type;

        public bool IsDefaultValue() => Value == DefaultValue;

        public TileData(TileType type, char value)
        {
            _value = value;
            _type = type;
        }
        public override string ToString()
        {
            return $"(TileType={_type}, Value={Value})";
        }
    }
    public static partial class GameHelper
    {
        public static bool Mergable(this TileData tileData, TileData other)
        {
            return tileData.Type == other.Type
                && tileData.Value == other.Value;
        }
        public static TileData IncreaseValue(this TileData tileData)
        {
            var maxValue = tileData.Type.GetMaxChar();
            tileData.Value = (char)Mathf.Min(tileData.Value + 1, maxValue);
            return tileData;
        }
        static char GetMaxChar(this TileType tileType)
            => tileType switch
            {
                TileType.Numeric => '9',
                TileType.UpperCase => 'Z',
                TileType.LowerCase => 'z',
                _ => default,
            };
    }
}
