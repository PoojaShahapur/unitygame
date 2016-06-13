namespace SDK.Lib
{
    public class ResMsgRouteCB : MsgRouteDispHandle
    {
        public ResMsgRouteCB()
        {
            this.addRouteHandle((int)MsgRouteType.eMRT_BASIC, Ctx.m_instance.mDownloadMgr, Ctx.m_instance.mDownloadMgr.handleMsg);
        }
    }
}