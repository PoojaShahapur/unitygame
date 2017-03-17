namespace SDK.Lib
{
    /**
     * @brief 非优先级或者优先级列表
     */
    public interface INoOrPriorityList
    {
        void setIsSpeedUpFind(bool value);
        void setIsOpKeepSort(bool value);
        void Clear();
        int Count();

        INoOrPriorityObject get(int index);
        bool Contains(INoOrPriorityObject item);
        void RemoveAt(int index);
        int getIndexByNoOrPriorityObject(INoOrPriorityObject priorityObject);

        void addNoOrPriorityObject(INoOrPriorityObject noPriorityObject, float priority = 0.0f);
        void removeNoOrPriorityObject(INoOrPriorityObject noPriorityObject);
    }
}