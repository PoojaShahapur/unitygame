using SDK.Common;
using SDK.Lib;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SceneDZData
    {
        public dzturn m_dzturn = new dzturn();          // 翻转按钮，结束当前一局
        public luckycoin m_luckycoin = new luckycoin(); // 对战场景中的幸运币
        public SelfTurnTip m_selfTurnTip = new SelfTurnTip();               // 自己回合提示
        public SelfCardFullTip m_selfCardFullTip = new SelfCardFullTip();   // 自己卡牌满

        public Text[] m_textArr = new Text[(int)EnSceneDZText.eTotal];
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

        public IAttackArrow m_attackArrow;
        public IGameOpState m_gameOpState;

        public int m_curWhiteIdx = -1;

        public SceneDragCard createOneCard(uint objid, EnDZPlayer m_playerFlag, CardArea area)
        {
            SceneDragCard cardItem = new SceneDragCard(this);
            if (uint.MaxValue == objid)
            {
                cardItem.setGameObject(UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getEnemyCardModel().getObject()) as GameObject);
            }
            else
            {
                cardItem.setGameObject(UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getSceneCardModel(EnSceneCardType.eScene_minion).getObject()) as GameObject);
            }
            cardItem.m_centerPos = m_cardCenterGOArr[(int)m_playerFlag, (int)area].transform.localPosition;
            cardItem.getGameObject().transform.parent = m_centerGO.transform;
            //cardItem.sceneCardItem.m_cardArea = area;
            //cardItem.sceneCardItem.m_playerFlag = m_playerFlag;
            // 设置出事位置为发牌位置
            cardItem.startPos = m_cardCenterGOArr[(int)m_playerFlag, (int)CardArea.CARDCELLTYPE_NONE].transform.localPosition;
            cardItem.destPos = m_cardCenterGOArr[(int)m_playerFlag, (int)area].transform.localPosition;

            return cardItem;
        }

        public void createMovePath(SceneDragCard card, Transform startPos, Transform destPos)
        {
            card.startPos = startPos.localPosition;
            card.destPos = destPos.localPosition;

            card.moveToDest();
        }
    }
}