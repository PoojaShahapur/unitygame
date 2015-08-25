using Game.Msg;
using Game.UI;
using SDK.Lib;
using SDK.Lib;

namespace Game.Game
{
    public class GameDataUserCmdHandle : NetCmdHandleBase
    {
        public GameDataUserCmdHandle()
        {
            m_id2HandleDic[stDataUserCmd.MAIN_USER_DATA_USERCMD_PARA] = psstMainUserDataUserCmd;
        }

        // 如果要调试，可以重载，方便调试
        //public override void handleMsg(ByteBuffer ba, byte byCmd, byte byParam)
        //{
        //
        //}

        protected void psstMainUserDataUserCmd(ByteBuffer msg)
        {
            stMainUserDataUserCmd cmd = new stMainUserDataUserCmd();
            cmd.derialize(msg);
            Ctx.m_instance.m_dataPlayer.m_dataMain = cmd.data;

            if (!Ctx.m_instance.m_uiMgr.hasForm(UIFormID.eUIShop))
            {
                Ctx.m_instance.m_uiMgr.loadForm(UIFormID.eUIShop);
            }

            IUIShop shop = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIShop) as IUIShop;
            if (shop != null)
            {
                shop.UpdateGoldNum(cmd.data.m_gold);
            }

            Ctx.m_instance.m_logSys.log(string.Format("接收到主数据，money = {0}", cmd.data.m_gold));
        }
    }
}