namespace BehaviorLibrary.Components
{
    public class BehaviorComponent
    {
        protected BehaviorReturnCode ReturnCode;
        protected BehaviorTree m_behaviorTree;

        public BehaviorComponent() { }

        // 第一次进入调用
        public virtual void onEnter()
        {

        }

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
    }
}