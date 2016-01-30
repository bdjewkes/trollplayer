
using System.Collections.Generic;

namespace SpryFox.Common {
    public static class DictionaryExtensions {
        public static V GetDefault<K, V>(this IDictionary<K, V> dictionary, K key, V def) {
            V value;
            return dictionary.TryGetValue(key, out value) ? value : def;
        }
    }
}