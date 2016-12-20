namespace SDK.Lib
{
    public class ResMsgRouteCB : MsgRouteDispHandle
    {
        public ResMsgRouteCB()
        {
            this.addRouteHandle((int)MsgRouteType.eMRT_BASIC, Ctx.mInstance.mDownloadMgr, Ctx.mInstance.mDownloadMgr.handleMsg);
        }
    }
}