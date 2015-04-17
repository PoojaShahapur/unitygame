namespace SDK.Common
{
    public enum MsgRouteType
    {
        eMRT_BASIC,      // 基本类型
    }

    public enum MsgRouteID
    {
        eMRIDSocketOpened,      // socket Opened
        eMRIDSocketClosed,      // socket Opened
        eMRIDLoadedWebRes,      // web 资源加载完成
        eMRIDThreadLog,      // 线程打日志
    }

    public class MsgRouteBase : IRecycle
    {
        public MsgRouteType m_msgType;
        public MsgRouteID m_msgID;          // 只需要一个 ID 就行了

        public MsgRouteBase(MsgRouteID id)
        {
            m_msgType = MsgRouteType.eMRT_BASIC;
            m_msgID = id;
        }

        virtual public void resetDefault()
        {

        }
    }
}