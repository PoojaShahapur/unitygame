using System.Collections.Generic;

namespace SDK.Lib
{
    public class MDictionary<TKey, TValue> where TValue : IComparer<TValue>
    {
        protected Dictionary<TKey, TValue> m_data;

        public MDictionary()
        {
            m_data = new Dictionary<TKey, TValue>();
        }

        public Dictionary<TKey, TValue> getData()
        {
            return this.m_data;
        }

        public int getCount()
        {
            return this.m_data.Count;
        }

        public TValue value(TKey key)
        {
            if (this.m_data.ContainsKey(key))
            {
                return this.m_data[key];
            }

            return default(TValue);
        }

        public TKey key(TValue value)
        {
            foreach (KeyValuePair<TKey, TValue> kv in this.m_data)
            {
                if (kv.Value.Equals(value))
                //if (kv.Value == value)
                {
                    return kv.Key;
                }
            }
            return default(TKey);
        }

        public void Add(TKey key, TValue value)
        {
            this.m_data[key] = value;
        }

        public void Remove(TKey key)
        {
            this.m_data.Remove(key);
        }

        public void Clear()
        {
            this.m_data.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            return this.m_data.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            foreach (KeyValuePair<TKey, TValue> kv in this.m_data)
            {
                if (kv.Value.Equals(value))
                //if (kv.Value == value)
                {
                    return true;
                }
            }
        
            return false;
        }
    }
}