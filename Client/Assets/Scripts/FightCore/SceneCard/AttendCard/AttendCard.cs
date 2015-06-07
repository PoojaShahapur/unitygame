﻿using SDK.Lib;

namespace FightCore
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
            m_sceneCardBaseData.m_trackAniControl = new AttendAniControl(this);
            m_sceneCardBaseData.m_dragControl = new AttendDragControl(this);
            m_sceneCardBaseData.m_behaviorControl = new AttendBehaviorControl(this);

            m_render = new SceneCardPlayerRender(this);
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }
    }
}