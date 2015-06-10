using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 不能移动到场景的卡牌，主要是英雄卡、技能卡、装备卡，不包括随从卡、法术卡
     */
    public class NotOutCard : SceneCard
    {
        public NotOutCard(SceneDZData data) : 
            base(data)
        {

        }

        override public void setBaseInfo(EnDZPlayer m_playerFlag, CardArea area, CardType cardType)
        {
            UtilApi.setPos(this.transform(), m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)area].transform.localPosition);
        }
    }
}