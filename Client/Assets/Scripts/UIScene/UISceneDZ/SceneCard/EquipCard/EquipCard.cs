using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 武器卡
     */
    public class EquipCard : SceneCard
    {
        public EquipCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_clickControl = new EquipClickControl(this);
            m_sceneCardBaseData.m_aniControl = new EquipAniControl(this);
            m_sceneCardBaseData.m_dragControl = new EquipDragControl(this);
            m_sceneCardBaseData.m_behaviorControl = new EquipBehaviorControl(this);

            m_render = new EquipSkillRender(this);
            m_sceneCardBaseData.m_effectControl = new EffectControl(this);
        }

        override public void setBaseInfo(EnDZPlayer m_playerFlag, CardArea area, CardType cardType)
        {
            this.transform().localPosition = m_sceneDZData.m_cardCenterGOArr[(int)m_playerFlag, (int)area].transform.localPosition;
        }
    }
}