using SDK.Lib;
using System;

namespace FightCore
{
    public class EnemyDZArea : SceneDZArea
    {
        public EnemyDZArea(SceneDZData sceneDZData, EnDZPlayer playerSide)
            : base(sceneDZData, playerSide)
        {
            m_outSceneCardList = new EnemyOutSceneCardList(m_sceneDZData, m_playerSide);
            m_inSceneCardList = new EnemyInSceneCardList(m_sceneDZData, m_playerSide);

            m_outSceneCardList.centerPos = m_sceneDZData.m_placeHolderGo.m_cardCenterGOArr[(int)m_playerSide, (int)CardArea.CARDCELLTYPE_COMMON].transform;
            m_outSceneCardList.radius = m_sceneDZData.m_placeHolderGo.m_cardCommonAreaWidthArr[(int)m_playerSide];

            m_inSceneCardList.centerPos = m_sceneDZData.m_placeHolderGo.m_cardCenterGOArr[(int)m_playerSide, (int)CardArea.CARDCELLTYPE_HAND].transform;
            m_inSceneCardList.radius = m_sceneDZData.m_placeHolderGo.m_cardHandAreaWidthArr[(int)m_playerSide];
        }
    }
}