using SDK.Common;

namespace FightCore
{
    public class SelfSkillBehaviorControl : SkillBehaviorControl
    {
        public SelfSkillBehaviorControl(SceneCardBase rhv) : 
            base(rhv)
        {

        }

        override public bool canLaunchAtt()
        {
            return (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].checkMp((int)this.m_card.sceneCardItem.svrCard.mpcost) && this.m_card.sceneCardItem.checkAttackTimes());
        }
    }
}