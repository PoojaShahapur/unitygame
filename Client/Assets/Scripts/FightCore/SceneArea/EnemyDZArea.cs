using SDK.Common;
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
        }

        // 给 enemy 可攻击的对象添加可攻击标识
        override public void updateCardAttackedState(GameOpState opt)
        {
            outSceneCardList.updateCardAttackedState(opt);
        }

        override public void clearCardAttackedState()
        {
            outSceneCardList.updateCardOutState(false);
        }
    }
}