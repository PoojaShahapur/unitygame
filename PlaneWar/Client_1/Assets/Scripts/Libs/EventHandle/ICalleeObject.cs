namespace SDK.Lib
{
    /**
     * @brief 可被调用的函数对象
     */
    public interface ICalleeObject
    {
        void call(IDispatchObject dispObj);
    }
}