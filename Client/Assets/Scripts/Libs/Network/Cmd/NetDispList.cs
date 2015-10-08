using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetDispList
    {
        protected int m_revMsgCnt = 0;      // 接收到消息的数量
        protected int m_handleMsgCnt = 0;   // 处理的消息的数量

        protected List<NetDispHandle> m_netDispList;
        protected bool m_bStopNetHandle;       // 是否停止网络消息处理

        public NetDispList()
        {
            m_netDispList = new List<NetDispHandle>();
            m_bStopNetHandle = false;
        }

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
            //if (false == m_bStopNetHandle)  // 如果没有停止网络处理
            //{
                foreach (var item in m_netDispList)
                {
                    item.handleMsg(msg);
                }
            //}
        }

        public void addOneRevMsg()
        {
            ++m_revMsgCnt;

            Ctx.m_instance.m_logSys.log(string.Format("接收到消息数量 {0}", m_revMsgCnt));
        }

        public void addOneHandleMsg()
        {
            ++m_handleMsgCnt;

            Ctx.m_instance.m_logSys.log(string.Format("处理消息数量 {0}", m_handleMsgCnt));
        }

        public void clearOneRevMsg()
        {
            m_revMsgCnt = 0;
            Ctx.m_instance.m_logSys.log("清理接收消息数量");
        }

        public void clearOneHandleMsg()
        {
            m_handleMsgCnt = 0;
            Ctx.m_instance.m_logSys.log("清理处理消息数量");
        }
    }
}