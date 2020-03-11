using System.Collections.Generic;
using UtilityExtensions;
namespace Libraries
{
    namespace Util
    {
        public class InvertibleMap<Key, Value>
        {
            private Dictionary<Key, Value> map;
            public Dictionary<Key, Value> Map { get { return map; } }
            private Dictionary<Value, IList<Key>> inverse;
            public InvertibleMap()
            {
                this.map = new Dictionary<Key, Value>();
                this.inverse = new Dictionary<Value, IList<Key>>();
            }
            public void Add(Key key, Value value)
            {
                map.Add(key, value);
                var keys = inverse.GetValueOrDefault(value, new List<Key>());
                keys.Add(key);
                inverse[value] = keys;
            }
            public Value this[Key key]
            {
                get { return this.map[key]; }
                set { this.Add(key, value); }
            }
            public IList<Key> InverseLookup(Value value)
            {
                return this.inverse[value];
            }
            public int Count { get { return map.Count; } }
        }
    }
}