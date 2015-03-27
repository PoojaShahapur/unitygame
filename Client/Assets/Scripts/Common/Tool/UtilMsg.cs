using SDK.Lib;

namespace SDK.Common
{
    /**
     * @brief 处理消息工具
     */
    public class UtilMsg
    {
        // 发送消息， bnet 如果 true 就直接发送到 socket ，否则直接进入输出消息队列
        public static void sendMsg(stNullUserCmd msg, bool bnet = true)
        {
            Ctx.m_instance.m_shareMgr.m_tmpBA = Ctx.m_instance.m_netMgr.getSendBA();
            if (Ctx.m_instance.m_shareMgr.m_tmpBA != null)
            {
                msg.serialize(Ctx.m_instance.m_shareMgr.m_tmpBA);
            }
            else
            {
                Ctx.m_instance.m_log.log("socket buffer null");
            }
            if (bnet)
            {
                // 打印日志
                Ctx.m_instance.m_shareMgr.m_tmpStr = string.Format("发送消息: byCmd = {0}, byParam = {1}", msg.byCmd, msg.byParam);
                Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareMgr.m_tmpStr);
            }
            Ctx.m_instance.m_netMgr.send(bnet);
        }

        public static void checkStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                Ctx.m_instance.m_log.log("str is null");
            }
        }
    }
}