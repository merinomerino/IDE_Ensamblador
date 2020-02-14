using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityExtensions
{
    //https://stackoverflow.com/a/53940423
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultVal)
          => dict.TryGetValue(key, out var value) ? value : defaultVal;
    }
}
