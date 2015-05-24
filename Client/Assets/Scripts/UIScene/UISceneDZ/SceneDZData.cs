using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SceneDZData
    {
        public RoundBtn m_roundBtn = new RoundBtn();          // 翻转按钮，结束当前一局
        public LuckCoin m_luckCoin = new LuckCoin(); // 对战场景中的幸运币
        public SelfTurnTip m_selfTurnTip = new SelfTurnTip();               // 自己回合提示
        public SelfCardFullTip m_selfCardFullTip = new SelfCardFullTip();   // 自己卡牌满

        public AuxLabel[] m_textArr = new AuxLabel[(int)EnSceneDZText.eTotal];
        public UIGrid[] m_mpGridArr = new UIGrid[(int)EnSceneDZText.eTotal];

        public GameObject m_centerGO;                   // 中心 GO ，所有场景中的牌都放在这个上面
        public GameObject m_startGO;                    // 开始按钮

        public SceneDragCard m_curDragItem;             // 当前正在拖放的 item

        //public GameObject[] m_handCardCenterGOArr;              // 手里卡牌中心
        //public GameObject[] m_startCardCenterGOArr;             // 发牌中心
        //public GameObject[] m_outCardCenterGOArr;               // 已经出的牌区域中心

        public GameObject[,] m_cardCenterGOArr = new GameObject[2, 6];    // 保存所有占位的位置信息

        public GameObject m_attackArrowGO;                      // 攻击箭头开始位置
        public GameObject m_arrowListGO;                        // 攻击箭头列表

        public AttackArrow m_attackArrow;
        public GameOpState m_gameOpState;
        public GameRunState m_gameRunState;             // 游戏运行状态

        public SceneDZArea[] m_sceneDZAreaArr;

        protected int m_preWhiteIdx = -1;      // 之前白色卡牌位置
        protected int m_curWhiteIdx = -1;      // 当前卡牌位置

        protected bool m_bHeroAniEnd = false;   // hero 动画是否结束
        protected bool m_bAddselfCard = false;  // 是否有自己的初始卡牌

        public GameObject m_timerGo;            // 定时器节点
        public DJSTimer m_DJSTimer;             // 定时器
        public List<uint> m_changeCardList = new List<uint>();     // 在初始阶段，选中的需要交换卡牌
        public DZDaoJiShiXmlLimit m_DZDaoJiShiXmlLimit;

        public bool bHeroAniEnd
        {
            get
            {
                return m_bHeroAniEnd;
            }
            set
            {
                m_bHeroAniEnd = value;
            }
        }

        public bool bAddselfCard
        {
            get
            {
                return m_bAddselfCard;
            }
            set
            {
                m_bAddselfCard = value;
            }
        }

        public int preWhiteIdx
        {
            get
            {
                return m_preWhiteIdx;
            }
            set
            {
                m_preWhiteIdx = value;
            }
        }

        public int curWhiteIdx
        {
            get
            {
                return m_curWhiteIdx;
            }
            set
            {
                m_preWhiteIdx = m_curWhiteIdx;
                m_curWhiteIdx = value;
            }
        }

        public SceneDragCard createOneCard(uint objid, EnDZPlayer m_playerFlag, CardArea area)
        {
            SceneDragCard cardItem = new SceneDragCard(this);
            if (uint.MaxValue == objid)
            {
                cardItem.setGameObject(UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getEnemyCardModel().getObject()) as GameObject);
            }
            else
            {
                TableCardItemBody tableBody = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, objid).m_itemBody as TableCardItemBody;
                GameObject tmpGO = Ctx.m_instance.m_modelMgr.getSceneCardModel((CardType)tableBody.m_type).getObject();
                if (tmpGO == null)
                {
                    tmpGO = Ctx.m_instance.m_modelMgr.getSceneCardModel(CardType.CARDTYPE_MAGIC).getObject();
                }
                cardItem.setGameObject(UtilApi.Instantiate(tmpGO) as GameObject);
            }

            cardItem.m_centerPos = m_cardCenterGOArr[(int)m_playerFlag, (int)area].transform.localPosition;
            //cardItem.getGameObject().transform.parent = m_centerGO.transform;
            cardItem.getGameObject().transform.SetParent(m_centerGO.transform);
            // 设置出事位置为发牌位置
            cardItem.startPos = m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_NONE].transform.localPosition;
            cardItem.destPos = m_cardCenterGOArr[(int)m_playerFlag, (int)area].transform.localPosition;

            // 设置是否可以动画
            if (m_playerFlag == EnDZPlayer.ePlayerEnemy)        // 如果是 enemy 的卡牌
            {
                cardItem.disableDrag();
                if(area == CardArea.CARDCELLTYPE_SKILL || area == CardArea.CARDCELLTYPE_EQUIP)
                {
                    cardItem.destScale = SceneCardEntityBase.SMALLFACT;
                }
            }
            // 如果是放在技能或者装备的位置，是不允许拖放的
            else if (area == CardArea.CARDCELLTYPE_SKILL || area == CardArea.CARDCELLTYPE_EQUIP)
            {
                cardItem.destScale = SceneCardEntityBase.SMALLFACT;
                cardItem.disableDrag();
            }

            // 更新边框
            if (EnDZPlayer.ePlayerSelf == m_playerFlag)
            {
                if(CardArea.CARDCELLTYPE_HAND == area)
                {
                    if(Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
                    {
                        cardItem.updateCardOutState(true);
                    }
                    else
                    {
                        cardItem.updateCardOutState(false);
                    }
                }
            }

            return cardItem;
        }

        public void createMovePath(SceneDragCard card, Transform startPos, Transform destPos)
        {
            card.startPos = startPos.localPosition;
            card.destPos = destPos.localPosition;

            card.moveToDestRST();
        }

        public SceneCardEntityBase getUnderSceneCard()
        {
            SceneCardEntityBase cardBase;
            GameObject underGo = Ctx.m_instance.m_coordConv.getUnderGameObject();
            if(underGo != null)
            {
                cardBase = m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].getUnderSceneCard(underGo);
                if(cardBase != null)
                {
                    return cardBase;
                }

                cardBase = m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].getUnderSceneCard(underGo);
                if (cardBase != null)
                {
                    return cardBase;
                }
            }

            return null;
        }

        public SceneCardEntityBase getSceneCardByThisID(uint thisID, ref EnDZPlayer side, ref CardArea slot)
        {
            SceneCardEntityBase cardBase;
            cardBase = m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].getSceneCardByThisID(thisID, ref slot);
            if(cardBase != null)
            {
                side = EnDZPlayer.ePlayerSelf;
                return cardBase;
            }

            cardBase = m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].getSceneCardByThisID(thisID, ref slot);
            if (cardBase != null)
            {
                side = EnDZPlayer.ePlayerEnemy;
                return cardBase;
            }

            return null;
        }
    }
}