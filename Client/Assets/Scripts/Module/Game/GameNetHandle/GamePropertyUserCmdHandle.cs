using Game.Msg;
using SDK.Common;
using SDK.Lib;
using Game.UI;

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
            m_id2HandleDic[stPropertyUserCmd.REQ_BUY_MARKET_MOBILE_OBJECT_CMD] = psstReqBuyMobileObjectPropertyUserCmd;
        }

        protected void psstRemoveObjectPropertyUserCmd(ByteBuffer msg)
        {
            stRemoveObjectPropertyUserCmd cmd = new stRemoveObjectPropertyUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataPack.psstRemoveObjectPropertyUserCmd(cmd.qwThisID);

            IUIOpenPack openPack = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIOpenPack) as IUIOpenPack;
            if (openPack != null)
            {
                openPack.updateData();
                openPack.updatePackNum();
            }
        }

        protected void psstRefCountObjectPropertyUserCmd(ByteBuffer msg)
        {
            stRefCountObjectPropertyUserCmd cmd = new stRefCountObjectPropertyUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataPack.psstRefCountObjectPropertyUserCmd(cmd.qwThisID, cmd.dwNum, cmd.type);

            IUIOpenPack openPack = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIOpenPack) as IUIOpenPack;
            if (openPack != null)
            {
                openPack.updateData();
                openPack.updatePackNum();
            }
        }

        protected void psstAddMobileObjectListPropertyUserCmd(ByteBuffer msg)
        {
            stAddMobileObjectListPropertyUserCmd cmd = new stAddMobileObjectListPropertyUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataPack.psstAddMobileObjectListPropertyUserCmd(cmd.list);
        }

        protected void psstAddMobileObjectPropertyUserCmd(ByteBuffer msg)
        {
            stAddMobileObjectPropertyUserCmd cmd = new stAddMobileObjectPropertyUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataPack.psstAddMobileObjectPropertyUserCmd(cmd.mobject);

            IUIOpenPack openPack = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIOpenPack) as IUIOpenPack;
            if (openPack != null)
            {
                openPack.updateData();
                openPack.updatePackNum();
            }
        }

        // 服务器返回商城消息
        protected void psstNotifyMarketAllObjectPropertyUserCmd(ByteBuffer msg)
        {
            stNotifyMarketAllObjectPropertyUserCmd cmd = new stNotifyMarketAllObjectPropertyUserCmd();
            cmd.derialize(msg);
            Ctx.m_instance.m_dataPlayer.m_dataShop.updateShop(cmd.id);
        }

        protected void psstReqBuyMobileObjectPropertyUserCmd(ByteBuffer msg)
        {
            stReqBuyMobileObjectPropertyUserCmd cmd = new stReqBuyMobileObjectPropertyUserCmd();
            cmd.derialize(msg);

           //给与购买成功提示
        }
    }
}