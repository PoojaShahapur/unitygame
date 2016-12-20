namespace SDK.Lib
{
    /**
     * @brief 系统消息流程，整个系统的消息分发都走这里，仅限单线程
     */
    public class SysMsgRoute : LockQueue<MsgRouteBase>
    {
        public SysMsgRoute(string name)
            : base(name)
        {

        }
    }
}