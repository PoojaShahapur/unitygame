namespace SDK.Lib
{
    /**
     * @brief 当需要管理的对象可能在遍历中间添加的时候，需要这个管理器
     */
    public class DelayHandleMgrBase
    {
        protected MList<DelayHandleObject> m_deferredAddQueue;
        protected MList<DelayHandleObject> m_deferredDelQueue;

        private int m_loopDepth;           // 是否在循环中，支持多层嵌套，就是循环中再次调用循环

        public DelayHandleMgrBase()
        {
            m_deferredAddQueue = new MList<DelayHandleObject>();
            m_deferredDelQueue = new MList<DelayHandleObject>();

            m_loopDepth = 0;
        }

        virtual public void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            if (m_loopDepth > 0)
            {
                if (!existAddList(delayObject))        // 如果添加列表中没有
                {
                    if (existDelList(delayObject))    // 如果已经添加到删除列表中
                    {
                        delFromDelayDelList(delayObject);
                    }

                    DelayHandleObject delayHandleObject = new DelayHandleObject();
                    delayHandleObject.m_delayParam = new DelayAddParam();
                    m_deferredAddQueue.Add(delayHandleObject);

                    delayHandleObject.m_delayObject = delayObject;
                    (delayHandleObject.m_delayParam as DelayAddParam).m_priority = priority;
                }
            }
        }

        virtual public void delObject(IDelayHandleItem delayObject)
        {
            if (m_loopDepth > 0)
            {
                if (!existDelList(delayObject))
                {
                    if (existAddList(delayObject))    // 如果已经添加到删除列表中
                    {
                        delFromDelayAddList(delayObject);
                    }

                    delayObject.setClientDispose();

                    DelayHandleObject delayHandleObject = new DelayHandleObject();
                    delayHandleObject.m_delayParam = new DelayDelParam();
                    m_deferredDelQueue.Add(delayHandleObject);
                    delayHandleObject.m_delayObject = delayObject;
                }
            }
        }

        // 只有没有添加到列表中的才能添加
        protected bool existAddList(IDelayHandleItem delayObject)
        {
            foreach(var item in m_deferredAddQueue.list)
            {
                if(UtilApi.isAddressEqual(item.m_delayObject, delayObject))
                {
                    return true;
                }
            }

            return false;
        }

        // 只有没有添加到列表中的才能添加
        protected bool existDelList(IDelayHandleItem delayObject)
        {
            foreach (var item in m_deferredDelQueue.list)
            {
                if (UtilApi.isAddressEqual(item.m_delayObject, delayObject))
                {
                    return true;
                }
            }

            return false;
        }

        // 从延迟添加列表删除一个 Item
        protected void delFromDelayAddList(IDelayHandleItem delayObject)
        {
            foreach (var item in m_deferredAddQueue.list)
            {
                if (UtilApi.isAddressEqual(item.m_delayObject, delayObject))
                {
                    m_deferredAddQueue.Remove(item);
                }
            }
        }

        // 从延迟删除列表删除一个 Item
        protected void delFromDelayDelList(IDelayHandleItem delayObject)
        {
            foreach (var item in m_deferredDelQueue.list)
            {
                if(UtilApi.isAddressEqual(item.m_delayObject, delayObject))
                {
                    m_deferredDelQueue.Remove(item);
                }
            }
        }

        private void processDelayObjects()
        {
            if (0 == m_loopDepth)       // 只有全部退出循环后，才能处理添加删除
            {
                if (m_deferredAddQueue.Count() > 0)
                {
                    for (int idx = 0; idx < m_deferredAddQueue.Count(); idx++)
                    {
                        addObject(m_deferredAddQueue[idx].m_delayObject, (m_deferredAddQueue[idx].m_delayParam as DelayAddParam).m_priority);
                    }

                    m_deferredAddQueue.Clear();
                }

                if (m_deferredDelQueue.Count() > 0)
                {
                    for (int idx = 0; idx < m_deferredDelQueue.Count(); idx++)
                    {
                        delObject(m_deferredDelQueue[idx].m_delayObject);
                    }

                    m_deferredDelQueue.Clear();
                }
            }
        }

        public void incDepth()
        {
            ++m_loopDepth;
        }

        public void decDepth()
        {
            --m_loopDepth;
            processDelayObjects();
        }

        public bool bInDepth()
        {
            return m_loopDepth > 0;
        }
    }
}