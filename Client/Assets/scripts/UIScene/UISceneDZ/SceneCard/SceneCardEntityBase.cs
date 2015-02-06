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
                    if (m_sceneCardItem.m_playerFlag == EnDZPlayer.ePlayerEnemy)        // 如果是 enemy 的卡牌
                    {
                        disableDrag();
                    }
                    // 如果是放在技能的位置，是不允许拖放的
                    else if (m_sceneCardItem.m_cardArea == CardArea.CARDCELLTYPE_SKILL)
                    {
                        disableDrag();
                    }

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
                    text = UtilApi.getComByP<Text>(gameObject, "attack/Canvas/Text");       // 攻击
                    text.text = m_sceneCardItem.m_svrCard.damage.ToString();
                    text = UtilApi.getComByP<Text>(gameObject, "cost/Canvas/Text");         // Magic
                    text.text = m_sceneCardItem.m_svrCard.mpcost.ToString();
                    text = UtilApi.getComByP<Text>(gameObject, "health/Canvas/Text");       // HP
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
                if ((m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpFaShu)))        // 法术攻击
                {
                    if (m_sceneDZData.m_gameOpState.canAttackOp(this, EnGameOp.eOpFaShu))
                    {
                        // 必然是有目标的法术攻击
                        // 发送法术攻击消息
                        stCardAttackMagicUserCmd cmd = new stCardAttackMagicUserCmd();
                        cmd.dwAttThisID = m_sceneDZData.m_gameOpState.getOpCardID();
                        cmd.dwMagicType = (uint)m_sceneCardItem.m_cardTableItem.m_faShu;
                        cmd.dwDefThisID = this.sceneCardItem.m_svrCard.qwThisID;
                        UtilMsg.sendMsg(cmd);
                    }
                    else if (m_sceneCardItem.m_cardArea == CardArea.CARDCELLTYPE_COMMON)        // 如果点击自己出过的牌，再次进入普通攻击
                    {
                        m_sceneDZData.m_gameOpState.enterAttackOp(EnGameOp.eOpAttack, this);
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
}