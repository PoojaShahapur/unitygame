using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    public class ClickControl : ControlBase
    {
        public ClickControl(SceneCardBase rhv) : 
            base(rhv)
        {
            
        }

        override public void init()
        {
            this.m_card.clkDisp.addEventHandle(onCardClick);
        }

        // 所有的卡牌都可以点击，包括主角、装备、技能、手里卡牌、出的卡牌
        public void onCardClick(IDispatchObject dispObj)
        {
            if (m_card.m_sceneDZData.m_gameRunState.isInState(GameRunState.INITCARD))      // 如果处于初始化卡牌阶段
            {
                string resPath = "";
                // 这个时候还没有服务器的数据 m_sceneCardItem
                int idx = 0;
                idx = m_card.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.findCardIdx(m_card);
                // 显示换牌标志
                if (m_card.m_sceneDZData.m_changeCardList.IndexOf(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[idx]) != -1)      // 如果已经选中
                {
                    m_card.m_sceneDZData.m_changeCardList.Remove(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[idx]);
                    // 去掉叉号
                    UtilApi.Destroy(m_card.chaHaoGo);        // 释放资源
                }
                else  // 选中
                {
                    m_card.m_sceneDZData.m_changeCardList.Add(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[idx]);
                    // 添加叉号
                    resPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "ChaHao.prefab");
                    ModelRes model = Ctx.m_instance.m_modelMgr.getAndSyncLoad<ModelRes>(resPath) as ModelRes;
                    m_card.chaHaoGo = model.InstantiateObject(resPath) as GameObject;
                    UtilApi.SetParent(m_card.chaHaoGo.transform, m_card.gameObject.transform, false);
                }
            }
            else        // 如果在对战阶段
            {
                if (m_card.sceneCardItem != null)
                {
                    if (m_card.m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpNormalAttack))
                    {
                        if (m_card.m_sceneDZData.m_gameOpState.canAttackOp(m_card, EnGameOp.eOpNormalAttack))
                        {
                            // 发送攻击指令
                            stCardAttackMagicUserCmd cmd = new stCardAttackMagicUserCmd();
                            cmd.dwAttThisID = m_card.m_sceneDZData.m_gameOpState.getOpCardID();
                            cmd.dwDefThisID = m_card.sceneCardItem.svrCard.qwThisID;
                            UtilMsg.sendMsg(cmd);

                            //m_sceneDZData.m_gameOpState.quitAttackOp(false);
                        }
                        else
                        {
                            m_card.enterAttack();
                        }
                    }
                    else if ((m_card.m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpFaShu)))        // 法术攻击
                    {
                        if (m_card.m_sceneDZData.m_gameOpState.canAttackOp(m_card, EnGameOp.eOpFaShu))
                        {
                            // 必然是有目标的法术攻击
                            // 发送法术攻击消息
                            stCardMoveAndAttackMagicUserCmd cmd = new stCardMoveAndAttackMagicUserCmd();
                            cmd.dwAttThisID = m_card.m_sceneDZData.m_gameOpState.getOpCardID();
                            cmd.dwMagicType = (uint)m_card.m_sceneDZData.m_gameOpState.getOpCardFaShu();
                            cmd.dwDefThisID = m_card.sceneCardItem.svrCard.qwThisID;
                            //m_sceneDZData.m_gameOpState.quitAttackOp(false);
                            UtilMsg.sendMsg(cmd);
                        }
                        else
                        {
                            m_card.enterAttack();
                        }
                    }
                    else if (m_card.m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpZhanHouAttack))      // 战吼攻击
                    {
                        if (m_card.m_sceneDZData.m_gameOpState.canAttackOp(m_card, EnGameOp.eOpZhanHouAttack))
                        {
                            // 发送攻击指令
                            stCardMoveAndAttackMagicUserCmd cmd = new stCardMoveAndAttackMagicUserCmd();
                            cmd.dwAttThisID = m_card.m_sceneDZData.m_gameOpState.getOpCardID();
                            cmd.dwMagicType = (uint)m_card.m_sceneDZData.m_gameOpState.getOpCardFaShu();
                            cmd.dwDefThisID = m_card.sceneCardItem.svrCard.qwThisID;
                            cmd.dst = new stObjectLocation();
                            cmd.dst.dwLocation = (uint)m_card.sceneCardItem.cardArea;
                            cmd.dst.y = m_card.curIndex;
                            UtilMsg.sendMsg(cmd);

                            //m_sceneDZData.m_gameOpState.quitAttackOp();
                        }
                        else
                        {
                            m_card.enterAttack();
                        }
                    }
                    else if (CardArea.CARDCELLTYPE_SKILL == m_card.sceneCardItem.cardArea)     // 如果是技能卡牌
                    {
                        stCardMoveAndAttackMagicUserCmd skillCmd = new stCardMoveAndAttackMagicUserCmd();
                        skillCmd.dwAttThisID = m_card.sceneCardItem.svrCard.qwThisID;
                        UtilMsg.sendMsg(skillCmd);
                    }
                    else        // 默认点击处理都走这里
                    {
                        m_card.enterAttack();
                    }
                }
            }
        }
    }
}
