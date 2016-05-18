using Fight;
using FightCore;
using Game.Msg;
using SDK.Lib;

namespace Game.Game
{
    public class GameHeroCardCmdHandle : NetCmdHandleBase
    {
        public GameHeroCardCmdHandle()
        {
            this.addParamHandle(stHeroCardCmd.NOFITY_ALL_CARD_TUJIAN_INFO_CMD, psstNotifyAllCardTujianInfoCmd);
            this.addParamHandle(stHeroCardCmd.NOFITY_ONE_CARD_TUJIAN_INFO_CMD, psstNotifyOneCardTujianInfoCmd);
            this.addParamHandle(stHeroCardCmd.RET_GIFTBAG_CARDS_DATA_CMD, psstRetGiftBagCardsDataUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_CARD_GROUP_LIST_INFO_CMD, psstRetCardGroupListInfoUserCmd);

            this.addParamHandle(stHeroCardCmd.RET_ONE_CARD_GROUP_INFO_CMD, psstRetOneCardGroupInfoUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_CREATE_ONE_CARD_GROUP_CMD, psstRetCreateOneCardGroupUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_SAVE_ONE_CARD_GROUP_CMD, psstRetSaveOneCardGroupUserCmd);

            this.addParamHandle(stHeroCardCmd.RET_ALL_HERO_INFO_CMD, psstRetAllHeroInfoUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_ONE_HERO_INFO_CMD, psstRetOneHeroInfoUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_HERO_FIGHT_MATCH_CMD, psstRetHeroFightMatchUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_DELETE_ONE_CARD_GROUP_CMD, psstRetDeleteOneCardGroupUserCmd);

            this.addParamHandle(stHeroCardCmd.RET_LEFT_CARDLIB_NUM_CMD, psstRetLeftCardLibNumUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_MAGIC_POINT_INFO_CMD, psstRetMagicPointInfoUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_REFRESH_BATTLE_STATE_CMD, psstRetRefreshBattleStateUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_REFRESH_BATTLE_PRIVILEGE_CMD, psstRetRefreshBattlePrivilegeUserCmd);

            this.addParamHandle(stHeroCardCmd.ADD_BATTLE_CARD_PROPERTY_CMD, psstAddBattleCardPropertyUserCmd);
            this.addParamHandle(stHeroCardCmd.NOTIFY_FIGHT_ENEMY_INFO_CMD, psstNotifyFightEnemyInfoUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_FIRST_HAND_CARD_CMD, psstRetFirstHandCardUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_MOVE_CARD_USERCMD_PARAMETER, psstRetMoveGameCardUserCmd);

            this.addParamHandle(stHeroCardCmd.RET_NOTIFY_HAND_IS_FULL_CMD, psstRetNotifyHandIsFullUserCmd);
            this.addParamHandle(stHeroCardCmd.ADD_ENEMY_HAND_CARD_PROPERTY_CMD, psstAddEnemyHandCardPropertyUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_ENEMY_HAND_CARD_NUM_CMD, psstRetEnemyHandCardNumUserCmd);
            this.addParamHandle(stHeroCardCmd.DEL_ENEMY_HAND_CARD_PROPERTY_CMD, psstDelEnemyHandCardPropertyUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_REMOVE_BATTLE_CARD_USERCMD, psstRetRemoveBattleCardUserCmd);

            this.addParamHandle(stHeroCardCmd.RET_NOTIFY_UNFINISHED_GAME_CMD, psstRetNotifyUnfinishedGameUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_REFRESH_CARD_ALL_STATE_CMD, psstRetRefreshCardAllStateUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_CLEAR_CARD_ONE_STATE_CMD, psstRetClearCardOneStateUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_SET_CARD_ONE_STATE_CMD, psstRetSetCardOneStateUserCmd);

            this.addParamHandle(stHeroCardCmd.RET_HERO_INTO_BATTLE_SCENE_CMD, psstRetHeroIntoBattleSceneUserCmd);
            this.addParamHandle(stHeroCardCmd.RET_CARD_ATTACK_FAIL_USERCMD_PARA, psstRetCardAttackFailUserCmd);
            //m_id2HandleDic[stHeroCardCmd.RET_BATTLE_HISTORY_INFO_CMD] = psstRetBattleHistoryInfoUserCmd;
            this.addParamHandle(stHeroCardCmd.RET_BATTLE_GAME_RESULT_CMD, psstRetBattleGameResultUserCmd);
            this.addParamHandle(stHeroCardCmd.NOTIFY_BATTLE_CARD_PROPERTY_CMD, psstNotifyBattleCardPropertyUserCmd);

            this.addParamHandle(stHeroCardCmd.NOTIFY_BATTLE_FLOW_START_CMD, psstNotifyBattleFlowStartUserCmd);
            this.addParamHandle(stHeroCardCmd.NOTIFY_BATTLE_FLOW_END_CMD, psstNotifyBattleFlowEndUserCmd);
            this.addParamHandle(stHeroCardCmd.NOTIFY_RESET_ATTACKTIMES_CMD, psstNotifyResetAttackTimesUserCmd);
            this.addParamHandle(stHeroCardCmd.NOTIFY_OUT_CARD_INFO_CMD, psstNotifyOutCardInfoUserCmd);
        }

        // 重载方便调试
        public override void handleMsg(ByteBuffer bu, byte byCmd, byte byParam)
        {
            base.handleMsg(bu, byCmd, byParam);
        }

        // 卡牌图鉴中显示的所有数据
        protected void psstNotifyAllCardTujianInfoCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stNotifyAllCardTujianInfoCmd cmd = new stNotifyAllCardTujianInfoCmd();
            cmd.derialize(msg);

            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstNotifyAllCardTujianInfoCmd(cmd.info);
            // 更新界面
            IUITuJian uiSC = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
            if (uiSC != null && uiSC.isVisible())
            {
                uiSC.psstNotifyAllCardTujianInfoCmd();
            }
        }

        // 一个卡牌图鉴信息
        protected void psstNotifyOneCardTujianInfoCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stNotifyOneCardTujianInfoCmd cmd = new stNotifyOneCardTujianInfoCmd();
            cmd.derialize(msg);

            bool bhas = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardDic.ContainsKey(cmd.id);
            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstNotifyOneCardTujianInfoCmd(cmd.id, cmd.num);
            // 更新界面
            IUITuJian uiSC = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
            if (uiSC != null && uiSC.isVisible())
            {
                uiSC.psstNotifyOneCardTujianInfoCmd(cmd.id, cmd.num, !bhas);
            }
        }

        // 返回开包显示的 5 张牌
        protected void psstRetGiftBagCardsDataUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stRetGiftBagCardsDataUserCmd cmd = new stRetGiftBagCardsDataUserCmd();
            cmd.derialize(msg);

            IUIOpenPack uiPack = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIOpenPack) as IUIOpenPack;
            if(uiPack != null)
            {
                uiPack.psstRetGiftBagCardsDataUserCmd(cmd.id);
            }
        }

        protected void psstRetCardGroupListInfoUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stRetCardGroupListInfoUserCmd cmd = new stRetCardGroupListInfoUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_logSys.log(string.Format("对战模式界面收到卡组列表信息，数量 {0}", cmd.info.Count));

            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstRetCardGroupListInfoUserCmd(cmd.info);
            // 更新界面
            IUITuJian uiSC = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
            if(uiSC != null && uiSC.isVisible())
            {
                uiSC.psstRetCardGroupListInfoUserCmd();
            }

            IUIJobSelect uiMS = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIJobSelect) as IUIJobSelect;
            if (uiMS != null && uiMS.isVisible())
            {
                uiMS.updateHeroList();
            }
        }

        protected void psstRetOneCardGroupInfoUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stRetOneCardGroupInfoUserCmd cmd = new stRetOneCardGroupInfoUserCmd();
            cmd.derialize(msg);
            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstRetOneCardGroupInfoUserCmd(cmd);
            // 更新界面
            IUITuJian uiSC = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
            if(uiSC != null && uiSC.isVisible())
            {
                uiSC.psstRetOneCardGroupInfoUserCmd(cmd.index, cmd.id);
            }
        }

        // 创建一个套牌
        protected void psstRetCreateOneCardGroupUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stRetCreateOneCardGroupUserCmd cmd = new stRetCreateOneCardGroupUserCmd();
            cmd.derialize(msg);

            // 更新数据
            Ctx.m_instance.m_dataPlayer.m_dataCard.psstRetCreateOneCardGroupUserCmd(cmd);
            // 更新界面
            IUITuJian uiSC = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
            if (uiSC != null && uiSC.isVisible())
            {
                uiSC.psstRetCreateOneCardGroupUserCmd(Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardGroupDic[cmd.index]);
            }
        }

        protected void psstRetDeleteOneCardGroupUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stRetDeleteOneCardGroupUserCmd cmd = new stRetDeleteOneCardGroupUserCmd();
            cmd.derialize(msg);

            if(cmd.success > 0)
            {
                // 更新数据
                int curIdx = Ctx.m_instance.m_dataPlayer.m_dataCard.psstRetDeleteOneCardGroupUserCmd(cmd.index);
                // 更新界面
                IUITuJian uiSC = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
                if (uiSC != null && uiSC.isVisible())
                {
                    uiSC.psstRetDeleteOneCardGroupUserCmd(curIdx);
                }
            }
        }

        protected void psstRetSaveOneCardGroupUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stRetSaveOneCardGroupUserCmd cmd = new stRetSaveOneCardGroupUserCmd();
            cmd.derialize(msg);

            if(cmd.success > 0)
            {
                IUITuJian uiSC = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUITuJian) as IUITuJian;
                if (uiSC != null && uiSC.isVisible())
                {
                    uiSC.psstRetSaveOneCardGroupUserCmd(cmd.index);
                }
            }
        }

        protected void psstRetAllHeroInfoUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stRetAllHeroInfoUserCmd cmd = new stRetAllHeroInfoUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataHero.psstRetAllHeroInfoUserCmd(cmd.info);

            IUIHero uiSH = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIHero) as IUIHero;
            if (uiSH != null && uiSH.isVisible())
            {
                uiSH.updateAllHero();
            }
        }

        // 返回一个 hero 信息
        protected void psstRetOneHeroInfoUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stRetOneHeroInfoUserCmd cmd = new stRetOneHeroInfoUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dataHero.psstRetOneHeroInfoUserCmd(cmd.info);
        }

        // 返回匹配结果
        protected void psstRetHeroFightMatchUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stRetHeroFightMatchUserCmd cmd = new stRetHeroFightMatchUserCmd();
            cmd.derialize(msg);

            // 显示匹配结果
            IUIJobSelect uiMS = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIJobSelect) as IUIJobSelect;
            if (uiMS != null && uiMS.isVisible())
            {
                uiMS.psstRetHeroFightMatchUserCmd(cmd);
            }
        }

        // 返回进入战斗场景消息
        protected void psstRetHeroIntoBattleSceneUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stRetHeroIntoBattleSceneUserCmd cmd = new stRetHeroIntoBattleSceneUserCmd();
            cmd.derialize(msg);
            msg.position = 0;

            IUIJobSelect ui = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIJobSelect) as IUIJobSelect;
            if (ui != null)
            {
                ui.psstRetHeroIntoBattleSceneUserCmd(msg);
            }
        }

        // 回归剩余卡牌数量
        protected void psstRetLeftCardLibNumUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stRetLeftCardLibNumUserCmd cmd = new stRetLeftCardLibNumUserCmd();
            cmd.derialize(msg);

            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_leftCardNum = cmd.selfNum;
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerEnemy].m_leftCardNum = cmd.otherNum;

            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiDZ != null && uiDZ.isVisible())
            {
                uiDZ.psstRetLeftCardLibNumUserCmd(cmd);
            }
        }

        // 返回 magic 点的数量
        protected void psstRetMagicPointInfoUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stRetMagicPointInfoUserCmd cmd = new stRetMagicPointInfoUserCmd();
            cmd.derialize(bu);

            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroMagicPoint = cmd.self;
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerEnemy].m_heroMagicPoint = cmd.other;

            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiDZ != null && uiDZ.isVisible())
            {
                uiDZ.psstRetMagicPointInfoUserCmd(cmd);
            }
        }

        // 刷新战斗状态
        protected void psstRetRefreshBattleStateUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stRetRefreshBattleStateUserCmd cmd = new stRetRefreshBattleStateUserCmd();
            cmd.derialize(bu);

            Ctx.m_instance.m_dataPlayer.m_dzData.m_state = cmd.state;

            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiDZ != null && uiDZ.isVisible())
            {
                uiDZ.psstRetRefreshBattleStateUserCmd(cmd);
            }
        }

        // 返回当前谁出牌
        protected void psstRetRefreshBattlePrivilegeUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            // 增加当前出牌次数
            ++Ctx.m_instance.m_dataPlayer.m_dzData.curPlayCardCount;

            stRetRefreshBattlePrivilegeUserCmd cmd = new stRetRefreshBattlePrivilegeUserCmd();
            cmd.derialize(bu);

            Ctx.m_instance.m_dataPlayer.m_dzData.m_priv = cmd.priv;

            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiDZ != null && uiDZ.isVisible())
            {
                uiDZ.psstRetRefreshBattlePrivilegeUserCmd(cmd);
            }
        }

        // 添加一个卡牌
        protected void psstAddBattleCardPropertyUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stAddBattleCardPropertyUserCmd cmd = new stAddBattleCardPropertyUserCmd();
            cmd.derialize(bu);

            Ctx.m_instance.m_logSys.log(string.Format("添加一个卡牌 thisid: {0}", cmd.mobject.qwThisID));

            if (cmd.byActionType == 1)
            {
                // 填充数据
                cmd.sceneItem = Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[cmd.who - 1].createCardItemBySvrData((EnDZPlayer)(cmd.who - 1), cmd.mobject);
                Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[cmd.who - 1].addOneSceneCard(cmd.sceneItem);       // 添加数据
            }
            else
            {
                cmd.sceneItem = Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[cmd.who - 1].updateCardInfoByCardItem(cmd.mobject);
            }

            if (cmd.sceneItem != null)      // 更新或者添加都需要这个数据必须存在
            {
                UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
                if (uiDZ != null && uiDZ.isVisible())
                {
                    uiDZ.psstAddBattleCardPropertyUserCmd(cmd);
                }
            }
        }

        // 对方信息
        protected void psstNotifyFightEnemyInfoUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            Ctx.m_instance.m_logSys.log("开始客户端初始动画播放");

            stNotifyFightEnemyInfoUserCmd cmd = new stNotifyFightEnemyInfoUserCmd();
            cmd.derialize(bu);

            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerEnemy].m_heroName = cmd.name;
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerEnemy].m_heroOccupation = cmd.occupation;

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstNotifyFightEnemyInfoUserCmd(cmd);
            }

            IUIDZ uiDZ = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIDZ) as IUIDZ;
            if (uiDZ != null)
            {
                uiDZ.psstNotifyFightEnemyInfoUserCmd();
            }
        }

        // 第一次几张牌
        protected void psstRetFirstHandCardUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            Ctx.m_instance.m_logSys.log("收到初始卡片列白");

            stRetFirstHandCardUserCmd cmd = new stRetFirstHandCardUserCmd();
            cmd.derialize(bu);

            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList = cmd.id;

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetFirstHandCardUserCmd(cmd);
            }
        }

        // 卡牌移动
        protected void psstRetMoveGameCardUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stRetMoveGameCardUserCmd cmd = new stRetMoveGameCardUserCmd();
            cmd.derialize(bu);

            if (cmd.success == 1)
            {
                // 更新数据
                cmd.side = Ctx.m_instance.m_dataPlayer.m_dzData.updateCardInfo(cmd);
            }

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetMoveGameCardUserCmd(cmd);
            }
        }

        // 通知手里卡牌已经满
        protected void psstRetNotifyHandIsFullUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stRetNotifyHandIsFullUserCmd cmd = new stRetNotifyHandIsFullUserCmd();
            cmd.derialize(bu);

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetNotifyHandIsFullUserCmd(cmd);
            }
        }

        // enemy 增加一个卡牌
        protected void psstAddEnemyHandCardPropertyUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstAddEnemyHandCardPropertyUserCmd();
            }
        }

        // 返回对方手里的卡牌数量，对方卡牌只有数量
        protected void psstRetEnemyHandCardNumUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stRetEnemyHandCardNumUserCmd cmd = new stRetEnemyHandCardNumUserCmd();
            cmd.derialize(bu);

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetEnemyHandCardNumUserCmd(cmd);
            }
        }

        // 删除对方手里一张卡牌
        protected void psstDelEnemyHandCardPropertyUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stDelEnemyHandCardPropertyUserCmd cmd = new stDelEnemyHandCardPropertyUserCmd();
            cmd.derialize(bu);

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstDelEnemyHandCardPropertyUserCmd(cmd);
            }
        }

        // 从已经出牌区域删除一个卡牌，这个目前就是战斗删除
        protected void psstRetRemoveBattleCardUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stRetRemoveBattleCardUserCmd cmd = new stRetRemoveBattleCardUserCmd();
            cmd.derialize(bu);

            Ctx.m_instance.m_logSys.log(string.Format("删除一个卡牌 thisid: {0}", cmd.dwThisID));

            int side = 0;
            SceneCardItem sceneItem = null;
            if (!Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[0].removeOneSceneCardByThisID(cmd.dwThisID, ref sceneItem))
            {
                side = 1;
                Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[1].removeOneSceneCardByThisID(cmd.dwThisID, ref sceneItem);
            }
            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                if (sceneItem != null)
                {
                    uiSceneDZ.psstRetRemoveBattleCardUserCmd(cmd, side, sceneItem);
                }
            }
        }

        // 通知客户端上一场战斗还没有结束
        protected void psstRetNotifyUnfinishedGameUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            Ctx.m_instance.m_dataPlayer.m_dzData.m_bLastEnd = false;
            // 可能要请求一些数据
            stReqEnterUnfinishedGameUserCmd cmd = new stReqEnterUnfinishedGameUserCmd();
            UtilMsg.sendMsg(cmd);
            Ctx.m_instance.m_dataPlayer.m_dzData.m_bLastEnd = true;
        }

        // 某一局开始后，刷新这一局的所有的状态
        protected void psstRetRefreshCardAllStateUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stRetRefreshCardAllStateUserCmd cmd = new stRetRefreshCardAllStateUserCmd();
            cmd.derialize(bu);

            SceneCardItem cardItem = Ctx.m_instance.m_dataPlayer.m_dzData.getCardItemByThisIDAndSide(cmd.dwThisID, (byte)(cmd.who - 1));
            if (cardItem != null)
            {
                cardItem.svrCard.state = cmd.state;
            }
        }

        // 清除某一个状态
        protected void psstRetClearCardOneStateUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stRetClearCardOneStateUserCmd cmd = new stRetClearCardOneStateUserCmd();
            cmd.derialize(bu);

            SceneCardItem cardItem = Ctx.m_instance.m_dataPlayer.m_dzData.getCardItemByThisIDAndSide(cmd.dwThisID, cmd.who);
            UtilMath.clearState((StateID)cmd.stateNum, cardItem.svrCard.state);
        }

        // 设置某一个状态
        protected void psstRetSetCardOneStateUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stRetSetCardOneStateUserCmd cmd = new stRetSetCardOneStateUserCmd();
            cmd.derialize(bu);

            SceneCardItem cardItem = Ctx.m_instance.m_dataPlayer.m_dzData.getCardItemByThisIDAndSide(cmd.dwThisID, (byte)(cmd.who - 1));
            UtilMath.setState((StateID)cmd.stateNum, cardItem.svrCard.state);
        }

        // 法术攻击失败
        protected void psstRetCardAttackFailUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stRetCardAttackFailUserCmd cmd = new stRetCardAttackFailUserCmd();
            cmd.derialize(bu);
            // 将不能使用的法术牌退回去
            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetCardAttackFailUserCmd(cmd);
            }
        }

        protected void psstRetBattleHistoryInfoUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stRetBattleHistoryInfoUserCmd cmd = new stRetBattleHistoryInfoUserCmd();
            cmd.derialize(bu);

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetBattleHistoryInfoUserCmd(cmd);
            }
        }

        protected void psstRetBattleGameResultUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stRetBattleGameResultUserCmd cmd = new stRetBattleGameResultUserCmd();
            cmd.derialize(bu);

            //if (cmd.win == 1 || cmd.win == 0)        // 赢了输了
            //{
            //    Ctx.m_instance.m_gameSys.loadGameScene();        // 加载游戏场景
            //}

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstRetBattleGameResultUserCmd(cmd);
            }
        }

        // 攻击返回
        protected void psstNotifyBattleCardPropertyUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stNotifyBattleCardPropertyUserCmd cmd = new stNotifyBattleCardPropertyUserCmd();
            cmd.derialize(bu);

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstNotifyBattleCardPropertyUserCmd(cmd);
            }
        }

        protected void psstNotifyBattleFlowStartUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstNotifyBattleFlowStartUserCmd(bu);
            }
        }

        protected void psstNotifyBattleFlowEndUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstNotifyBattleFlowEndUserCmd(bu);
            }
        }

        // 清除自己的卡牌(除了手牌)的攻击次数
        protected void psstNotifyResetAttackTimesUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstNotifyResetAttackTimesUserCmd();
            }
        }

        protected void psstNotifyOutCardInfoUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer bu = dispObj as ByteBuffer;

            stNotifyOutCardInfoUserCmd cmd = new stNotifyOutCardInfoUserCmd();
            cmd.derialize(bu);

            UISceneDZ uiSceneDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiSceneDZ != null && uiSceneDZ.isVisible())
            {
                uiSceneDZ.psstNotifyOutCardInfoUserCmd(cmd);
            }
        }
    }
}