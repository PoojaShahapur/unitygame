using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 对系统 List 的封装
     */
    public class MList<T>
    {
        //public delegate int CompareFunc(T left, T right);

        protected List<T> mList;
        protected int m_uniqueId;       // 唯一 Id ，调试使用

        public MList()
        {
            mList = new List<T>();
        }

        public MList(int capacity)
        {
            mList = new List<T>(capacity);
        }

        public T[] ToArray()
        {
            return mList.ToArray();
        }

        public List<T> list()
        {
            return mList;
        }

        public int uniqueId
        {
            get
            {
                return m_uniqueId;
            }
            set
            {
                m_uniqueId = value;
            }
        }

        public List<T> buffer
        {
            get
            {
                return mList;
            }
        }

        public int size
        {
            get
            {
                return mList.Count;
            }
        }

        public void Add(T item)
        {
            mList.Add(item);
        }

        // 主要是 Add 一个 float 类型的 Vector3
        public void Add(T item_1, T item_2, T item_3)
        {
            mList.Add(item_1);
            mList.Add(item_2);
            mList.Add(item_3);
        }

        // 主要是 Add 一个 float 类型的 UV
        public void Add(T item_1, T item_2)
        {
            mList.Add(item_1);
            mList.Add(item_2);
        }

        // 主要是 Add 一个 byte 类型的 Color32
        public void Add(T item_1, T item_2, T item_3, T item_4)
        {
            mList.Add(item_1);
            mList.Add(item_2);
            mList.Add(item_3);
            mList.Add(item_4);
        }

        public void push(T item)
        {
            mList.Add(item);
        }

        public bool Remove(T item)
        {
            return mList.Remove(item);
        }

        public T this[int index]
        {
            get
            {
                return mList[index];
            }
            set
            {
                mList[index] = value;
            }
        }

        public void Clear()
        {
            mList.Clear();
        }

        public int Count()
        {
            return mList.Count;
        }

        public int length()
        {
            return mList.Count;
        }

        public void setLength(int value)
        {
            mList.Capacity = value;
        }

        public void RemoveAt(int index)
        {
            mList.RemoveAt(index);
        }

        public int IndexOf(T item)
        {
            return mList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (index <= Count())
            {
                mList.Insert(index, item);
            }
            else
            {
            }
        }

        public bool Contains(T item)
        {
            return mList.Contains(item);
        }

        public void Sort(System.Comparison<T> comparer)
        {
            mList.Sort(comparer);
        }

        public void merge(MList<T> appendList)
        {
            if(appendList != null)
            {
                foreach(T item in appendList.list())
                {
                    mList.Add(item);
                }
            }
        }
    }
}