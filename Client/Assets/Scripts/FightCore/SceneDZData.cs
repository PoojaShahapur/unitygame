using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace FightCore
{
    public class SceneDZData
    {
        public SceneDZArea[] m_sceneDZAreaArr;
        public HistoryArea m_historyArea;
        public RoundMgr m_roundMgr;
        public PlaceHolderGo m_placeHolderGo;

        public DragDropData m_dragDropData;
        public CardNpcMgr m_cardNpcMgr;
        public AttackArrow m_attackArrow;
        public GameOpState m_gameOpState;
        public GameRunState m_gameRunState;             // 游戏运行状态

        protected int m_preWhiteIdx = -1;      // 之前白色卡牌位置
        protected int m_curWhiteIdx = -1;      // 当前卡牌位置

        protected bool m_bHeroAniEnd = false;   // hero 动画是否结束
        protected bool m_bAddselfCard = false;  // 是否有自己的初始卡牌
        public List<int> m_changeCardIdxList;     // 在初始阶段，选中的需要交换卡牌的索引，从左到右分别是 0 ， 1 因为可能初始卡牌有相同的 CardId
        public DZDaoJiShiXmlLimit m_DZDaoJiShiXmlLimit;

        public FightMsgMgr m_fightMsgMgr;
        public WatchCardInfo m_watchCardInfo;
        public WatchOutCardInfo m_watchOutCardInfo;

        public SceneDZData()
        {
            m_placeHolderGo = new PlaceHolderGo();
            // 加载xml配置文件
            m_DZDaoJiShiXmlLimit = Ctx.m_instance.m_mapCfg.m_mapXml.m_list[0] as DZDaoJiShiXmlLimit;

            m_gameRunState = new GameRunState(this);
            m_sceneDZAreaArr = new SceneDZArea[(int)EnDZPlayer.ePlayerTotal];
            m_cardNpcMgr = new CardNpcMgr(this);
            m_changeCardIdxList = new List<int>();
            m_fightMsgMgr = new FightMsgMgr(this);

            m_bHeroAniEnd = true;
            m_watchCardInfo = new WatchCardInfo();
            m_watchOutCardInfo = new WatchOutCardInfo(this);
            m_roundMgr = new RoundMgr(this);
            m_roundMgr.startInitCardTimer();           // 启动定时器
            m_dragDropData = new DragDropData();
        }

        public void findWidget()
        {
            m_placeHolderGo.findWidget();
            m_cardNpcMgr.findWidget();
        }

        public void addEventHandle()
        {

        }

        public void init()
        {
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf] = new SelfDZArea(this, EnDZPlayer.ePlayerSelf);
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy] = new EnemyDZArea(this, EnDZPlayer.ePlayerEnemy);

            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].init();
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].init();

            m_historyArea = new HistoryArea(UtilApi.GoFindChildByName(CVSceneDZPath.HistoryGo));
            m_historyArea.m_sceneDZData = this;
            m_attackArrow = new AttackArrow(this);
            m_gameOpState = new GameOpState(this);

            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].crystalPtPanel.findWidget();
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].crystalPtPanel.findWidget();
        }

        public void dispose()
        {
            // 释放自己的资源
            // 移除定时器
            m_roundMgr.dispose();
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].dispose();
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].dispose();

            m_cardNpcMgr.dispose();
        }

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

        public SceneCardBase getUnderSceneCard()
        {
            SceneCardBase cardBase;
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

        public SceneCardBase getSceneCardByThisID(uint thisID, ref EnDZPlayer side, ref CardArea slot)
        {
            SceneCardBase cardBase;
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

        public void heroAniEndDisp()
        {
            bHeroAniEnd = true;
            UtilApi.SetActive(m_placeHolderGo.m_startGO, true);      // 主角动画完成，需要显示开始按钮
            if (bAddselfCard)
            {
                addSelfFirstCard();
            }
        }

        public void addSelfFirstCard()
        {
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.addInitCard();
        }

        // 获取拖动时候卡牌的高度
        public float getDragCardHeight()
        {
            return m_placeHolderGo.m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_HAND].transform.localPosition.y + SceneDZCV.DRAG_YDELTA;
        }
    }
}