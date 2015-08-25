using SDK.Lib;

namespace SDK.Lib
{
    public class ResMsgRouteCB : MsgRouteDispHandle
    {
        public ResMsgRouteCB()
        {
            m_id2DispDic[(int)MsgRouteType.eMRT_BASIC] = new ResLoadMgr();
        }
    }
}