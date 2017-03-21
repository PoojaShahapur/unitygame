namespace SDK.Lib
{
    /**
     * @brief 优先级队列
     */
    public class NoPriorityList : INoOrPriorityList
    {
        protected MList<INoOrPriorityObject> mNoPriorityProcessObjectList;  // 优先级对象列表

        protected MDictionary<INoOrPriorityObject, int> mDic;       // 查找字典
        protected bool mIsSpeedUpFind;      // 是否开启查找

        public NoPriorityList()
        {
            this.mNoPriorityProcessObjectList = new MList<INoOrPriorityObject>();
            this.mIsSpeedUpFind = false;
        }

        public void setIsSpeedUpFind(bool value)
        {
            this.mIsSpeedUpFind = value;

            if (this.mIsSpeedUpFind)
            {
                this.mDic = new MDictionary<INoOrPriorityObject, int>();
            }
        }

        public void setIsOpKeepSort(bool value)
        {

        }

        public void Clear()
        {
            this.mNoPriorityProcessObjectList.Clear();

            if(this.mIsSpeedUpFind)
            {
                this.mDic.Clear();
            }
        }

        public int Count()
        {
            return this.mNoPriorityProcessObjectList.Count();
        }

        public INoOrPriorityObject get(int index)
        {
            INoOrPriorityObject ret = null;

            if(index < this.Count())
            {
                ret = this.mNoPriorityProcessObjectList.get(index);
            }

            return ret;
        }

        public bool Contains(INoOrPriorityObject item)
        {
            bool ret = false;

            if (null != item)
            {
                if (this.mIsSpeedUpFind)
                {
                    ret = this.mDic.ContainsKey(item);
                }
                else
                {
                    int index = 0;
                    int listLen = this.mNoPriorityProcessObjectList.Count();

                    while (index < listLen)
                    {
                        if (item == this.mNoPriorityProcessObjectList.get(index))
                        {
                            ret = true;
                            break;
                        }

                        ++index;
                    }
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("NoPriorityList::Contains, failed", LogTypeId.eLogNoPriorityListCheck);
                }
            }

            return ret;
        }

        public void RemoveAt(int index)
        {
            if (this.mIsSpeedUpFind)
            {
                this.effectiveRemove(this.mNoPriorityProcessObjectList[index]);
            }
            else
            {
                this.mNoPriorityProcessObjectList.RemoveAt(index);
            }
        }

        public int getIndexByNoPriorityObject(INoOrPriorityObject priorityObject)
        {
            int retIndex = -1;

            int index = 0;
            int listLen = this.mNoPriorityProcessObjectList.Count();

            while (index < listLen)
            {
                if (this.mNoPriorityProcessObjectList.get(index) == priorityObject)
                {
                    retIndex = index;
                    break;
                }

                ++index;
            }

            return retIndex;
        }

        public int getIndexByNoOrPriorityObject(INoOrPriorityObject priorityObject)
        {
            return this.getIndexByNoPriorityObject(priorityObject);
        }

        public void addNoPriorityObject(INoOrPriorityObject noPriorityObject)
        {
            if (null != noPriorityObject)
            {
                if (!this.Contains(noPriorityObject))
                {
                    this.mNoPriorityProcessObjectList.Add(noPriorityObject);

                    if (this.mIsSpeedUpFind)
                    {
                        this.mDic.Add(noPriorityObject, this.mNoPriorityProcessObjectList.Count() - 1);
                    }
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("NoPriorityList::addNoPriorityObject, failed", LogTypeId.eLogNoPriorityListCheck);
                }
            }
        }

        public void removeNoPriorityObject(INoOrPriorityObject noPriorityObject)
        {
            if (null != noPriorityObject)
            {
                if (this.Contains(noPriorityObject))
                {
                    if (this.mIsSpeedUpFind)
                    {
                        this.effectiveRemove(noPriorityObject);
                    }
                    else
                    {
                        int index = this.getIndexByNoPriorityObject(noPriorityObject);

                        if (-1 != index)
                        {
                            this.mNoPriorityProcessObjectList.RemoveAt(index);
                        }
                    }
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("NoPriorityList::addNoPriorityObject, failed", LogTypeId.eLogNoPriorityListCheck);
                }
            }
        }

        public void addNoOrPriorityObject(INoOrPriorityObject noPriorityObject, float priority = 0.0f)
        {
            this.addNoPriorityObject(noPriorityObject);
        }

        public void removeNoOrPriorityObject(INoOrPriorityObject noPriorityObject)
        {
            this.removeNoPriorityObject(noPriorityObject);
        }

        // 快速移除元素
        protected bool effectiveRemove(INoOrPriorityObject item)
        {
            bool ret = false;

            if (this.mDic.ContainsKey(item))
            {
                ret = true;

                int idx = this.mDic[item];
                this.mDic.Remove(item);

                if (idx == this.mNoPriorityProcessObjectList.Count() - 1)    // 如果是最后一个元素，直接移除
                {
                    this.mNoPriorityProcessObjectList.RemoveAt(idx);
                }
                else
                {
                    this.mNoPriorityProcessObjectList.set(idx, this.mNoPriorityProcessObjectList.get(this.mNoPriorityProcessObjectList.Count() - 1));
                    this.mNoPriorityProcessObjectList.RemoveAt(this.mNoPriorityProcessObjectList.Count() - 1);
                    this.mDic.Add(this.mNoPriorityProcessObjectList.get(idx), idx);
                }
            }

            return ret;
        }

        protected void updateIndex(int idx)
        {
            int listLen = this.mNoPriorityProcessObjectList.Count();

            while (idx < listLen)
            {
                this.mDic.Add(this.mNoPriorityProcessObjectList.get(idx), idx);

                ++idx;
            }
        }
    }
}