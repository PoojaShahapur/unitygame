using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 一类组件的创建
     */
    public class RepeatUntilFailCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            RepeatUntilFail repeatUntilFail = new RepeatUntilFail(null);
            return repeatUntilFail;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}