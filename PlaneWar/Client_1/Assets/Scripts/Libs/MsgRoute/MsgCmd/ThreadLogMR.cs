namespace SDK.Lib
{
    // 线程日志
    public class ThreadLogMR : MsgRouteBase
    {
        public string mLogSys;

        public ThreadLogMR()
            : base(MsgRouteID.eMRIDThreadLog)
        {

        }
    }
}