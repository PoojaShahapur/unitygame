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
            m_id2HandleDic[stHeroCardCmd.RET_DELETE_ONE_CARD_GROUP_CMD] = psstRetDeleteOneCardGroupUserCmd;

            m_id2HandleDic[stHeroCardCmd.RET_LEFT_CARDLIB_NUM_CMD] = psstRetLeftCardLibNumUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_MAGIC_POINT_INFO_CMD] = psstRetMagicPointInfoUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_REFRESH_BATTLE_STATE_CMD] = psstRetRefreshBattleStateUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_REFRESH_BATTLE_PRIVILEGE_CMD] = psstRetRefreshBattlePrivilegeUserCmd;

            m_id2HandleDic[stHeroCardCmd.ADD_BATTLE_CARD_PROPERTY_CMD] = psstAddBattleCardPropertyUserCmd;
            m_id2HandleDic[stHeroCardCmd.NOTIFY_FIGHT_ENEMY_INFO_CMD] = psstNotifyFightEnemyInfoUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_FIRST_HAND_CARD_CMD] = psstRetFirstHandCardUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_MOVE_CARD_USERCMD_PARAMETER] = psstRetMoveGameCardUserCmd;

            m_id2HandleDic[stHeroCardCmd.RET_NOTIFY_HAND_IS_FULL_CMD] = psstRetNotifyHandIsFullUserCmd;
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

            UISceneMoShi uiMS = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneMoShi) as UISceneMoShi;
            if (uiMS != null && uiMS.isVisible())
            {
                uiMS.updateHeroList();
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
                uiSC.psstRetCreateOneCardGroupUserCmd(Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardGroupDic[cmd.index]);
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

            // 显示匹配结果
            UISceneMoShi uiMS = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneMoShi) as UISceneMoShi;
            if (uiMS != null && uiMS.isVisible())
            {
                uiMS.psstRetHeroFightMatchUserCmd(cmd);
            }

            Ctx.m_instance.m_loadDZScene();
        }

        protected void psstRetLeftCardLibNumUserCmd(IByteArray msg)
        {
            stRetLeftCardLibNumUserCmd cmd = new stRetLeftCardLibNumUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_leftCardNum = cmd.selfNum;
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerEnemy].m_leftCardNum = cmd.otherNum;

            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiDZ != null && uiDZ.isVisible())
            {
                uiDZ.psstRetLeftCardLibNumUserCmd(cmd);
            }
        }

        protected void psstRetMagicPointInfoUserCmd(IByteArray ba)
        {
            stRetMagicPointInfoUserCmd cmd = new stRetMagicPointInfoUserCmd();
            cmd.derialize(ba);

            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroMagicPoint = cmd.self;
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerEnemy].m_heroMagicPoint = cmd.other;

            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiDZ != null && uiDZ.isVisible())
            {
                uiDZ.psstRetMagicPointInfoUserCmd(cmd);
            }
        }

        protected void psstRetRefreshBattleStateUserCmd(IByteArray ba)
        {
            stRetRefreshBattleStateUserCmd cmd = new stRetRefreshBattleStateUserCmd();
            cmd.derialize(ba);

            Ctx.m_instance.m_dataPlayer.m_dzData.m_state = cmd.state;

            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiDZ != null && uiDZ.isVisible())
            {
                uiDZ.psstRetRefreshBattleStateUserCmd(cmd);
            }
        }

        protected void psstRetRefreshBattlePrivilegeUserCmd(IByteArray ba)
        {
            stRetRefreshBattlePrivilegeUserCmd cmd = new stRetRefreshBattlePrivilegeUserCmd();
            cmd.derialize(ba);

            Ctx.m_instance.m_dataPlayer.m_dzData.m_priv = cmd.priv;

            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiDZ != null && uiDZ.isVisible())
            {
                uiDZ.psstRetRefreshBattlePrivilegeUserCmd(cmd);
            }
        }

        // 添加一个卡牌
        protected void psstAddBattleCardPropertyUserCmd(IByteArray ba)
        {
            stAddBattleCardPropertyUserCmd cmd = new stAddBattleCardPropertyUserCmd();
            cmd.derialize(ba);

            SceneCardItem sceneItem = new SceneCardItem();
            sceneItem.m_svrCard = cmd.mobject;
            sceneItem.m_slot = cmd.slot;
            // 填充数据
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[cmd.who - 1].m_sceneCardList.Add(sceneItem);       // 添加数据

            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiDZ != null && uiDZ.isVisible())
            {
                uiDZ.psstAddBattleCardPropertyUserCmd(cmd, sceneItem);
            }
        }

        protected void psstNotifyFightEnemyInfoUserCmd(IByteArray ba)
        {
            stNotifyFightEnemyInfoUserCmd cmd = new stNotifyFightEnemyInfoUserCmd();
            cmd.derialize(ba);

            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerEnemy].m_heroName = cmd.name;
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerEnemy].m_heroOccupation = cmd.occupation;

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstNotifyFightEnemyInfoUserCmd(cmd);
            }

            UIDZ uiDZ = Ctx.m_instance.m_uiMgr.getForm(UIFormID.UIDZ) as UIDZ;
            if (uiDZ != null)
            {
                uiDZ.psstNotifyFightEnemyInfoUserCmd();
            }
        }

        // 第一次几张牌
        protected void psstRetFirstHandCardUserCmd(IByteArray ba)
        {
            stRetFirstHandCardUserCmd cmd = new stRetFirstHandCardUserCmd();
            cmd.derialize(ba);

            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList = cmd.id;

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetFirstHandCardUserCmd(cmd);
            }
        }

        protected void psstRetMoveGameCardUserCmd(IByteArray ba)
        {
            stRetMoveGameCardUserCmd cmd = new stRetMoveGameCardUserCmd();
            cmd.derialize(ba);

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetMoveGameCardUserCmd(cmd);
            }
        }

        protected void psstRetNotifyHandIsFullUserCmd(IByteArray ba)
        {
            stRetNotifyHandIsFullUserCmd cmd = new stRetNotifyHandIsFullUserCmd();
            cmd.derialize(ba);
        }
    }
}