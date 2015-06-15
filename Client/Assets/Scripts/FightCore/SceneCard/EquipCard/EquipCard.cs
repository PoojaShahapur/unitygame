﻿using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 武器卡
     */
    public class EquipCard : NotOutCard
    {
        public EquipCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_clickControl = new EquipClickControl(this);
            m_sceneCardBaseData.m_trackAniControl = new EquipAniControl(this);
            m_sceneCardBaseData.m_dragControl = new EquipDragControl(this);
            m_sceneCardBaseData.m_behaviorControl = new EquipBehaviorControl(this);

            m_render = new EquipSkillRender(this);
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }

        override public void updateCardDataChangeBySvr(t_Card svrCard_ = null)
        {

        }

        override public void updateCardDataNoChangeByTable()
        {

        }
    }
}