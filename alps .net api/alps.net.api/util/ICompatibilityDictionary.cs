using System.Collections.Generic;

namespace alps.net.api.util
{
    public interface ICompatibilityDictionary<K,V> : IDictionary<K,V>
    {
        public new bool TryAdd(K key, V value);
    }
}
