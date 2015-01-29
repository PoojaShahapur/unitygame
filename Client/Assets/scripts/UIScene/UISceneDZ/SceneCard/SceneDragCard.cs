using Game.Msg;
using SDK.Common;
using System;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 场景中可拖放的卡牌，仅限场景中可拖放的卡牌
     */
    public class SceneDragCard : SceneAniCard
    {
        public SceneDZData m_sceneDZData = new SceneDZData();
        // 拖动中分发
        public Action m_moveDisp;
        public Vector3 m_centerPos;         // 中心点位置
        public float m_radius = 1;              // 半径
        public bool m_isCalc;               // 本次是否计算过超过圆形区域的处理

        public SceneDragCard(SceneDZData sceneDZData)
        {
            m_sceneDZData = sceneDZData;
        }

        public override void Start()
        {
            base.Start();

            UIDragObject drag = gameObject.AddComponent<UIDragObject>();
            drag.target = gameObject.transform;
            drag.m_startDragDisp = onStartDrag;
            drag.m_moveDisp = onMove;
            drag.m_dropEndDisp = onDrogEnd;

            WindowDragTilt title = gameObject.AddComponent<WindowDragTilt>();

            UtilApi.addEventHandle(gameObject, onClk);
        }

        protected void onStartDrag()
        {
            m_sceneDZData.m_curDragItem = this;     // 设置当前拖放的目标

            // 开始拖动的时候需要记住原始的信息，如果拖放失败，好回来
            //destPos = gameObject.transform.localPosition;
            //destRot = gameObject.transform.localRotation.eulerAngles;
            //destScale = gameObject.transform.localScale;
        }

        protected void onMove()
        {
            // 这个地方需要判断是不是在允许的范围内
            if (!m_isCalc)
            {
                if (UtilMath.xzDis(m_centerPos, transform.localPosition) > m_radius)
                {
                    m_isCalc = true;

                    if (!Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())        // 不是自己出牌
                    {
                        UICamera.simuStopDrag();
                        m_sceneDZData.m_curDragItem = null;
                        UIDragObject drag = gameObject.AddComponent<UIDragObject>();
                        drag.reset();
                        this.moveToDest();      // 退回去
                        m_isCalc = false;
                    }
                }
            }

            // 如果不是自己出牌或者拖动的是装备卡牌，就不用通知其它的移动了
            if(Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
                if (m_sceneCardItem.m_cardTableItem.m_type != (int)CardType.CARDTYPE_EQUIP)        // 如果拖动的手里的牌不是装备牌
                {
                    if(m_moveDisp != null)
                    {
                        m_moveDisp();
                    }
                }
            }
        }

        // 拖放结束处理
        protected void onDrogEnd()
        {
            m_isCalc = false;
            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.m_svrCard != null)
                {
                    stMoveGameCardUserCmd cmd = new stMoveGameCardUserCmd();
                    cmd.dst = new stObjectLocation();
                    if (m_sceneCardItem.m_cardTableItem != null)
                    {
                        cmd.dst.dwTableID = 0;
                        cmd.dst.x = 0;
                        cmd.qwThisID = m_sceneCardItem.m_svrCard.qwThisID;

                        if (m_sceneCardItem.m_cardTableItem.m_type == (int)CardType.CARDTYPE_EQUIP)
                        {
                            cmd.dst.dwLocation = (int)CardArea.CARDCELLTYPE_EQUIP;
                            cmd.dst.y = 0;
                        }
                        else
                        {
                            cmd.dst.dwLocation = (int)CardArea.CARDCELLTYPE_COMMON;
                            cmd.dst.y = (byte)m_sceneDZData.m_curWhiteIdx;
                        }
                        UtilMsg.sendMsg(cmd);
                    }
                }
            }

            // test
            //moveToDest();
            //UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneDZ) as UISceneDZ;
            //uiDZ.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].moveCard();

            // test 测试攻击箭头
            //m_sceneDZData.m_attackArrow.startArrow();
        }

        public void onClk(GameObject go)
        {
            if (m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpAttack))
            {
                if (m_sceneDZData.m_gameOpState.canAttackOp(this))
                {
                    // 发送攻击指令
                    stCardAttackMagicUserCmd cmd = new stCardAttackMagicUserCmd();
                    cmd.dwAttThisID = m_sceneDZData.m_gameOpState.getOpCardID();
                    cmd.dwDefThisID = this.m_sceneCardItem.m_svrCard.qwThisID;
                    UtilMsg.sendMsg(cmd);

                    m_sceneDZData.m_gameOpState.quitAttackOp();
                }
                else
                {
                    if (this.m_sceneCardItem.m_cardArea == CardArea.CARDCELLTYPE_COMMON)
                    {
                        // 只有点击自己的时候，才启动攻击
                        if (m_sceneCardItem.m_playerFlag == EnDZPlayer.ePlayerSelf)
                        {
                            m_sceneDZData.m_gameOpState.enterAttackOp(EnGameOp.eOpAttack, this);
                        }
                    }
                }
            }
            else
            {
                if (this.m_sceneCardItem.m_cardArea == CardArea.CARDCELLTYPE_COMMON)
                {
                    // 只有点击自己的时候，才启动攻击
                    if (m_sceneCardItem.m_playerFlag == EnDZPlayer.ePlayerSelf)
                    {
                        m_sceneDZData.m_gameOpState.enterAttackOp(EnGameOp.eOpAttack, this);
                    }
                }
            }
        }
    }
}