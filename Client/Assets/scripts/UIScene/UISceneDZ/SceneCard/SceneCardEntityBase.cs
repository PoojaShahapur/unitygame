using Game.Msg;
using SDK.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 场景中卡牌基类
     */
    public class SceneCardEntityBase : LSBehaviour
    {
        public static Vector3 SMALLFACT = new Vector3(0.5f, 0.5f, 0.5f);    // 小牌时的缩放因子
        public static Vector3 BIGFACT = new Vector3(1.2f, 1.2f, 1.2f);      // 大牌时候的因子

        protected SceneCardItem m_sceneCardItem;
        public SceneDZData m_sceneDZData = new SceneDZData();
        public ushort m_index = 0;             // 在牌中的索引，主要是手里的牌和打出去的牌

        protected NumAniSequence m_numAniSeq = new NumAniSequence();       // 攻击动画序列，这个所有的都有

        public override void Start()
        {
            base.Start();

            UtilApi.addEventHandle(gameObject, onClk);
        }

        public SceneCardItem sceneCardItem
        {
            get
            {
                return m_sceneCardItem;
            }
            set
            {
                m_sceneCardItem = value;

                if (m_sceneCardItem != null)
                {
                    updateCardDataChange();
                    updateCardDataNoChange();
                }
            }
        }

        public void destroy()
        {
            UtilApi.Destroy(gameObject);
            m_sceneCardItem = null;
        }

        // 更新卡牌属性，这个主要更改卡牌经常改变的属性
        public virtual void updateCardDataChange()
        {
            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.m_cardArea == CardArea.CARDCELLTYPE_COMMON || m_sceneCardItem.m_cardArea == CardArea.CARDCELLTYPE_HAND)
                {
                    Text text;
                    text = UtilApi.getComByP<Text>(gameObject, "attack/Text");       // 攻击
                    text.text = m_sceneCardItem.m_svrCard.damage.ToString();
                    text = UtilApi.getComByP<Text>(gameObject, "cost/Text");         // Magic
                    text.text = m_sceneCardItem.m_svrCard.mpcost.ToString();
                    text = UtilApi.getComByP<Text>(gameObject, "health/Text");       // HP
                    text.text = m_sceneCardItem.m_svrCard.hp.ToString();
                }
            }
        }

        // 这个主要是更新卡牌不经常改变的属性
        public virtual void updateCardDataNoChange()
        {
            if (m_sceneCardItem != null)
            {
                if (m_sceneCardItem.m_cardArea != CardArea.CARDCELLTYPE_HERO)
                {
                    UtilApi.updateCardDataNoChange(m_sceneCardItem.m_cardTableItem, gameObject);
                }
            }
        }

        // 关闭拖放功能
        public virtual void disableDrag()
        {

        }

        // 开启拖动
        public virtual void  enableDrag()
        {

        }

        public void onClk(GameObject go)
        {
            if (this.m_sceneCardItem != null)
            {
                if (m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpAttack))
                {
                    if (m_sceneDZData.m_gameOpState.canAttackOp(this, EnGameOp.eOpAttack))
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
                else if ((m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpFaShu)))        // 法术攻击
                {
                    if (m_sceneDZData.m_gameOpState.canAttackOp(this, EnGameOp.eOpFaShu))
                    {
                        // 必然是有目标的法术攻击
                        // 发送法术攻击消息
                        stCardAttackMagicUserCmd cmd = new stCardAttackMagicUserCmd();
                        cmd.dwAttThisID = m_sceneDZData.m_gameOpState.getOpCardID();
                        cmd.dwMagicType = (uint)m_sceneDZData.m_gameOpState.getOpCardFaShu();
                        cmd.dwDefThisID = this.sceneCardItem.m_svrCard.qwThisID;
                        m_sceneDZData.m_gameOpState.quitAttackOp();
                        UtilMsg.sendMsg(cmd);
                    }
                    else if (m_sceneCardItem.m_cardArea == CardArea.CARDCELLTYPE_COMMON)        // 如果点击自己出过的牌，再次进入普通攻击
                    {
                        m_sceneDZData.m_gameOpState.enterAttackOp(EnGameOp.eOpAttack, this);
                    }
                }
                else if (m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpZhanHouAttack))      // 战吼攻击
                {
                    if (m_sceneDZData.m_gameOpState.canAttackOp(this, EnGameOp.eOpZhanHouAttack))
                    {
                        if (m_sceneDZData.m_gameOpState.canAttackOp(this, EnGameOp.eOpAttack))
                        {
                            // 发送攻击指令
                            stCardMoveAndAttackMagicUserCmd cmd = new stCardMoveAndAttackMagicUserCmd();
                            cmd.dwAttThisID = m_sceneDZData.m_gameOpState.getOpCardID();
                            cmd.dwMagicType = (uint)m_sceneDZData.m_gameOpState.getOpCardFaShu();
                            cmd.dwDefThisID = this.m_sceneCardItem.m_svrCard.qwThisID;
                            cmd.dst.dwLocation = (uint)this.m_sceneCardItem.m_cardArea;
                            cmd.dst.y = this.m_index;
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

        public void updateCardGreenFrame(bool benable)
        {
            GameObject go = UtilApi.TransFindChildByPObjAndPath(getGameObject(), "bailight");
            if (go != null)
            {
                if (benable)
                {
                    if (sceneCardItem != null)
                    {
                        if (sceneCardItem.m_svrCard.mpcost <= Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)sceneCardItem.m_playerFlag].m_heroMagicPoint.mp)
                        {
                            UtilApi.getComByP<MeshRenderer>(go).enabled = true;
                        }
                        else
                        {
                            UtilApi.getComByP<MeshRenderer>(go).enabled = false;
                        }
                    }
                }
                else
                {
                    UtilApi.getComByP<MeshRenderer>(go).enabled = false;
                }
            }
        }

        // 播放攻击动画，就是移动过去砸一下
        public void playAttackAni(Vector3 destPos)
        {
            Vector3 midPt;      // 中间点
            midPt = (gameObject.transform.localPosition + destPos) / 2;
            midPt.y = 2;

            SimpleCurveAni curveAni = new SimpleCurveAni();
            m_numAniSeq.addOneNumAni(curveAni);
            curveAni.setGO(gameObject);
            curveAni.setTime(0.3f);
            curveAni.setPlotCount(3);
            curveAni.addPlotPt(0, gameObject.transform.localPosition);
            curveAni.addPlotPt(1, midPt);
            curveAni.addPlotPt(2, destPos);

            curveAni.setEaseType(iTween.EaseType.easeInExpo);

            curveAni = new SimpleCurveAni();
            m_numAniSeq.addOneNumAni(curveAni);
            curveAni.setGO(gameObject);
            curveAni.setTime(0.3f);
            curveAni.setPlotCount(3);
            curveAni.addPlotPt(0, destPos);
            curveAni.addPlotPt(1, midPt);
            curveAni.addPlotPt(2, gameObject.transform.localPosition);

            curveAni.setEaseType(iTween.EaseType.easeInExpo);

            m_numAniSeq.play();
        }

        public void playFlyNum(int num)
        {
            Ctx.m_instance.m_pFlyNumMgr.addFlyNum(num, gameObject.transform.localPosition, m_sceneDZData.m_centerGO);
        }
    }
}