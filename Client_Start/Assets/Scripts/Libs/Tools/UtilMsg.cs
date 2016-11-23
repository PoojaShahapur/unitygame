using LuaInterface;
using System.Text;

namespace SDK.Lib
{
    /**
     * @brief 处理消息工具
     */
    public class UtilMsg
    {
        // 发送消息， bnet 如果 true 就直接发送到 socket ，否则直接进入输出消息队列
        public static void sendMsg(stNullUserCmd msg, bool bnet = true)
        {
            Ctx.mInstance.mShareData.mTmpBA = Ctx.mInstance.mNetMgr.getSendBA();
            if (Ctx.mInstance.mShareData.mTmpBA != null)
            {
                msg.serialize(Ctx.mInstance.mShareData.mTmpBA);
            }
            else
            {
                Ctx.mInstance.mLogSys.log("socket buffer null");
            }
            if (bnet)
            {
                // 打印日志
                Ctx.mInstance.mShareData.mTmpStr = string.Format("发送消息: byCmd = {0}, byParam = {1}", msg.byCmd, msg.byParam);
                Ctx.mInstance.mLogSys.log(Ctx.mInstance.mShareData.mTmpStr);
            }
            Ctx.mInstance.mNetMgr.send(bnet);
        }

        //static public void sendMsg(ushort commandID, LuaStringBuffer buffer, bool bnet = true)
        static public void sendMsg(ushort commandID, LuaInterface.LuaByteBuffer buffer, bool bnet = true)
        {
            Ctx.mInstance.mShareData.mTmpBA = Ctx.mInstance.mNetMgr.getSendBA();
            if (Ctx.mInstance.mShareData.mTmpBA != null)
            {
                Ctx.mInstance.mShareData.mTmpBA.writeBytes(buffer.buffer, 0, (uint)buffer.buffer.Length);
                Ctx.mInstance.mNetMgr.send(bnet);
            }
        }

        //static public void sendMsgParam(LuaTable luaTable, LuaStringBuffer buffer, bool bnet = true)
        //static public void sendMsgRpc(LuaStringBuffer buffer, bool bnet = true)
        static public void sendMsgRpc(LuaInterface.LuaByteBuffer buffer, bool bnet = true)
        {
            //uint id = UtilLua2CS.getTableAttrUInt(luaTable, "id");
            //string service = UtilLua2CS.getTableAttrStr(luaTable, "service");
            //string method = UtilLua2CS.getTableAttrStr(luaTable, "method");

            Ctx.mInstance.mShareData.mTmpBA = Ctx.mInstance.mNetMgr.getSendBA();
            if (Ctx.mInstance.mShareData.mTmpBA != null)
            {
                //Ctx.mInstance.mShareData.mTmpBA.writeUnsignedInt32(id);
                //Ctx.mInstance.mShareData.mTmpBA.writeMultiByte(service, Encoding.UTF8, 0);
                //Ctx.mInstance.mShareData.mTmpBA.writeMultiByte(method, Encoding.UTF8, 0);

                Ctx.mInstance.mShareData.mTmpBA.writeBytes(buffer.buffer, 0, (uint)buffer.buffer.Length);
                Ctx.mInstance.mNetMgr.send(bnet);
            }
        }

        public static void checkStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                Ctx.mInstance.mLogSys.log("str is null");
            }
        }

        // 格式化消息数据到数组形式
        public static void formatBytes2Array(byte[] bytes, uint len)
        {
            string str = "{ ";
            bool isFirst = true;
            for (int idx = 0; idx < len; ++idx)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    str += ", ";
                }
                str += bytes[idx];
            }

            str += " }";

            Ctx.mInstance.mLogSys.log(str);
        }
    }
}