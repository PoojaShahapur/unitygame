using Game.Msg;

namespace SDK.Lib
{
    /**
     * @brief 战斗消息统一处理的地方
     */
    public class ImmeFightMsgMgr
    {
        public ImmeFightMsgMgr()
        {
            
        }

        // 将一个战斗消息分解成客户端自己的战斗流程
        public void psSvrFightMsg()
        {
            int actId = 1000;         // 动作的 Id
            OneAttackFlowSeq attackSeq = Ctx.m_instance.m_skillAttackFlowMgr.getOneAttackFlowSeq(actId.ToString());         // 根据动作 Id 获取动作序列
            OneHurtFlowSeq hurtSeq = Ctx.m_instance.m_skillAttackFlowMgr.getOneHurtFlowSeq(actId.ToString());         // 根据动作 Id 获取被击动作序列

            uint attackThisId = 1000;   // 攻击者 ThisId
            uint hurtThisId = 1000;     // 被击者 ThisId
            Player attackPlayer = Ctx.m_instance.m_playerMgr.getPlayerByThisId(attackThisId);   // 获取攻击者
            Player hurtPlayer = Ctx.m_instance.m_playerMgr.getPlayerByThisId(hurtThisId);       // 获取受伤者

            ImmeSkillAttackItem attackItem = new ImmeSkillAttackItem(EImmeAttackType.eSkill);        // 保存客户端的攻击数据
            attackSeq.attackItem = attackItem;

            ImmeSkillHurtItem hurtItem = new ImmeSkillHurtItem(EImmeHurtType.eSkill);
            hurtSeq.hurtItem = hurtItem;
        }
    }
}