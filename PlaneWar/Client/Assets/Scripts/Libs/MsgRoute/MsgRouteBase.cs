namespace SDK.Lib
{
    public enum MsgRouteType
    {
        eMRT_BASIC,      // 基本类型
    }

    public enum MsgRouteID
    {
        eMRIDSocketOpened,      // socket Opened
        eMRIDSocketClosed,      // socket Closed
        eMRIDLoadedWebRes,      // web 资源加载完成
        eMRIDThreadLog,      // 线程打日志
    }

    public class MsgRouteBase : IRecycle, IDispatchObject
    {
        public MsgRouteType mMsgType;
        public MsgRouteID mMsgID;          // 只需要一个 ID 就行了

        public MsgRouteBase(MsgRouteID id)
        {
            mMsgType = MsgRouteType.eMRT_BASIC;
            mMsgID = id;
        }

        virtual public void resetDefault()
        {

        }
    }
}