﻿using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief 对系统 List 的封装
     */
    public class MList<T>
    {
        protected List<T> m_list;

        public MList()
        {
            m_list = new List<T>();
        }

        public List<T> list
        {
            get
            {
                return m_list;
            }
        }

        public void Add(T item)
        {
            m_list.Add(item);
        }

        public void Remove(T item)
        {
            m_list.Remove(item);
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

        }
    }
}