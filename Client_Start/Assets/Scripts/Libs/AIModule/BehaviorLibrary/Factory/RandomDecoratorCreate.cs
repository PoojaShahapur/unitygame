using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 一类组件的创建
     */
    public class RandomDecoratorCreate : ComponentCreate
    {
        override protected BehaviorComponent createDefault()
        {
            RandomDecorator randomDecorator = new RandomDecorator(0, null, null);
            return randomDecorator;
        }

        override protected void buildDefault(BehaviorComponent btCmt, SecurityElement btNode)
        {

        }
    }
}