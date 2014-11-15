using SDK.Common;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * @brief 心跳管理器
 */
namespace SDK.Lib
{
    class TickMgr : ITickMgr
    {
        protected List<ProcessObject> m_TickLst = new List<ProcessObject>();
        protected List<DeferredMethod> m_deferredMethodQueue = new List<DeferredMethod>();
        protected bool m_duringAdvance;

        public TickMgr()
        {
            
        }

        public void Advance(float delta)
        {
            processScheduledObjects();
            m_duringAdvance = true;
            foreach (ProcessObject tk in m_TickLst)
            {
                (tk.m_listener as ITickedObject).OnTick(delta);
            }
            m_duringAdvance = false;
        }

        public void AddTickObj(ITickedObject obj, float priority = 0.0f)
        {
            addObject(obj, priority);
        }

        public void callLater(Action<ITickedObject, float> method, ArrayList args = null)
		{
			DeferredMethod dm = new DeferredMethod();
			dm.m_method = method;
			dm.m_args = args;
            m_deferredMethodQueue.Add(dm);
		}

        private void addObject(ITickedObject tickObject, float priority)
		{
			if (m_duringAdvance)
			{
                ArrayList args = new ArrayList();
                args.Add(tickObject);
                args.Add(priority);
				callLater(addObject, args);
				return;
			}
			
			int position = -1;
			for (int i = 0; i < m_TickLst.Count; i++)
			{
				if (m_TickLst[i] == null)
					continue;
				
				if (m_TickLst[i].m_listener == tickObject)
				{
					return;
				}
				
				if (m_TickLst[i].m_priority < priority)
				{
					position = i;
					break;
				}
			}
			
			ProcessObject processObject = new ProcessObject();
            processObject.m_listener = tickObject;
			processObject.m_priority = priority;
			
			if (position < 0 || position >= m_TickLst.Count)
				m_TickLst.Add(processObject);
			else
				m_TickLst.Insert(position, processObject);
		}

        private void processScheduledObjects()
		{
			if (m_deferredMethodQueue.Count > 0)
			{
				for (int j = 0; j < m_deferredMethodQueue.Count; j++)
				{
                    DeferredMethod curDM = m_deferredMethodQueue[j];
                    curDM.m_method.Invoke(curDM.m_args[0] as ITickedObject, (float)curDM.m_args[1]);
				}
				
                m_deferredMethodQueue.Clear();
			}
		}
    }
}