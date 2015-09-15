using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 一类组件的创建
     */
    public class TimerCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            Timer timer = new Timer(null, 0, null);
            return timer;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}