using Game.Msg;
using SDK.Lib;

namespace Game.Game
{
    public class GamePropertyUserCmdHandle : NetCmdHandleBase
    {
        public GamePropertyUserCmdHandle()
        {
            this.addParamHandle(stPropertyUserCmd.REMOVEUSEROBJECT_PROPERTY_USERCMD_PARAMETER, psstRemoveObjectPropertyUserCmd);
            this.addParamHandle(stPropertyUserCmd.REFCOUNTOBJECT_PROPERTY_USERCMD_PARAMETER, psstRefCountObjectPropertyUserCmd);
            this.addParamHandle(stPropertyUserCmd.ADDUSER_MOBJECT_LIST_PROPERTY_USERCMD_PARAMETER, psstAddMobileObjectListPropertyUserCmd);
            this.addParamHandle(stPropertyUserCmd.ADDUSER_MOBJECT_PROPERTY_USERCMD_PARAMETER, psstAddMobileObjectPropertyUserCmd);
            this.addParamHandle(stPropertyUserCmd.NOFITY_MARKET_ALL_OBJECT_CMD, psstNotifyMarketAllObjectPropertyUserCmd);
            this.addParamHandle(stPropertyUserCmd.REQ_BUY_MARKET_MOBILE_OBJECT_CMD, psstReqBuyMobileObjectPropertyUserCmd);
        }

        protected void psstRemoveObjectPropertyUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

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

        protected void psstRefCountObjectPropertyUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

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

        protected void psstAddMobileObjectListPropertyUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stAddMobileObjectListPropertyUserCmd cmd = new stAddMobileObjectListPropertyUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataPack.psstAddMobileObjectListPropertyUserCmd(cmd.list);
        }

        protected void psstAddMobileObjectPropertyUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

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
        protected void psstNotifyMarketAllObjectPropertyUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stNotifyMarketAllObjectPropertyUserCmd cmd = new stNotifyMarketAllObjectPropertyUserCmd();
            cmd.derialize(msg);
            Ctx.m_instance.m_dataPlayer.m_dataShop.updateShop(cmd.id);
        }

        protected void psstReqBuyMobileObjectPropertyUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stReqBuyMobileObjectPropertyUserCmd cmd = new stReqBuyMobileObjectPropertyUserCmd();
            cmd.derialize(msg);

           //给与购买成功提示
        }
    }
}