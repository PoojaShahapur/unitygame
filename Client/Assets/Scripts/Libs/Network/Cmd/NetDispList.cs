using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetDispList
    {
        protected List<NetDispHandle> m_netDispList = new List<NetDispHandle>();

        public void addOneDisp(NetDispHandle disp)
        {
            if (m_netDispList.IndexOf(disp) == -1)
            {
                m_netDispList.Add(disp);
            }
        }

        public void removeOneDisp(NetDispHandle disp)
        {
            if (m_netDispList.IndexOf(disp) != -1)
            {
                m_netDispList.Remove(disp);
            }
        }

        public void handleMsg(ByteBuffer msg)
        {
            foreach (var item in m_netDispList)
            {
                item.handleMsg(msg);
            }
        }
    }
}