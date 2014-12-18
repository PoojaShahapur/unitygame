using Game.UI;
using SDK.Common;

namespace Game.Game
{
    /**
     * @brief UI 创建工厂
     */
    public class GameUIFactory : IUIFactory
    {
        public IForm CreateForm(UIFormID id)
        {
            if(UIFormID.UIBackPack == id)
            {
                return new UIPack();
            }

            return null;
        }
    }
}