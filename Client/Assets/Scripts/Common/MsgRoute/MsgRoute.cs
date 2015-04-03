namespace SDK.Common
{
    public class SocketOpenedMR : MsgRouteBase
    {
        public SocketOpenedMR()
            : base(MsgRouteID.eMRIDSocketOpened)
        {

        }
    }

    public class SocketCloseedMR : MsgRouteBase
    {
        public SocketCloseedMR()
            : base(MsgRouteID.eMRIDSocketClosed)
        {

        }
    }
}