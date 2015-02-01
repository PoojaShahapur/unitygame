using SDK.Common;

namespace Game.UI
{
    public class EnemyDZArea : SceneDZArea
    {
        public EnemyDZArea(SceneDZData sceneDZData, EnDZPlayer playerFlag)
            : base(sceneDZData, playerFlag)
        {
            m_hero.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyHero));
            m_inSceneCardList = new EnemyInSceneCardList(m_sceneDZData, m_playerFlag);
        }
    }
}