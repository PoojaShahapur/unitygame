using SDK.Common;

namespace Game.UI
{
    public class SelfDZArea : SceneDZArea
    {
        public SelfDZArea()
        {
            m_hero.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfHero));
        }
    }
}