using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetCmdNotify
    {
        protected int m_revMsgCnt;      // 接收到消息的数量
        protected int m_handleMsgCnt;   // 处理的消息的数量

        protected List<NetModuleDispHandle> m_netModuleDispList;
        protected bool m_bStopNetHandle;       // 是否停止网络消息处理
        protected CmdDispInfo mCmdDispInfo;

        public NetCmdNotify()
        {
            m_revMsgCnt = 0;
            m_handleMsgCnt = 0;
            m_netModuleDispList = new List<NetModuleDispHandle>();
            m_bStopNetHandle = false;
            mCmdDispInfo = new CmdDispInfo();
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

        public void addOneDisp(NetModuleDispHandle disp)
        {
            if (m_netModuleDispList.IndexOf(disp) == -1)
            {
                m_netModuleDispList.Add(disp);
            }
        }

        public void removeOneDisp(NetModuleDispHandle disp)
        {
            if (m_netModuleDispList.IndexOf(disp) != -1)
            {
                m_netModuleDispList.Remove(disp);
            }
        }

        public void handleMsg(ByteBuffer msg)
        {
            //if (false == m_bStopNetHandle)  // 如果没有停止网络处理
            //{
            byte byCmd = 0;
            msg.readUnsignedInt8(ref byCmd);
            byte byParam = 0;
            msg.readUnsignedInt8(ref byParam);
            msg.setPos(0);

            mCmdDispInfo.bu = msg;
            mCmdDispInfo.byCmd = byCmd;
            mCmdDispInfo.byParam = byParam;

            foreach (var item in m_netModuleDispList)
            {
                item.handleMsg(mCmdDispInfo);
            }
            //}
        }

        public void addOneRevMsg()
        {
            ++m_revMsgCnt;

            Ctx.mInstance.mLogSys.log(string.Format("接收到消息数量 {0}", m_revMsgCnt));
        }

        public void addOneHandleMsg()
        {
            ++m_handleMsgCnt;

            Ctx.mInstance.mLogSys.log(string.Format("处理消息数量 {0}", m_handleMsgCnt));
        }

        public void clearOneRevMsg()
        {
            m_revMsgCnt = 0;
            Ctx.mInstance.mLogSys.log("清理接收消息数量");
        }

        public void clearOneHandleMsg()
        {
            m_handleMsgCnt = 0;
            Ctx.mInstance.mLogSys.log("清理处理消息数量");
        }
    }
}