using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 随从卡
     */
    public class AttendCard : SceneCard
    {
        public AttendCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_clickControl = new AttendClickControl(this);
            m_sceneCardBaseData.m_aniControl = new AttendAniControl(this);
            m_sceneCardBaseData.m_dragControl = new AttendDragControl(this);
            m_sceneCardBaseData.m_behaviorControl = new AttendBehaviorControl(this);

            m_render = new SceneCardPlayerRender();
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }
    }
}