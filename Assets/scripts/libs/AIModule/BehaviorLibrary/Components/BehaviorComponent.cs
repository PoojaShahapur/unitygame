namespace BehaviorLibrary.Components
{
    public class BehaviorComponent
    {
        protected BehaviorReturnCode ReturnCode;

        public BehaviorComponent() { }

        // 第一次进入调用
        public virtual void onEnter()
        {

        }

        // 更新
        public virtual BehaviorReturnCode Behave(InsParam inputParam)
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