using System.Collections.Generic;

namespace SDK.Common
{
    public class MsgRouteDispList
    {
        protected List<MsgRouteDispHandle> m_dispList = new List<MsgRouteDispHandle>();

        public void addOneDisp(MsgRouteDispHandle disp)
        {
            if(m_dispList.IndexOf(disp) == -1)
            {
                m_dispList.Add(disp);
            }
        }

        public void removeOneDisp(MsgRouteDispHandle disp)
        {
            if(m_dispList.IndexOf(disp) != -1)
            {
                m_dispList.Remove(disp);
            }
        }

        public void handleMsg(MsgRouteBase msg)
        {
            foreach(var item in m_dispList)
            {
                item.handleMsg(msg);
            }

            Ctx.m_instance.m_poolSys.deleteObj(msg);
        }
    }
}