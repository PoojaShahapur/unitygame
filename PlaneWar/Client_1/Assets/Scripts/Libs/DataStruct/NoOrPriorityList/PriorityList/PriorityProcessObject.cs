namespace SDK.Lib
{
    /**
     * @brief 优先级队列对象
     */
    public class PriorityProcessObject
    {
        public INoOrPriorityObject mPriorityObject;
        public float mPriority;

        public PriorityProcessObject()
        {
            this.mPriorityObject = null;
            this.mPriority = 0.0f;
        }
    }
}