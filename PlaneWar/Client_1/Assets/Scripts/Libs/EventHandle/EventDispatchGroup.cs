namespace SDK.Lib
{
    public class EventDispatchGroup
    {
        protected MDictionary<int, EventDispatch> mGroupID2DispatchDic;
        protected LoopDepth mLoopDepth;       // 是否是在循环遍历中

        public EventDispatchGroup()
        {
            this.mGroupID2DispatchDic = new MDictionary<int, EventDispatch>();
            this.mLoopDepth = new LoopDepth();
        }

        // 添加分发器
        public void addEventDispatch(int groupID, EventDispatch disp)
        {
            if (!this.mGroupID2DispatchDic.ContainsKey(groupID))
            {
                this.mGroupID2DispatchDic[groupID] = disp;
            }
        }

        public void addEventHandle(int groupID, ICalleeObject pThis, MAction<IDispatchObject> handle)
        {
            // 如果没有就创建一个
            if (!this.mGroupID2DispatchDic.ContainsKey(groupID))
            {
                this.addEventDispatch(groupID, new EventDispatch());
            }

            this.mGroupID2DispatchDic[groupID].addEventHandle(pThis, handle);
        }

        public void removeEventHandle(int groupID, ICalleeObject pThis, MAction<IDispatchObject> handle)
        {
            if (this.mGroupID2DispatchDic.ContainsKey(groupID))
            {
                this.mGroupID2DispatchDic[groupID].removeEventHandle(pThis, handle);

                // 如果已经没有了
                if (!this.mGroupID2DispatchDic[groupID].hasEventHandle())
                {
                    this.mGroupID2DispatchDic.Remove(groupID);
                }
            }
            else
            {
             
            }
        }

        public void dispatchEvent(int groupID, IDispatchObject dispatchObject)
        {
            this.mLoopDepth.incDepth();

            if (this.mGroupID2DispatchDic.ContainsKey(groupID))
            {
                this.mGroupID2DispatchDic[groupID].dispatchEvent(dispatchObject);
            }
            else
            {
             
            }

            this.mLoopDepth.decDepth();
        }

        public void clearAllEventHandle()
        {
            if (!this.mLoopDepth.isInDepth())
            {
                foreach (EventDispatch dispatch in this.mGroupID2DispatchDic.Values)
                {
                    dispatch.clearEventHandle();
                }

                this.mGroupID2DispatchDic.Clear();
            }
            else
            {
            
            }
        }

        public void clearGroupEventHandle(int groupID)
        {
            if (!this.mLoopDepth.isInDepth())
            {
                if (this.mGroupID2DispatchDic.ContainsKey(groupID))
                {
                    this.mGroupID2DispatchDic[groupID].clearEventHandle();
                    this.mGroupID2DispatchDic.Remove(groupID);
                }
                else
                {
                 
                }
            }
            else
            {
             
            }
        }

        public bool hasEventHandle(int groupID)
        {
            if(this.mGroupID2DispatchDic.ContainsKey(groupID))
            {
                return this.mGroupID2DispatchDic[groupID].hasEventHandle();
            }

            return false;
        }
    }
}