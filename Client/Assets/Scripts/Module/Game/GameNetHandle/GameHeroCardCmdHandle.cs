using Game.Msg;
using Game.UI;
using SDK.Common;
using SDK.Lib;

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
            m_id2HandleDic[stHeroCardCmd.ADD_ENEMY_HAND_CARD_PROPERTY_CMD] = psstAddEnemyHandCardPropertyUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_ENEMY_HAND_CARD_NUM_CMD] = psstRetEnemyHandCardNumUserCmd;
            m_id2HandleDic[stHeroCardCmd.DEL_ENEMY_HAND_CARD_PROPERTY_CMD] = psstDelEnemyHandCardPropertyUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_REMOVE_BATTLE_CARD_USERCMD] = psstRetRemoveBattleCardUserCmd;

            m_id2HandleDic[stHeroCardCmd.RET_NOTIFY_UNFINISHED_GAME_CMD] = psstRetNotifyUnfinishedGameUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_REFRESH_CARD_ALL_STATE_CMD] = psstRetRefreshCardAllStateUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_CLEAR_CARD_ONE_STATE_CMD] = psstRetClearCardOneStateUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_SET_CARD_ONE_STATE_CMD] = psstRetSetCardOneStateUserCmd;

            m_id2HandleDic[stHeroCardCmd.RET_HERO_INTO_BATTLE_SCENE_CMD] = psstRetHeroIntoBattleSceneUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_CARD_ATTACK_FAIL_USERCMD_PARA] = psstRetCardAttackFailUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_BATTLE_HISTORY_INFO_CMD] = psstRetBattleHistoryInfoUserCmd;
            m_id2HandleDic[stHeroCardCmd.RET_BATTLE_GAME_RESULT_CMD] = psstRetBattleGameResultUserCmd;
        }

        // 卡牌图鉴中显示的所有数据
        protected void psstNotifyAllCardTujianInfoCmd(ByteArray msg)
        {
            stNotifyAllCardTujianInfoCmd cmd = new stNotifyAllCardTujianInfoCmd();
            cmd.derialize(msg);

            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstNotifyAllCardTujianInfoCmd(cmd.info);
            // 更新界面
            UISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC;
            if (uiSC != null && uiSC.isVisible())
            {
                uiSC.psstNotifyAllCardTujianInfoCmd();
            }
        }

        // 一个卡牌图鉴信息
        protected void psstNotifyOneCardTujianInfoCmd(ByteArray msg)
        {
            stNotifyOneCardTujianInfoCmd cmd = new stNotifyOneCardTujianInfoCmd();
            cmd.derialize(msg);

            bool bhas = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardDic.ContainsKey(cmd.id);
            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstNotifyOneCardTujianInfoCmd(cmd.id, cmd.num);
            // 更新界面
            UISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC;
            if (uiSC != null && uiSC.isVisible())
            {
                uiSC.psstNotifyOneCardTujianInfoCmd(cmd.id, cmd.num, !bhas);
            }
        }

        // 返回开包显示的 5 张牌
        protected void psstRetGiftBagCardsDataUserCmd(ByteArray msg)
        {
            stRetGiftBagCardsDataUserCmd cmd = new stRetGiftBagCardsDataUserCmd();
            cmd.derialize(msg);

            UISceneExtPack uiPack = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneExtPack) as UISceneExtPack;
            if(uiPack != null)
            {
                uiPack.psstRetGiftBagCardsDataUserCmd(cmd.id);
            }
        }

        protected void psstRetCardGroupListInfoUserCmd(ByteArray msg)
        {
            stRetCardGroupListInfoUserCmd cmd = new stRetCardGroupListInfoUserCmd();
            cmd.derialize(msg);

            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstRetCardGroupListInfoUserCmd(cmd.info);
            // 更新界面
            UISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC;
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

        protected void psstRetOneCardGroupInfoUserCmd(ByteArray msg)
        {
            stRetOneCardGroupInfoUserCmd cmd = new stRetOneCardGroupInfoUserCmd();
            cmd.derialize(msg);
            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstRetOneCardGroupInfoUserCmd(cmd);
            // 更新界面
            UISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC;
            if(uiSC != null && uiSC.isVisible())
            {
                uiSC.psstRetOneCardGroupInfoUserCmd(cmd.index, cmd.id);
            }
        }

        // 创建一个套牌
        protected void psstRetCreateOneCardGroupUserCmd(ByteArray msg)
        {
            stRetCreateOneCardGroupUserCmd cmd = new stRetCreateOneCardGroupUserCmd();
            cmd.derialize(msg);

            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstRetCreateOneCardGroupUserCmd(cmd);
            // 更新界面
            UISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC;
            if (uiSC != null && uiSC.isVisible())
            {
                uiSC.psstRetCreateOneCardGroupUserCmd(Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardGroupDic[cmd.index]);
            }
        }

        protected void psstRetDeleteOneCardGroupUserCmd(ByteArray msg)
        {
            stRetDeleteOneCardGroupUserCmd cmd = new stRetDeleteOneCardGroupUserCmd();
            cmd.derialize(msg);

            if(cmd.success > 0)
            {
                // 更新数据
                Ctx.m_instance.m_dataPlayer.m_dataCard.psstRetDeleteOneCardGroupUserCmd(cmd.index);
                // 更新界面
                UISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC;
                if (uiSC != null && uiSC.isVisible())
                {
                    uiSC.psstRetDeleteOneCardGroupUserCmd(cmd.index);
                }
            }
        }

        protected void psstRetSaveOneCardGroupUserCmd(ByteArray msg)
        {
            stRetSaveOneCardGroupUserCmd cmd = new stRetSaveOneCardGroupUserCmd();
            cmd.derialize(msg);

            if(cmd.success > 0)
            {
                UISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as UISceneWDSC;
                if (uiSC != null && uiSC.isVisible())
                {
                    uiSC.psstRetSaveOneCardGroupUserCmd(cmd.index);
                }
            }
        }

        protected void psstRetAllHeroInfoUserCmd(ByteArray msg)
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

        // 返回一个 hero 信息
        protected void psstRetOneHeroInfoUserCmd(ByteArray msg)
        {
            stRetOneHeroInfoUserCmd cmd = new stRetOneHeroInfoUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataHero.psstRetOneHeroInfoUserCmd(cmd.info);
        }

        // 返回匹配结果
        protected void psstRetHeroFightMatchUserCmd(ByteArray msg)
        {
            stRetHeroFightMatchUserCmd cmd = new stRetHeroFightMatchUserCmd();
            cmd.derialize(msg);

            // 显示匹配结果
            UISceneMoShi uiMS = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneMoShi) as UISceneMoShi;
            if (uiMS != null && uiMS.isVisible())
            {
                uiMS.psstRetHeroFightMatchUserCmd(cmd);
            }
        }

        // 返回进入战斗场景消息
        protected void psstRetHeroIntoBattleSceneUserCmd(ByteArray msg)
        {
            Ctx.m_instance.m_gameSys.loadDZScene();
        }

        // 回归剩余卡牌数量
        protected void psstRetLeftCardLibNumUserCmd(ByteArray msg)
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

        // 返回 magic 点的数量
        protected void psstRetMagicPointInfoUserCmd(ByteArray ba)
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

        // 刷新战斗状态
        protected void psstRetRefreshBattleStateUserCmd(ByteArray ba)
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

        // 返回当前谁出牌
        protected void psstRetRefreshBattlePrivilegeUserCmd(ByteArray ba)
        {
            // 增加当前出牌次数
            ++Ctx.m_instance.m_dataPlayer.m_dzData.curPlayCardCount;

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
        protected void psstAddBattleCardPropertyUserCmd(ByteArray ba)
        {
            stAddBattleCardPropertyUserCmd cmd = new stAddBattleCardPropertyUserCmd();
            cmd.derialize(ba);

            Ctx.m_instance.m_log.log(string.Format("添加一个卡牌 thisid: {0}", cmd.mobject.qwThisID));

            SceneCardItem sceneItem = null;
            if (cmd.byActionType == 1)
            {
                sceneItem = new SceneCardItem();
                sceneItem.m_svrCard = cmd.mobject;
                sceneItem.curSlot = cmd.slot;
                sceneItem.m_cardArea = (CardArea)cmd.slot;
                sceneItem.m_playerFlag = (EnDZPlayer)(cmd.who - 1);
                sceneItem.m_cardTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, sceneItem.m_svrCard.dwObjectID).m_itemBody as TableCardItemBody;
                // 填充数据
                Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[cmd.who - 1].addOneSceneCard(sceneItem);       // 添加数据
            }
            else
            {
                sceneItem = Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[cmd.who - 1].updateCardInfoByCardItem(cmd.mobject);
            }

            if (sceneItem != null)      // 更新或者添加都需要这个数据必须存在
            {
                UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
                if (uiDZ != null && uiDZ.isVisible())
                {
                    uiDZ.psstAddBattleCardPropertyUserCmd(cmd, sceneItem);
                }
            }
        }

        // 对方信息
        protected void psstNotifyFightEnemyInfoUserCmd(ByteArray ba)
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
        protected void psstRetFirstHandCardUserCmd(ByteArray ba)
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

        // 卡牌移动
        protected void psstRetMoveGameCardUserCmd(ByteArray ba)
        {
            stRetMoveGameCardUserCmd cmd = new stRetMoveGameCardUserCmd();
            cmd.derialize(ba);

            if (cmd.success == 1)
            {
                // 更新数据
                cmd.side = Ctx.m_instance.m_dataPlayer.m_dzData.updateCardInfo(cmd);
            }

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetMoveGameCardUserCmd(cmd);
            }
        }

        // 通知手里卡牌已经满
        protected void psstRetNotifyHandIsFullUserCmd(ByteArray ba)
        {
            stRetNotifyHandIsFullUserCmd cmd = new stRetNotifyHandIsFullUserCmd();
            cmd.derialize(ba);

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetNotifyHandIsFullUserCmd(cmd);
            }
        }

        // enemy 增加一个卡牌
        protected void psstAddEnemyHandCardPropertyUserCmd(ByteArray ba)
        {
            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstAddEnemyHandCardPropertyUserCmd();
            }
        }

        // 返回对方手里的卡牌数量，对方卡牌只有数量
        protected void psstRetEnemyHandCardNumUserCmd(ByteArray ba)
        {
            stRetEnemyHandCardNumUserCmd cmd = new stRetEnemyHandCardNumUserCmd();
            cmd.derialize(ba);

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetEnemyHandCardNumUserCmd(cmd);
            }
        }

        // 删除对方手里一张卡牌
        protected void psstDelEnemyHandCardPropertyUserCmd(ByteArray ba)
        {
            stDelEnemyHandCardPropertyUserCmd cmd = new stDelEnemyHandCardPropertyUserCmd();
            cmd.derialize(ba);

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstDelEnemyHandCardPropertyUserCmd(cmd);
            }
        }

        // 从已经出牌区域删除一个卡牌
        protected void psstRetRemoveBattleCardUserCmd(ByteArray ba)
        {
            stRetRemoveBattleCardUserCmd cmd = new stRetRemoveBattleCardUserCmd();
            cmd.derialize(ba);

            Ctx.m_instance.m_log.log(string.Format("删除一个卡牌 thisid: {0}", cmd.dwThisID));

            int side = 0;
            SceneCardItem sceneItem = null;
            if (!Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[0].removeOneSceneCardByThisID(cmd.dwThisID, ref sceneItem))
            {
                side = 1;
                Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[1].removeOneSceneCardByThisID(cmd.dwThisID, ref sceneItem);
            }
            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                if (sceneItem != null)
                {
                    uiSceneDZ.psstRetRemoveBattleCardUserCmd(cmd, side, sceneItem);
                }
            }
        }

        // 通知客户端上一场战斗还没有结束
        protected void psstRetNotifyUnfinishedGameUserCmd(ByteArray ba)
        {
            Ctx.m_instance.m_dataPlayer.m_dzData.m_bLastEnd = false;
            // 可能要请求一些数据
            stReqEnterUnfinishedGameUserCmd cmd = new stReqEnterUnfinishedGameUserCmd();
            UtilMsg.sendMsg(cmd);
            Ctx.m_instance.m_dataPlayer.m_dzData.m_bLastEnd = true;
        }

        // 某一局开始后，刷新这一局的所有的状态
        protected void psstRetRefreshCardAllStateUserCmd(ByteArray ba)
        {
            stRetRefreshCardAllStateUserCmd cmd = new stRetRefreshCardAllStateUserCmd();
            cmd.derialize(ba);

            SceneCardItem cardItem = Ctx.m_instance.m_dataPlayer.m_dzData.getCardItemByThisIDAndSide(cmd.dwThisID, (byte)(cmd.who - 1));
            if (cardItem != null)
            {
                cardItem.m_svrCard.state = cmd.state;
            }
        }

        // 清除某一个状态
        protected void psstRetClearCardOneStateUserCmd(ByteArray ba)
        {
            stRetClearCardOneStateUserCmd cmd = new stRetClearCardOneStateUserCmd();
            cmd.derialize(ba);

            SceneCardItem cardItem = Ctx.m_instance.m_dataPlayer.m_dzData.getCardItemByThisIDAndSide(cmd.dwThisID, cmd.who);
            UtilMath.clearState((StateID)cmd.stateNum, cardItem.m_svrCard.state);
        }

        // 设置某一个状态
        protected void psstRetSetCardOneStateUserCmd(ByteArray ba)
        {
            stRetSetCardOneStateUserCmd cmd = new stRetSetCardOneStateUserCmd();
            cmd.derialize(ba);

            SceneCardItem cardItem = Ctx.m_instance.m_dataPlayer.m_dzData.getCardItemByThisIDAndSide(cmd.dwThisID, (byte)(cmd.who - 1));
            UtilMath.setState((StateID)cmd.stateNum, cardItem.m_svrCard.state);
        }

        // 法术攻击失败
        protected void psstRetCardAttackFailUserCmd(ByteArray ba)
        {
            stRetCardAttackFailUserCmd cmd = new stRetCardAttackFailUserCmd();
            cmd.derialize(ba);
            // 将不能使用的法术牌退回去
            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetCardAttackFailUserCmd(cmd);
            }
        }

        protected void psstRetBattleHistoryInfoUserCmd(ByteArray ba)
        {
            stRetBattleHistoryInfoUserCmd cmd = new stRetBattleHistoryInfoUserCmd();
            cmd.derialize(ba);

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetBattleHistoryInfoUserCmd(cmd);
            }
        }

        protected void psstRetBattleGameResultUserCmd(ByteArray ba)
        {
            stRetBattleGameResultUserCmd cmd = new stRetBattleGameResultUserCmd();
            cmd.derialize(ba);

            if (cmd.win == 1 || cmd.win == 0)        // 赢了输了
            {
                Ctx.m_instance.m_gameSys.loadGameScene();        // 加载游戏场景
            }
        }
    }
}