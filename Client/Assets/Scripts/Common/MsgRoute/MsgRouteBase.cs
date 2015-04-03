namespace SDK.Common
{
    public enum MsgRouteID
    {
        eMRIDSocketOpened,      // socket Opened
        eMRIDSocketClosed,      // socket Opened
    }

    public class MsgRouteBase
    {
        public MsgRouteID m_msgID;          // 只需要一个 ID 就行了

        public MsgRouteBase(MsgRouteID id)
        {
            m_msgID = id;
        }
    }
}