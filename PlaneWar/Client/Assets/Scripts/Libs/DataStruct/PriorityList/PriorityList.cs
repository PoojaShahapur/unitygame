namespace SDK.Lib
{
    /**
     * @brief 优先级队列
     */
    public class PriorityList
    {
        protected MList<PriorityProcessObject> mPriorityProcessObjectList;  // 优先级对象列表
        protected PrioritySort mPrioritySort;   // 排序方式

        protected MDictionary<IPriorityObject, int> mDic;       // 查找字典
        protected bool mIsSpeedUpFind;      // 是否开启查找

        public PriorityList()
        {
            this.mPriorityProcessObjectList = new MList<PriorityProcessObject>();
            this.mPrioritySort = PrioritySort.ePS_Great;
            this.mIsSpeedUpFind = false;
        }

        public void setIsSpeedUpFind(bool value)
        {
            this.mIsSpeedUpFind = value;

            if (this.mIsSpeedUpFind)
            {
                this.mDic = new MDictionary<IPriorityObject, int>();
            }
        }

        public void Clear()
        {
            this.mPriorityProcessObjectList.Clear();

            if(this.mIsSpeedUpFind)
            {
                this.mDic.Clear();
            }
        }

        public int Count()
        {
            return this.mPriorityProcessObjectList.Count();
        }

        public IPriorityObject get(int index)
        {
            IPriorityObject ret = null;

            if(index < Count())
            {
                ret = this.mPriorityProcessObjectList.get(index).mPriorityObject;
            }

            return ret;
        }

        public float getPriority(int index)
        {
            float ret = 0;

            if (index < Count())
            {
                ret = this.mPriorityProcessObjectList.get(index).mPriority;
            }

            return ret;
        }

        public bool Contains(IPriorityObject item)
        {
            bool ret = false;

            if (this.mIsSpeedUpFind)
            {
                ret = this.mDic.ContainsKey(item);
            }
            else
            {
                int index = 0;
                int len = this.mPriorityProcessObjectList.Count();

                while(index < len)
                {
                    if(item == this.mPriorityProcessObjectList.get(index).mPriorityObject)
                    {
                        ret = true;
                        break;
                    }

                    ++index;
                }
            }

            return ret;
        }

        public void RemoveAt(int index)
        {
            if (this.mIsSpeedUpFind)
            {
                this.effectiveRemove(this.mPriorityProcessObjectList[index].mPriorityObject);
            }
            else
            {
                this.mPriorityProcessObjectList.RemoveAt(index);
            }
        }

        public int getIndexByPriority(float priority)
        {
            int retIndex = -1;

            int index = 0;
            int len = this.mPriorityProcessObjectList.Count();

            while (index < len)
            {
                if (PrioritySort.ePS_Less == this.mPrioritySort)
                {
                    if (this.mPriorityProcessObjectList.get(index).mPriority >= priority)
                    {
                        retIndex = index;
                        break;
                    }
                }
                else if (PrioritySort.ePS_Great == this.mPrioritySort)
                {
                    if (this.mPriorityProcessObjectList.get(index).mPriority <= priority)
                    {
                        retIndex = index;
                        break;
                    }
                }

                ++index;
            }

            return retIndex;
        }

        public int getIndexByPriorityObject(IPriorityObject priorityObject)
        {
            int retIndex = -1;

            int index = 0;
            int len = this.mPriorityProcessObjectList.Count();

            while (index < len)
            {
                if (this.mPriorityProcessObjectList.get(index).mPriorityObject == priorityObject)
                {
                    retIndex = index;
                    break;
                }

                ++index;
            }

            return retIndex;
        }

        public void addPriorityObject(IPriorityObject priorityObject, float priority = 0.0f, bool isNeedSort = false)
        {
            if (null != priorityObject)
            {
                if (!this.Contains(priorityObject))
                {
                    PriorityProcessObject priorityProcessObject = null;
                    priorityProcessObject = new PriorityProcessObject();

                    priorityProcessObject.mPriorityObject = priorityObject;
                    priorityProcessObject.mPriority = priority;

                    if (!isNeedSort)
                    {
                        this.mPriorityProcessObjectList.Add(priorityProcessObject);

                        if (this.mIsSpeedUpFind)
                        {
                            this.mDic.Add(priorityObject, this.mPriorityProcessObjectList.Count() - 1);
                        }
                    }
                    else
                    {
                        int index = this.getIndexByPriority(priority);

                        if (-1 == index)
                        {
                            this.mPriorityProcessObjectList.Add(priorityProcessObject);

                            if (this.mIsSpeedUpFind)
                            {
                                this.mDic.Add(priorityObject, this.mPriorityProcessObjectList.Count() - 1);
                            }
                        }
                        else
                        {
                            this.mPriorityProcessObjectList.Insert(index, priorityProcessObject);

                            if (this.mIsSpeedUpFind)
                            {
                                this.mDic.Add(priorityObject, index);
                                this.updateIndex(index + 1);
                            }
                        }
                    }
                }
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("PriorityList::addPriorityObject, failed", LogTypeId.eLogCommon);
                }
            }
        }

        public void removePriorityObject(IPriorityObject priorityObject)
        {
            if (this.Contains(priorityObject))
            {
                if (this.mIsSpeedUpFind)
                {
                    this.effectiveRemove(priorityObject);
                }
                else
                {
                    int index = this.getIndexByPriorityObject(priorityObject);

                    if(-1 != index)
                    {
                        this.mPriorityProcessObjectList.RemoveAt(index);
                    }
                }
            }
        }

        // 快速移除元素
        protected bool effectiveRemove(IPriorityObject item)
        {
            bool ret = false;

            if (this.mDic.ContainsKey(item))
            {
                ret = true;

                int idx = this.mDic[item];
                this.mDic.Remove(item);

                if (idx == this.mPriorityProcessObjectList.Count() - 1)    // 如果是最后一个元素，直接移除
                {
                    this.mPriorityProcessObjectList.RemoveAt(idx);
                }
                else
                {
                    this.mPriorityProcessObjectList.set(idx, this.mPriorityProcessObjectList.get(this.mPriorityProcessObjectList.Count() - 1));
                    this.mPriorityProcessObjectList.RemoveAt(this.mPriorityProcessObjectList.Count() - 1);
                    this.mDic.Add(this.mPriorityProcessObjectList.get(idx).mPriorityObject, idx);
                }
            }

            return ret;
        }

        protected void updateIndex(int idx)
        {
            int len = this.mPriorityProcessObjectList.Count();

            while (idx < len)
            {
                this.mDic.Add(this.mPriorityProcessObjectList.get(idx).mPriorityObject, idx);

                ++idx;
            }
        }
    }
}