using SDK.Common;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 当需要管理的对象可能在遍历中间添加的时候，需要这个管理器
     */
    public class DelayHandleMgrBase
    {
        protected List<DeferredMethod> m_deferredAddMethodQueue = new List<DeferredMethod>();
        protected List<DeferredMethod> m_deferredDelMethodQueue = new List<DeferredMethod>();
        protected bool m_duringAdvance;

        public virtual void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            if (m_duringAdvance)
            {
                ArrayList args = new ArrayList();
                args.Add(delayObject);
                args.Add(priority);
                callAddLater(addObject, args);
            }
        }

        public virtual void delObject(IDelayHandleItem delayObject)
        {
            if (m_duringAdvance)
            {
                delayObject.setClientDispose();
                ArrayList args = new ArrayList();
                args.Add(delayObject);
                callDelLater(delObject, args);
            }
        }

        protected void callAddLater(Action<IDelayHandleItem, float> method, ArrayList args = null)
        {
            DeferredMethod dm = new DeferredMethod();
            dm.m_addMethod = method;
            dm.m_args = args;
            m_deferredAddMethodQueue.Add(dm);
        }

        protected void callDelLater(Action<IDelayHandleItem> method, ArrayList args = null)
        {
            DeferredMethod dm = new DeferredMethod();
            dm.m_delMethod = method;
            dm.m_args = args;
            m_deferredDelMethodQueue.Add(dm);
        }

        protected void processScheduledObjects()
        {
            if (m_deferredAddMethodQueue.Count > 0)
            {
                for (int j = 0; j < m_deferredAddMethodQueue.Count; j++)
                {
                    DeferredMethod curDM = m_deferredAddMethodQueue[j];
                    curDM.m_addMethod.Invoke(curDM.m_args[0] as IDelayHandleItem, (float)curDM.m_args[1]);
                }

                m_deferredAddMethodQueue.Clear();
            }

            if (m_deferredDelMethodQueue.Count > 0)
            {
                for (int j = 0; j < m_deferredDelMethodQueue.Count; j++)
                {
                    DeferredMethod curDM = m_deferredDelMethodQueue[j];
                    curDM.m_delMethod.Invoke(curDM.m_args[0] as IDelayHandleItem);
                }

                m_deferredDelMethodQueue.Clear();
            }
        }

        public virtual void Advance(float delta)
        {
            processScheduledObjects();
            m_duringAdvance = true;
        }
    }
}