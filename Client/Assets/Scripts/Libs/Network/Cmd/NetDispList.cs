using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetDispList
    {
        protected List<NetDispHandle> m_netDispList = new List<NetDispHandle>();
        protected bool m_bStopNetHandle = false;       // 是否停止网络消息处理

        public bool bStopNetHandle
        {
            get
            {
                return m_bStopNetHandle;
            }
            set
            {
                m_bStopNetHandle = value;
            }
        }

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
            if (false == m_bStopNetHandle)  // 如果没有停止网络处理
            {
                foreach (var item in m_netDispList)
                {
                    item.handleMsg(msg);
                }
            }
        }
    }
}