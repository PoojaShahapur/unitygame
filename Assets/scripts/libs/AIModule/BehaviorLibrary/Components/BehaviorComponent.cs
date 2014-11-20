using SDK.Common;

namespace BehaviorLibrary.Components
{
    public class BehaviorComponent
    {
        protected BehaviorReturnCode ReturnCode;
        protected BehaviorTree m_behaviorTree;

        public BehaviorComponent() { }

        public BehaviorTree behaviorTree
        {
            get
            {
                return m_behaviorTree;
            }
            set
            {
                m_behaviorTree = value;
            }
        }

        // 第一次进入调用
        public virtual void onEnter()
        {

        }

        // 更新
        public virtual BehaviorReturnCode Behave()
        {
            return BehaviorReturnCode.Failure;
        }

        // 退出的时候调用
        public virtual void onExit()
        {

        }

        // 添加子节点
        public virtual void addChild(BehaviorComponent child)
        {

        }

        public void toggleBehavior(BehaviorState bs)
        {
            if (m_behaviorTree.inputParam.beingEntity.aiController.aiLocalState.behaviorState != bs)
            {
                m_behaviorTree.inputParam.beingEntity.aiController.aiLocalState.behaviorState = bs;
                onEnter();
            }
        }
    }
}