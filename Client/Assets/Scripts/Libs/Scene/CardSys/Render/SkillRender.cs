using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 装备卡、技能卡，渲染器
     */
    public class SkillRender : EquipSkillRenderBase
    {
        public SkillRender(SceneEntityBase entity_) :
            base(entity_)
        {
            m_boxModelPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/SkillCardBox.prefab");
        }
    }
}