using SDK.Common;

namespace Game.UI
{
    public class EnemyDZArea : SceneDZArea
    {
        public EnemyDZArea(SceneDZData sceneDZData, EnDZPlayer playerFlag)
            : base(sceneDZData, playerFlag)
        {
            m_centerHero.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyHero));
            m_inSceneCardList = new EnemyInSceneCardList(m_sceneDZData, m_playerFlag);
        }

        // 给 enemy 可攻击的对象添加可攻击标识
        public void addAttackTargetFlags(EnGameOp gameOp)
        {

        }
    }
}