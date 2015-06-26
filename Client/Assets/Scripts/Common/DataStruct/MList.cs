using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief 对系统 List 的封装
     */
    public class MList<T>
    {
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

        public List<T> list
        {
            get
            {
                return m_list;
            }
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

        public void Add(T item)
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
                Ctx.m_instance.m_logSys.log(string.Format("Insert Failed index={0}, Count={1}", index, Count()));
            }
        }

        public bool Contains(T item)
        {
            return m_list.Contains(item);
        }
    }
}