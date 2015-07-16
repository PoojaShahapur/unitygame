using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 技能动作配置资源
     */
    public class SkillActionRes : InsResBase
    {
        protected AttackActionNode m_attackActionNode;      // 攻击动作的流程

        public SkillActionRes()
        {

        }

        override public void init(ResItem res)
        {
            string text = res.getText(GetPath());
            Ctx.m_instance.m_aiSystem.behaviorTreeMgr.parseXml(text);

            base.init(res);
        }

        override public void failed(ResItem res)
        {
            base.failed(res);
        }

        override public void unload()
        {
            base.unload();
        }
    }
}