namespace SDK.Lib
{
    public class MsgRouteBase : IRecycle, IDispatchObject
    {
        public MsgRouteType mMsgType;
        public MsgRouteID mMsgID;          // 只需要一个 ID 就行了
        public bool mIsMainThreadImmeHandle;    // 是否主线程立即处理消息

        public MsgRouteBase(MsgRouteID id, MsgRouteType type = MsgRouteType.eMRT_BASIC)
        {
            this.mMsgType = type;
            this.mMsgID = id;

            this.mIsMainThreadImmeHandle = true;
        }

        virtual public void resetDefault()
        {

        }

        public void setIsMainThreadImmeHandle(bool value)
        {
            this.mIsMainThreadImmeHandle = value;
        }

        public bool isMainThreadImmeHandle()
        {
            return mIsMainThreadImmeHandle;
        }
    }
}