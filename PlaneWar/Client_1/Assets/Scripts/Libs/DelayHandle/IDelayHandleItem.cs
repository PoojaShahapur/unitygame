namespace SDK.Lib
{
    /**
     * @brief 延迟添加的对象
     */
    public interface IDelayHandleItem
    {
        void setClientDispose(bool isDispose);
        bool isClientDispose();
    }
}