using Game.Msg;
using Game.UI;
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
            m_id2HandleDic[stHeroCardCmd.RET_CARD_GROUP_LIST_INFO_CMD] = psstRetCardGroupListInfoUserCmd;

            m_id2HandleDic[stHeroCardCmd.RET_ONE_CARD_GROUP_INFO_CMD] = psstRetOneCardGroupInfoUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_CREATE_ONE_CARD_GROUP_CMD] = psstRetCreateOneCardGroupUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_SAVE_ONE_CARD_GROUP_CMD] = psstRetSaveOneCardGroupUserCmd;

            m_id2HandleDic[stHeroCardCmd.RET_ALL_HERO_INFO_CMD] = psstRetAllHeroInfoUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_ONE_HERO_INFO_CMD] = psstRetOneHeroInfoUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_HERO_FIGHT_MATCH_CMD] = psstRetHeroFightMatchUserCmd;
        }

        protected void psstNotifyAllCardTujianInfoCmd(IByteArray msg)
        {
            stNotifyAllCardTujianInfoCmd cmd = new stNotifyAllCardTujianInfoCmd();
            cmd.derialize(msg);

            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstNotifyAllCardTujianInfoCmd(cmd.info);
            // 更新界面
            IUISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as IUISceneWDSC;
            if (uiSC != null && uiSC.isVisible())
            {
                uiSC.psstNotifyAllCardTujianInfoCmd();
            }
        }

        protected void psstNotifyOneCardTujianInfoCmd(IByteArray msg)
        {
            stNotifyOneCardTujianInfoCmd cmd = new stNotifyOneCardTujianInfoCmd();
            cmd.derialize(msg);

            bool bhas = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardDic.ContainsKey(cmd.id);
            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstNotifyOneCardTujianInfoCmd(cmd.id, cmd.num);
            // 更新界面
            IUISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as IUISceneWDSC;
            if (uiSC != null && uiSC.isVisible())
            {
                uiSC.psstNotifyOneCardTujianInfoCmd(cmd.id, cmd.num, !bhas);
            }
        }

        // 返回开包显示的 5 张牌
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

        protected void psstRetCardGroupListInfoUserCmd(IByteArray msg)
        {
            stRetCardGroupListInfoUserCmd cmd = new stRetCardGroupListInfoUserCmd();
            cmd.derialize(msg);

            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstRetCardGroupListInfoUserCmd(cmd.info);
            // 更新界面
            IUISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as IUISceneWDSC;
            if(uiSC != null && uiSC.isVisible())
            {
                uiSC.psstRetCardGroupListInfoUserCmd();
            }
        }

        protected void psstRetOneCardGroupInfoUserCmd(IByteArray msg)
        {
            stRetOneCardGroupInfoUserCmd cmd = new stRetOneCardGroupInfoUserCmd();
            cmd.derialize(msg);
            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstRetOneCardGroupInfoUserCmd(cmd);
            // 更新界面
            IUISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as IUISceneWDSC;
            if(uiSC != null && uiSC.isVisible())
            {
                uiSC.psstRetOneCardGroupInfoUserCmd(cmd.index, cmd.id);
            }
        }

        // 创建一个套牌
        protected void psstRetCreateOneCardGroupUserCmd(IByteArray msg)
        {
            stRetCreateOneCardGroupUserCmd cmd = new stRetCreateOneCardGroupUserCmd();
            cmd.derialize(msg);

            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstRetCreateOneCardGroupUserCmd(cmd);
            // 更新界面
            IUISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as IUISceneWDSC;
            if (uiSC != null && uiSC.isVisible())
            {
                uiSC.psstRetCreateOneCardGroupUserCmd(cmd);
            }
        }

        protected void psstRetDeleteOneCardGroupUserCmd(IByteArray msg)
        {
            stRetDeleteOneCardGroupUserCmd cmd = new stRetDeleteOneCardGroupUserCmd();
            cmd.derialize(msg);

            if(cmd.success > 0)
            {
                // 更新数据
                Ctx.m_instance.m_dataPlayer.m_dataCard.psstRetDeleteOneCardGroupUserCmd(cmd.index);
                // 更新界面
                IUISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as IUISceneWDSC;
                if (uiSC != null && uiSC.isVisible())
                {
                    uiSC.psstRetDeleteOneCardGroupUserCmd(cmd.index);
                }
            }
        }

        protected void psstRetSaveOneCardGroupUserCmd(IByteArray msg)
        {
            stRetSaveOneCardGroupUserCmd cmd = new stRetSaveOneCardGroupUserCmd();
            cmd.derialize(msg);

            if(cmd.success > 0)
            {
                IUISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as IUISceneWDSC;
                if (uiSC != null && uiSC.isVisible())
                {
                    uiSC.psstRetSaveOneCardGroupUserCmd(cmd.index);
                }
            }
        }

        protected void psstRetAllHeroInfoUserCmd(IByteArray msg)
        {
            stRetAllHeroInfoUserCmd cmd = new stRetAllHeroInfoUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataHero.psstRetAllHeroInfoUserCmd(cmd.info);

            UISceneHero uiSH = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneHero) as UISceneHero;
            if (uiSH != null && uiSH.isVisible())
            {
                uiSH.updateAllHero();
            }
        }

        protected void psstRetOneHeroInfoUserCmd(IByteArray msg)
        {
            stRetOneHeroInfoUserCmd cmd = new stRetOneHeroInfoUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataHero.psstRetOneHeroInfoUserCmd(cmd.info);
        }

        protected void psstRetHeroFightMatchUserCmd(IByteArray msg)
        {
            stRetHeroFightMatchUserCmd cmd = new stRetHeroFightMatchUserCmd();
            cmd.derialize(msg);
        }
    }
}