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
        protected int mUniqueId;       // 唯一 Id ，调试使用

        public MList()
        {
            this.mList = new List<T>();
        }

        public MList(int capacity)
        {
            this.mList = new List<T>(capacity);
        }

        public T[] ToArray()
        {
            return this.mList.ToArray();
        }

        public List<T> list()
        {
            return this.mList;
        }

        public int uniqueId
        {
            get
            {
                return this.mUniqueId;
            }
            set
            {
                this.mUniqueId = value;
            }
        }

        public List<T> buffer
        {
            get
            {
                return this.mList;
            }
        }

        public int size
        {
            get
            {
                return this.mList.Count;
            }
        }

        public void Add(T item)
        {
            this.mList.Add(item);
        }

        // 主要是 Add 一个 float 类型的 Vector3
        public void Add(T item_1, T item_2, T item_3)
        {
            this.mList.Add(item_1);
            this.mList.Add(item_2);
            this.mList.Add(item_3);
        }

        // 主要是 Add 一个 float 类型的 UV
        public void Add(T item_1, T item_2)
        {
            this.mList.Add(item_1);
            this.mList.Add(item_2);
        }

        // 主要是 Add 一个 byte 类型的 Color32
        public void Add(T item_1, T item_2, T item_3, T item_4)
        {
            this.mList.Add(item_1);
            this.mList.Add(item_2);
            this.mList.Add(item_3);
            this.mList.Add(item_4);
        }

        public void push(T item)
        {
            this.mList.Add(item);
        }

        public bool Remove(T item)
        {
            return this.mList.Remove(item);
        }

        public T this[int index]
        {
            get
            {
                return this.mList[index];
            }
            set
            {
                this.mList[index] = value;
            }
        }

        public void Clear()
        {
            this.mList.Clear();
        }

        public int Count()
        {
            return this.mList.Count;
        }

        public int length()
        {
            return this.mList.Count;
        }

        public void setLength(int value)
        {
            this.mList.Capacity = value;
        }

        public void RemoveAt(int index)
        {
            this.mList.RemoveAt(index);
        }

        public int IndexOf(T item)
        {
            return this.mList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (index <= this.Count())
            {
                this.mList.Insert(index, item);
            }
            else
            {
            }
        }

        public bool Contains(T item)
        {
            return this.mList.Contains(item);
        }

        public void Sort(System.Comparison<T> comparer)
        {
            this.mList.Sort(comparer);
        }

        public void merge(MList<T> appendList)
        {
            if(appendList != null)
            {
                foreach(T item in appendList.list())
                {
                    this.mList.Add(item);
                }
            }
        }
    }
}