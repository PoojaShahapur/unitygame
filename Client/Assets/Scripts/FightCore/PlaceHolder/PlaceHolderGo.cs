using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    public class PlaceHolderGo
    {
        public GameObject m_centerGO;                   // 中心 GO ，所有场景中的牌都放在这个上面
        public GameObject m_startGO;                    // 开始按钮
        public GameObject[,] m_cardCenterGOArr;         // 保存所有占位的位置信息
        public GameObject[] m_cardHandRadiusGO;         // 手牌半径位置
        public GameObject[] m_cardCommonRadiusGO;       // 场牌半径位置
        public float[] m_cardHandAreaWidthArr;      // 卡牌手牌区域宽度
        public float[] m_cardCommonAreaWidthArr;    // 出牌区手牌区域宽度
        public GameObject m_timerGo;            // 定时器节点

        public GameObject m_attackArrowGO;                      // 攻击箭头开始位置
        public GameObject m_arrowListGO;                        // 攻击箭头列表

        public PlaceHolderGo()
        {
            m_cardCenterGOArr = new GameObject[2, 6];
        }

        public void findWidget()
        {
            m_centerGO = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.CenterGO);
            m_startGO = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.StartGO);
            //UtilApi.SetActive(m_startGO, false);      // 默认是隐藏的

            m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_NONE] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfStartCardCenterGO);
            m_cardCenterGOArr[(int)EnDZPlayer.ePlayerEnemy, (int)CardArea.CARDCELLTYPE_NONE] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyStartCardCenterGO);

            m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_COMMON] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfOutCardCenterGO);
            m_cardCenterGOArr[(int)EnDZPlayer.ePlayerEnemy, (int)CardArea.CARDCELLTYPE_COMMON] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyOutCardCenterGO);

            m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_HAND] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfCardCenterGO);
            m_cardCenterGOArr[(int)EnDZPlayer.ePlayerEnemy, (int)CardArea.CARDCELLTYPE_HAND] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyCardCenterGO);

            m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_EQUIP] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfEquipGO);
            m_cardCenterGOArr[(int)EnDZPlayer.ePlayerEnemy, (int)CardArea.CARDCELLTYPE_EQUIP] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyEquipGO);

            m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_SKILL] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfSkillGO);
            m_cardCenterGOArr[(int)EnDZPlayer.ePlayerEnemy, (int)CardArea.CARDCELLTYPE_SKILL] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemySkillGO);

            m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_HERO] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfHeroGO);
            m_cardCenterGOArr[(int)EnDZPlayer.ePlayerEnemy, (int)CardArea.CARDCELLTYPE_HERO] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyHeroGO);

            m_cardHandRadiusGO = new GameObject[2];
            m_cardHandRadiusGO[0] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfCardHandRadiusGO);
            m_cardHandRadiusGO[1] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyCardHandRadiusGO);

            m_cardHandAreaWidthArr = new float[2];
            m_cardHandAreaWidthArr[0] = m_cardHandRadiusGO[0].transform.localPosition.x - m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_HAND].transform.localPosition.x;
            m_cardHandAreaWidthArr[1] = m_cardHandRadiusGO[1].transform.localPosition.x - m_cardCenterGOArr[(int)EnDZPlayer.ePlayerEnemy, (int)CardArea.CARDCELLTYPE_HAND].transform.localPosition.x;

            m_cardCommonRadiusGO = new GameObject[2];
            m_cardCommonRadiusGO[0] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfCardCommonRadiusGO);
            m_cardCommonRadiusGO[1] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyCardCommonRadiusGO);

            m_cardCommonAreaWidthArr = new float[2];
            m_cardCommonAreaWidthArr[0] = m_cardCommonRadiusGO[0].transform.localPosition.x - m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_HAND].transform.localPosition.x;
            m_cardCommonAreaWidthArr[1] = m_cardCommonRadiusGO[1].transform.localPosition.x - m_cardCenterGOArr[(int)EnDZPlayer.ePlayerEnemy, (int)CardArea.CARDCELLTYPE_HAND].transform.localPosition.x;

            m_timerGo = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.TimerGo);

            m_attackArrowGO = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.ArrowStartPosGO);
            m_arrowListGO = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.ArrowListGO);

            if (m_cardHandAreaWidthArr[0] < 0)
            {
                m_cardHandAreaWidthArr[0] = (-m_cardHandAreaWidthArr[0]);
            }
            if (m_cardHandAreaWidthArr[1] < 0)
            {
                m_cardHandAreaWidthArr[1] = (-m_cardHandAreaWidthArr[1]);
            }

            if (m_cardCommonAreaWidthArr[0] < 0)
            {
                m_cardCommonAreaWidthArr[0] = (-m_cardCommonAreaWidthArr[0]);
            }
            if (m_cardCommonAreaWidthArr[1] < 0)
            {
                m_cardCommonAreaWidthArr[1] = (-m_cardCommonAreaWidthArr[1]);
            }
        }
    }
}