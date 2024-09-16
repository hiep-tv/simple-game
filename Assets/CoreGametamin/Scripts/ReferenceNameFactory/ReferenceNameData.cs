using System;
using UnityEngine;

namespace Gametamin.Core
{
    [Serializable]
    public class ReferenceNameData : IEquatable<ReferenceNameData>
    {
        [SerializeField] string _name;
        [SerializeField] string _value;
        public ReferenceNameData(string name, string @value)
        {
            _name = name;
            _value = @value;
        }
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(_value))
                {
                    return _name;
                }
                return _value;
            }
            set => _value = value;
        }
        public bool Equals(ReferenceNameData other)
        {
            if (other != null)
            {
                return Name.EqualsSafe(other.Name, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
