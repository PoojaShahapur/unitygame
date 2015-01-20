using SDK.Common;

namespace Game.UI
{
    public class EnemyDZArea : SceneDZArea
    {
        public EnemyDZArea()
        {
            m_hero.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyHero));
        }
    }
}