using SDK.Lib;
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

    public class LoadedWebResMR : MsgRouteBase
    {
        public ITask m_task;

        public LoadedWebResMR()
            : base(MsgRouteID.eMRIDLoadedWebRes)
        {

        }

        override public void resetDefault()
        {
            m_task = null;
        }
    }

    // 线程日志
    public class ThreadLogMR : MsgRouteBase
    {
        public string m_log;

        public ThreadLogMR()
            : base(MsgRouteID.eMRIDThreadLog)
        {

        }
    }
}