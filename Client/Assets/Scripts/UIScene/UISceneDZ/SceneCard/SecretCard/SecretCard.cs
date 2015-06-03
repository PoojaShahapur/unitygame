using SDK.Lib;

namespace Game.UI
{
    public class SecretCard : SceneCard
    {
        public SecretCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_clickControl = new SecretClickControl(this);
            m_sceneCardBaseData.m_aniControl = new SecretAniControl(this);
            m_sceneCardBaseData.m_dragControl = new SecretDragControl(this);
            m_sceneCardBaseData.m_behaviorControl = new SecretBehaviorControl(this);

            m_render = new SceneCardPlayerRender(this);
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }
    }
}