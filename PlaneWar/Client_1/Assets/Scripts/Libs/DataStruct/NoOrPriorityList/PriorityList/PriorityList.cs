namespace SDK.Lib
{
    /**
     * @brief 优先级队列
     */
    public class PriorityList : INoOrPriorityList
    {
        protected MList<PriorityProcessObject> mPriorityProcessObjectList;  // 优先级对象列表
        protected PrioritySort mPrioritySort;   // 排序方式

        protected MDictionary<INoOrPriorityObject, int> mDic;   // 查找字典
        protected bool mIsSpeedUpFind;          // 是否开启查找
        protected bool mIsOpKeepSort;           // 操作的时候是否保持排序

        public PriorityList()
        {
            this.mPriorityProcessObjectList = new MList<PriorityProcessObject>();
            this.mPrioritySort = PrioritySort.ePS_Great;
            this.mIsSpeedUpFind = false;
            this.mIsOpKeepSort = false;
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
            this.mIsOpKeepSort = value;
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

        public INoOrPriorityObject get(int index)
        {
            INoOrPriorityObject ret = null;

            if(index < this.Count())
            {
                ret = this.mPriorityProcessObjectList.get(index).mPriorityObject;
            }

            return ret;
        }

        public float getPriority(int index)
        {
            float ret = 0;

            if (index < this.Count())
            {
                ret = this.mPriorityProcessObjectList.get(index).mPriority;
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
                    int listLen = this.mPriorityProcessObjectList.Count();

                    while (index < listLen)
                    {
                        if (item == this.mPriorityProcessObjectList.get(index).mPriorityObject)
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
                    Ctx.mInstance.mLogSys.log("PriorityList::Contains, failed", LogTypeId.eLogPriorityListCheck);
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
            int listLen = this.mPriorityProcessObjectList.Count();

            while (index < listLen)
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

        public int getIndexByPriorityObject(INoOrPriorityObject priorityObject)
        {
            int retIndex = -1;

            int index = 0;
            int listLen = this.mPriorityProcessObjectList.Count();

            while (index < listLen)
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

        public int getIndexByNoOrPriorityObject(INoOrPriorityObject priorityObject)
        {
            return this.getIndexByPriorityObject(priorityObject);
        }

        public void addPriorityObject(INoOrPriorityObject priorityObject, float priority = 0.0f)
        {
            if (null != priorityObject)
            {
                if (!this.Contains(priorityObject))
                {
                    PriorityProcessObject priorityProcessObject = null;
                    priorityProcessObject = new PriorityProcessObject();

                    priorityProcessObject.mPriorityObject = priorityObject;
                    priorityProcessObject.mPriority = priority;

                    if (!this.mIsOpKeepSort)
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
                    Ctx.mInstance.mLogSys.log("PriorityList::addPriorityObject, failed", LogTypeId.eLogPriorityListCheck);
                }
            }
        }

        public void removePriorityObject(INoOrPriorityObject priorityObject)
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

        public void addNoOrPriorityObject(INoOrPriorityObject noPriorityObject, float priority = 0.0f)
        {
            this.addPriorityObject(noPriorityObject);
        }

        public void removeNoOrPriorityObject(INoOrPriorityObject noPriorityObject)
        {
            this.removePriorityObject(noPriorityObject);
        }

        // 快速移除元素
        protected bool effectiveRemove(INoOrPriorityObject item)
        {
            bool ret = false;

            if (this.mDic.ContainsKey(item))
            {
                ret = true;

                int index = this.mDic[item];
                this.mDic.Remove(item);

                if (index == this.mPriorityProcessObjectList.Count() - 1)    // 如果是最后一个元素，直接移除
                {
                    this.mPriorityProcessObjectList.RemoveAt(index);
                }
                else
                {
                    // 这样移除会使优先级顺序改变
                    if (!this.mIsOpKeepSort)
                    {
                        this.mPriorityProcessObjectList.set(index, this.mPriorityProcessObjectList.get(this.mPriorityProcessObjectList.Count() - 1));
                        this.mPriorityProcessObjectList.RemoveAt(this.mPriorityProcessObjectList.Count() - 1);
                        this.mDic.Add(this.mPriorityProcessObjectList.get(index).mPriorityObject, index);
                    }
                    else
                    {
                        this.mPriorityProcessObjectList.RemoveAt(index);
                        this.updateIndex(index);
                    }
                }
            }

            return ret;
        }

        protected void updateIndex(int index)
        {
            int listLen = this.mPriorityProcessObjectList.Count();

            while (index < listLen)
            {
                this.mDic.Add(this.mPriorityProcessObjectList.get(index).mPriorityObject, index);

                ++index;
            }
        }
    }
}