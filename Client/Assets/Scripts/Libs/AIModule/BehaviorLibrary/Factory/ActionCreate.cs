using System;
using System.Collections.Generic;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 一类组件的创建
     */
    public class ActionCreate : ComponentCreate
    {
        public ActionCreate()
        {
            regHandle();
        }

        protected void regHandle()
        {
            // Actions 组件注册
            m_id2CreateDic["Wander"] = createActionWander;
            m_id2BuildDic["Wander"] = buildActionWander;

            m_id2CreateDic["Follow"] = createActionFollow;
            m_id2BuildDic["Follow"] = buildActionFollow;

            m_id2CreateDic["AttackAll"] = createActionAttackAll;
            m_id2BuildDic["AttackAll"] = buildActionAttackAll;
        }

        public BehaviorComponent createActionWander()
        {
            ActionWander actionWander = new ActionWander();
            return actionWander;
        }

        public void buildActionWander(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }

        public BehaviorComponent createActionFollow()
        {
            ActionFollow actionFollow = new ActionFollow();
            return actionFollow;
        }

        public void buildActionFollow(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }

        public BehaviorComponent createActionAttackAll()
        {
            ActionAttackAll actionAttackAll = new ActionAttackAll();
            return actionAttackAll;
        }

        public void buildActionAttackAll(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}