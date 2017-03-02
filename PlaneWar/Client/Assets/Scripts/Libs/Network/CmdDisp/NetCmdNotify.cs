using System.Collections.Generic;

namespace SDK.Lib
{
    public class NetCmdNotify
    {
        protected int mRevMsgCnt;      // 接收到消息的数量
        protected int mHandleMsgCnt;   // 处理的消息的数量

        protected List<NetModuleDispHandle> mNetModuleDispList;
        protected bool mIsStopNetHandle;       // 是否停止网络消息处理
        protected CmdDispInfo mCmdDispInfo;

        public NetCmdNotify()
        {
            mRevMsgCnt = 0;
            mHandleMsgCnt = 0;
            mNetModuleDispList = new List<NetModuleDispHandle>();
            mIsStopNetHandle = false;
            mCmdDispInfo = new CmdDispInfo();
        }

        public bool isStopNetHandle
        {
            get
            {
                return mIsStopNetHandle;
            }
            set
            {
                mIsStopNetHandle = value;
            }
        }

        public void addOneDisp(NetModuleDispHandle disp)
        {
            if (mNetModuleDispList.IndexOf(disp) == -1)
            {
                mNetModuleDispList.Add(disp);
            }
        }

        public void removeOneDisp(NetModuleDispHandle disp)
        {
            if (mNetModuleDispList.IndexOf(disp) != -1)
            {
                mNetModuleDispList.Remove(disp);
            }
        }

        public void handleMsg(ByteBuffer msg)
        {
            //if (false == mIsStopNetHandle)  // 如果没有停止网络处理
            //{
            byte byCmd = 0;
            msg.readUnsignedInt8(ref byCmd);
            byte byParam = 0;
            msg.readUnsignedInt8(ref byParam);
            msg.setPos(0);

            mCmdDispInfo.bu = msg;
            mCmdDispInfo.byCmd = byCmd;
            mCmdDispInfo.byParam = byParam;

            foreach (var item in mNetModuleDispList)
            {
                item.handleMsg(mCmdDispInfo);
            }
            //}
        }

        public void addOneRevMsg()
        {
            ++mRevMsgCnt;            
        }

        public void addOneHandleMsg()
        {
            ++mHandleMsgCnt;
        }

        public void clearOneRevMsg()
        {
            mRevMsgCnt = 0;
        }

        public void clearOneHandleMsg()
        {
            mHandleMsgCnt = 0;
        }
    }
}