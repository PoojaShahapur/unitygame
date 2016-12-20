using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 对系统 List 的封装
     */
    public class MList<T>
    {
        //public delegate int CompareFunc(T left, T right);

        protected List<T> m_list;
        protected int m_uniqueId;       // 唯一 Id ，调试使用

        public MList()
        {
            m_list = new List<T>();
        }

        public MList(int capacity)
        {
            m_list = new List<T>(capacity);
        }

        public T[] ToArray()
        {
            return m_list.ToArray();
        }

        public List<T> list()
        {
            return m_list;
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
                return m_list;
            }
        }

        public int size
        {
            get
            {
                return m_list.Count;
            }
        }

        public void Add(T item)
        {
            m_list.Add(item);
        }

        // 主要是 Add 一个 float 类型的 Vector3
        public void Add(T item_1, T item_2, T item_3)
        {
            m_list.Add(item_1);
            m_list.Add(item_2);
            m_list.Add(item_3);
        }

        // 主要是 Add 一个 float 类型的 UV
        public void Add(T item_1, T item_2)
        {
            m_list.Add(item_1);
            m_list.Add(item_2);
        }

        // 主要是 Add 一个 byte 类型的 Color32
        public void Add(T item_1, T item_2, T item_3, T item_4)
        {
            m_list.Add(item_1);
            m_list.Add(item_2);
            m_list.Add(item_3);
            m_list.Add(item_4);
        }

        public void push(T item)
        {
            m_list.Add(item);
        }

        public bool Remove(T item)
        {
            return m_list.Remove(item);
        }

        public T this[int index]
        {
            get
            {
                return m_list[index];
            }
            set
            {
                m_list[index] = value;
            }
        }

        public void Clear()
        {
            m_list.Clear();
        }

        public int Count()
        {
            return m_list.Count;
        }

        public int length()
        {
            return m_list.Count;
        }

        public void setLength(int value)
        {
            m_list.Capacity = value;
        }

        public void RemoveAt(int index)
        {
            m_list.RemoveAt(index);
        }

        public int IndexOf(T item)
        {
            return m_list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (index <= Count())
            {
                m_list.Insert(index, item);
            }
            else
            {
            }
        }

        public bool Contains(T item)
        {
            return m_list.Contains(item);
        }

        public void Sort(System.Comparison<T> comparer)
        {
            m_list.Sort(comparer);
        }

        public void merge(MList<T> appendList)
        {
            if(appendList != null)
            {
                foreach(T item in appendList.list())
                {
                    m_list.Add(item);
                }
            }
        }
    }
}