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

        protected Dictionary<T, int> mDic;    // 为了加快查找速度，当前 Element 到索引映射
        protected bool mIsSpeedUpFind;  // 是否加快查询，这个只适用于元素在列表中是唯一的，例如引用之类的，如果有相同的，就会有问题，注意了

        public MList()
        {
            this.mList = new List<T>();
            this.mDic = new Dictionary<T, int>();
            this.mIsSpeedUpFind = true;
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

            if (this.mIsSpeedUpFind)
            {
                this.mDic[item] = this.mList.Count - 1;
            }
        }

        // 主要是 Add 一个 float 类型的 Vector3
        public void Add(T item_1, T item_2, T item_3)
        {
            this.mList.Add(item_1);
            this.mList.Add(item_2);
            this.mList.Add(item_3);

            if(this.mIsSpeedUpFind)
            {
                this.mDic[item_1] = this.mList.Count - 3;
                this.mDic[item_2] = this.mList.Count - 2;
                this.mDic[item_3] = this.mList.Count - 1;
            }
        }

        // 主要是 Add 一个 float 类型的 UV
        public void Add(T item_1, T item_2)
        {
            this.mList.Add(item_1);
            this.mList.Add(item_2);

            if (this.mIsSpeedUpFind)
            {
                this.mDic[item_1] = this.mList.Count - 2;
                this.mDic[item_2] = this.mList.Count - 1;
            }
        }

        // 主要是 Add 一个 byte 类型的 Color32
        public void Add(T item_1, T item_2, T item_3, T item_4)
        {
            this.mList.Add(item_1);
            this.mList.Add(item_2);
            this.mList.Add(item_3);
            this.mList.Add(item_4);

            if (this.mIsSpeedUpFind)
            {
                this.mDic[item_1] = this.mList.Count - 4;
                this.mDic[item_2] = this.mList.Count - 3;
                this.mDic[item_3] = this.mList.Count - 2;
                this.mDic[item_4] = this.mList.Count - 1;
            }
        }

        public void push(T item)
        {
            this.mList.Add(item);

            if (this.mIsSpeedUpFind)
            {
                this.mDic[item] = this.mList.Count - 1;
            }
        }

        public bool Remove(T item)
        {
            if (this.mIsSpeedUpFind)
            {
                return this.effectiveRemove(item);
            }
            else
            {
                return this.mList.Remove(item);
            }
        }

        public T this[int index]
        {
            get
            {
                return this.mList[index];
            }
            set
            {
                if (this.mIsSpeedUpFind)
                {
                    this.mDic[value] = index;
                }

                this.mList[index] = value;
            }
        }

        public void Clear()
        {
            this.mList.Clear();

            if (this.mIsSpeedUpFind)
            {
                this.mDic.Clear();
            }
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
            if (this.mIsSpeedUpFind)
            {
                this.effectiveRemove(this.mList[index]);
            }
            else
            {
                this.mList.RemoveAt(index);
            }
        }

        public int IndexOf(T item)
        {
            if (this.mIsSpeedUpFind)
            {
                if (this.mDic.ContainsKey(item))
                {
                    return this.mDic[item];
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return this.mList.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            if (index <= this.Count())
            {
                this.mList.Insert(index, item);

                if (this.mIsSpeedUpFind)
                {
                    this.mDic[item] = index;

                    this.updateIndex(index + 1);
                }
            }
            else
            {

            }
        }

        public bool Contains(T item)
        {
            if (this.mIsSpeedUpFind)
            {
                return this.mDic.ContainsKey(item);
            }
            else
            {
                return this.mList.Contains(item);
            }
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

                    if (this.mIsSpeedUpFind)
                    {
                        this.mDic[item] = this.mList.Count - 1;
                    }
                }
            }
        }

        // 快速移除元素
        protected bool effectiveRemove(T item)
        {
            bool ret = false;

            if (this.mDic.ContainsKey(item))
            {
                ret = true;

                int idx = this.mDic[item];
                this.mDic.Remove(item);

                if (idx == this.mList.Count - 1)    // 如果是最后一个元素，直接移除
                {
                    this.mList.RemoveAt(idx);
                }
                else
                {
                    this.mList[idx] = this.mList[this.mList.Count - 1];
                    this.mList.RemoveAt(this.mList.Count - 1);
                    this.mDic[this.mList[idx]] = idx;
                }
            }

            return ret;
        }

        protected void updateIndex(int idx)
        {
            int len = this.mList.Count;

            while(idx < len)
            {
                this.mDic[this.mList[idx]] = idx;

                ++idx;
            }
        }
    }
}