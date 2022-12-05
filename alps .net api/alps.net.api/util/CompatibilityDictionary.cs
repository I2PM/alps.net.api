using System.Collections.Generic;

namespace alps.net.api.util
{
    public class CompatibilityDictionary<K, V> : Dictionary<K, V>, ICompatibilityDictionary<K, V>
    {
        new public bool TryAdd(K key, V value)
        {
#if NET48
            if (!this.ContainsKey(key)){
                this.Add(key, value);
                return this.ContainsKey(key);
            }
            else return false;
#else
            return base.TryAdd(key, value);
#endif

        }
    }
}
