using SDK.Lib;

namespace Game.UI
{
    public class SecretCard : SceneCardBase
    {
        public SecretCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_clickControl = new SecretClickControl(this);
            m_aniControl = new SecretAniControl(this);
            m_dragControl = new SecretDragControl(this);
            m_behaviorControl = new SecretBehaviorControl(this);

            m_render = new SceneCardPlayerRender();
            m_effectControl = new EffectControl(this);
        }
    }
}