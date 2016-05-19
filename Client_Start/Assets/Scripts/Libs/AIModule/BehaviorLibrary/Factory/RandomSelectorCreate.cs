using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 类组件的创建
     */
    public class RandomSelectorCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            RandomSelector randomSelector = new RandomSelector();
            return randomSelector;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}