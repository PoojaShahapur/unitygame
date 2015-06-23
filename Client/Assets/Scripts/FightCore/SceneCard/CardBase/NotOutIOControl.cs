namespace FightCore
{
    public class NotOutIOControl : ExceptBlackIOControl
    {
        public NotOutIOControl(SceneCardBase rhv) : 
            base(rhv)
        {
            
        }

        // 开启拖动
        override public void enableDrag()
        {

        }

        // 关闭拖放功能
        override public void disableDrag()
        {

        }
    }
}