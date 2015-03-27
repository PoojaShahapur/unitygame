using Game.Msg;
using SDK.Common;

namespace Game.Game
{
    public class GamePropertyUserCmdHandle : NetCmdHandleBase
    {
        public GamePropertyUserCmdHandle()
        {
            m_id2HandleDic[stPropertyUserCmd.REMOVEUSEROBJECT_PROPERTY_USERCMD_PARAMETER] = psstRemoveObjectPropertyUserCmd;
            m_id2HandleDic[stPropertyUserCmd.REFCOUNTOBJECT_PROPERTY_USERCMD_PARAMETER] = psstRefCountObjectPropertyUserCmd;
            m_id2HandleDic[stPropertyUserCmd.ADDUSER_MOBJECT_LIST_PROPERTY_USERCMD_PARAMETER] = psstAddMobileObjectListPropertyUserCmd;
            m_id2HandleDic[stPropertyUserCmd.ADDUSER_MOBJECT_PROPERTY_USERCMD_PARAMETER] = psstAddMobileObjectPropertyUserCmd;
            m_id2HandleDic[stPropertyUserCmd.NOFITY_MARKET_ALL_OBJECT_CMD] = psstNotifyMarketAllObjectPropertyUserCmd;
        }

        protected void psstRemoveObjectPropertyUserCmd(ByteArray msg)
        {
            stRemoveObjectPropertyUserCmd cmd = new stRemoveObjectPropertyUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataPack.psstRemoveObjectPropertyUserCmd(cmd.qwThisID);
        }

        protected void psstRefCountObjectPropertyUserCmd(ByteArray msg)
        {
            stRefCountObjectPropertyUserCmd cmd = new stRefCountObjectPropertyUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataPack.psstRefCountObjectPropertyUserCmd(cmd.qwThisID, cmd.dwNum, cmd.type);
        }

        protected void psstAddMobileObjectListPropertyUserCmd(ByteArray msg)
        {
            stAddMobileObjectListPropertyUserCmd cmd = new stAddMobileObjectListPropertyUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataPack.psstAddMobileObjectListPropertyUserCmd(cmd.list);
        }

        protected void psstAddMobileObjectPropertyUserCmd(ByteArray msg)
        {
            stAddMobileObjectPropertyUserCmd cmd = new stAddMobileObjectPropertyUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataPack.psstAddMobileObjectPropertyUserCmd(cmd.mobject);
        }

        // 服务器返回商城消息
        protected void psstNotifyMarketAllObjectPropertyUserCmd(ByteArray msg)
        {
            stNotifyMarketAllObjectPropertyUserCmd cmd = new stNotifyMarketAllObjectPropertyUserCmd();
            cmd.derialize(msg);
            Ctx.m_instance.m_dataPlayer.m_dataShop.updateShop(cmd.id);
        }
    }
}