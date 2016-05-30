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
            Ctx.m_instance.m_shareData.m_tmpBA = Ctx.m_instance.m_netMgr.getSendBA();
            if (Ctx.m_instance.m_shareData.m_tmpBA != null)
            {
                msg.serialize(Ctx.m_instance.m_shareData.m_tmpBA);
            }
            else
            {
                Ctx.m_instance.m_logSys.log("socket buffer null");
            }
            if (bnet)
            {
                // 打印日志
                Ctx.m_instance.m_shareData.m_tmpStr = string.Format("发送消息: byCmd = {0}, byParam = {1}", msg.byCmd, msg.byParam);
                Ctx.m_instance.m_logSys.log(Ctx.m_instance.m_shareData.m_tmpStr);
            }
            Ctx.m_instance.m_netMgr.send(bnet);
        }

        static public void sendMsg(ushort commandID, LuaStringBuffer buffer, bool bnet = true)
        {
            Ctx.m_instance.m_shareData.m_tmpBA = Ctx.m_instance.m_netMgr.getSendBA();
            if (Ctx.m_instance.m_shareData.m_tmpBA != null)
            {
                Ctx.m_instance.m_shareData.m_tmpBA.writeBytes(buffer.buffer, 0, (uint)buffer.buffer.Length);
                Ctx.m_instance.m_netMgr.send(bnet);
            }
        }

        //static public void sendMsgParam(LuaTable luaTable, LuaStringBuffer buffer, bool bnet = true)
        static public void sendMsgRpc(LuaStringBuffer buffer, bool bnet = true)
        {
            //uint id = UtilLua2CS.getTableAttrUInt(luaTable, "id");
            //string service = UtilLua2CS.getTableAttrStr(luaTable, "service");
            //string method = UtilLua2CS.getTableAttrStr(luaTable, "method");

            Ctx.m_instance.m_shareData.m_tmpBA = Ctx.m_instance.m_netMgr.getSendBA();
            if (Ctx.m_instance.m_shareData.m_tmpBA != null)
            {
                //Ctx.m_instance.m_shareData.m_tmpBA.writeUnsignedInt32(id);
                //Ctx.m_instance.m_shareData.m_tmpBA.writeMultiByte(service, Encoding.UTF8, 0);
                //Ctx.m_instance.m_shareData.m_tmpBA.writeMultiByte(method, Encoding.UTF8, 0);

                Ctx.m_instance.m_shareData.m_tmpBA.writeBytes(buffer.buffer, 0, (uint)buffer.buffer.Length);
                Ctx.m_instance.m_netMgr.send(bnet);
            }
        }

        public static void checkStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                Ctx.m_instance.m_logSys.log("str is null");
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

            Ctx.m_instance.m_logSys.log(str);
        }
    }
}