using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    public class SkillCard : NotOutCard
    {
        public SkillCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_clickControl = new SkillClickControl(this);
            m_sceneCardBaseData.m_trackAniControl = new SkillAniControl(this);
            m_sceneCardBaseData.m_dragControl = new SkillDragControl(this);
            m_sceneCardBaseData.m_behaviorControl = new SkillBehaviorControl(this);

            m_render = new EquipSkillRender(this);
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }

        override public void setBaseInfo(EnDZPlayer m_playerFlag, CardArea area, CardType cardType)
        {
            UtilApi.setPos(this.transform(), m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)area].transform.localPosition);
        }

        override public void updateCardDataChangeBySvr(t_Card svrCard_ = null)
        {

        }

        override public void updateCardDataNoChangeByTable()
        {

        }
    }
}