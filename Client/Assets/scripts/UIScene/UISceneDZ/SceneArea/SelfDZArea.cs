using SDK.Common;

namespace Game.UI
{
    public class SelfDZArea : SceneDZArea
    {
        public SelfDZArea(SceneDZData sceneDZData, EnDZPlayer playerFlag)
            : base(sceneDZData, playerFlag)
        {
            m_hero.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfHero));
            m_inSceneCardList = new SelfInSceneCardList(m_sceneDZData, m_playerFlag);
        }
    }
}