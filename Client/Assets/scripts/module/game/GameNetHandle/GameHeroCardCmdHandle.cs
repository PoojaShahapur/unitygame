using Game.Msg;
using SDK.Common;

namespace Game.Game
{
    public class GameHeroCardCmdHandle : NetCmdHandleBase
    {
        public GameHeroCardCmdHandle()
        {
            m_id2HandleDic[stHeroCardCmd.NOFITY_ALL_CARD_TUJIAN_INFO_CMD] = psstNotifyAllCardTujianInfoCmd;
            m_id2HandleDic[stHeroCardCmd.NOFITY_ONE_CARD_TUJIAN_INFO_CMD] = psstNotifyOneCardTujianInfoCmd;
            m_id2HandleDic[stHeroCardCmd.RET_GIFTBAG_CARDS_DATA_CMD] = psstRetGiftBagCardsDataUserCmd;
        }

        protected void psstNotifyAllCardTujianInfoCmd(IByteArray msg)
        {
            stNotifyAllCardTujianInfoCmd cmd = new stNotifyAllCardTujianInfoCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataCard.psstNotifyAllCardTujianInfoCmd(cmd.info);
        }

        protected void psstNotifyOneCardTujianInfoCmd(IByteArray msg)
        {
            stNotifyOneCardTujianInfoCmd cmd = new stNotifyOneCardTujianInfoCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataCard.psstNotifyOneCardTujianInfoCmd(cmd.id, cmd.num);
        }

        protected void psstRetGiftBagCardsDataUserCmd(IByteArray msg)
        {
            stRetGiftBagCardsDataUserCmd cmd = new stRetGiftBagCardsDataUserCmd();
            cmd.derialize(msg);

            IUISceneExtPack uiPack = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneExtPack) as IUISceneExtPack;
            if(uiPack != null)
            {
                uiPack.psstRetGiftBagCardsDataUserCmd(cmd.id);
            }
        }
    }
}