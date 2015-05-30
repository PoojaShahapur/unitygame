using SDK.Common;
using System;

namespace Game.UI
{
    public class EnemyDZArea : SceneDZArea
    {
        public EnemyDZArea(SceneDZData sceneDZData, EnDZPlayer playerFlag)
            : base(sceneDZData, playerFlag)
        {
            m_centerHero.gameObject = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyHero);
            m_inSceneCardList = new EnemyInSceneCardList(m_sceneDZData, m_playerFlag);
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