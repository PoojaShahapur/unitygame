using System.Collections.Generic;

namespace SDK.Lib
{
    //public class MDictionary<TKey, TValue> where TValue : IComparer<TValue>
    public class MDictionary<TKey, TValue>
    {
        protected Dictionary<TKey, TValue> mData;

        public MDictionary()
        {
            mData = new Dictionary<TKey, TValue>();
        }

        public Dictionary<TKey, TValue> getData()
        {
            return this.mData;
        }

        public int getCount()
        {
            return this.mData.Count;
        }

        public TValue this[TKey key]
        {
            get
            {
                return this.value(key);
            }
            set
            {
                this.Add(key, value);
            }
        }

        public TValue value(TKey key)
        {
            if (this.mData.ContainsKey(key))
            {
                return this.mData[key];
            }

            return default(TValue);
        }

        public TKey key(TValue value)
        {
            foreach (KeyValuePair<TKey, TValue> kv in this.mData)
            {
                if (kv.Value.Equals(value))
                //if (kv.Value == value)
                {
                    return kv.Key;
                }
            }
            return default(TKey);
        }

        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                return this.mData.Keys;
            }
        }

        public Dictionary<TKey, TValue> .ValueCollection Values
        {
            get
            {
                return this.mData.Values;
            }
        }

        public int Count()
        {
            return this.mData.Keys.Count;
        }

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            return this.mData.GetEnumerator();
        }

        public void Add(TKey key, TValue value)
        {
            this.mData[key] = value;
        }

        public void Remove(TKey key)
        {
            this.mData.Remove(key);
        }

        public void Clear()
        {
            this.mData.Clear();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.mData.TryGetValue(key, out value);
        }

        public bool ContainsKey(TKey key)
        {
            return this.mData.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            foreach (KeyValuePair<TKey, TValue> kv in this.mData)
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